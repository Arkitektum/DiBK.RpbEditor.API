using AutoMapper;
using DiBK.RpbEditor.Application.Exceptions;
using DiBK.RpbEditor.Application.Models.DTO;
using DiBK.RpbEditor.Application.Utils;
using SOSI.Produktspesifikasjon.Reguleringsplanforslag.Planbestemmelser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DiBK.RpbEditor.Application.Services
{
    public class ConverterService : IConverterService
    {
        private readonly ITemplatingService _templatingService;
        private readonly ICodeListService _codeListService;
        private readonly IPdfService _pdfService;
        private readonly IMapper _mapper;

        public ConverterService(
            ITemplatingService templatingService,
            ICodeListService codeListService,
            IPdfService pdfService,
            IMapper mapper)
        {
            _templatingService = templatingService;
            _codeListService = codeListService;
            _pdfService = pdfService;
            _mapper = mapper;
        }

        public Reguleringsplanbestemmelser FromXml(Stream xmlStream)
        {
            ReguleringsplanbestemmelserType model;

            try
            {
                model = DeserializeXML<ReguleringsplanbestemmelserType>(xmlStream);
            }
            catch (Exception exception)
            {
                throw new CouldNotDeserializeXmlException("Ugyldig XML", exception);
            }

            return _mapper.Map<Reguleringsplanbestemmelser>(model);
        }

        public async Task<string> ToXml(Reguleringsplanbestemmelser reguleringsplanbestemmelser)
        {
            SetSortOrder(reguleringsplanbestemmelser);
            await SetCodeListDescriptions(reguleringsplanbestemmelser);

            var mapped = _mapper.Map<ReguleringsplanbestemmelserType>(reguleringsplanbestemmelser);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("n1", "http://skjema.geonorge.no/SOSI/produktspesifikasjon/Reguleringsplanforslag/5.0/Planbestemmelser");

            return SerializeXML(mapped, namespaces);
        }

        public async Task<string> ToHtml(Reguleringsplanbestemmelser reguleringsplanbestemmelser)
        {
            SetSortOrder(reguleringsplanbestemmelser);
            await SetCodeListDescriptions(reguleringsplanbestemmelser);

            return await _templatingService.RenderViewAsync(
                "Planbestemmelser.Planbestemmelser.cshtml",
                reguleringsplanbestemmelser
            );
        }

        public async Task<string> ToHtml(Stream xmlStream)
        {
            var model = FromXml(xmlStream);

            return await ToHtml(model);
        }

        public async Task<byte[]> ToPdf(Reguleringsplanbestemmelser reguleringsplanbestemmelser)
        {
            SetSortOrder(reguleringsplanbestemmelser);
            await SetCodeListDescriptions(reguleringsplanbestemmelser);

            var html = await ToHtml(reguleringsplanbestemmelser);

            return await _pdfService.GeneratePdf(html);
        }

        public async Task<byte[]> ToPdf(Stream xmlStream)
        {
            var model = FromXml(xmlStream);

            return await ToPdf(model);
        }

        private static string SerializeXML<T>(T model, XmlSerializerNamespaces namespaces) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));

            using var writer = new Utf8StringWriter();
            serializer.Serialize(writer, model, namespaces);

            return writer.ToString();
        }

        private static T DeserializeXML<T>(Stream stream) where T : class
        {
            if (stream == null)
                return null;

            using var reader = XmlReader.Create(stream);
            reader.MoveToContent();

            var serializer = new XmlSerializer(typeof(T));

            return serializer.Deserialize(reader) as T;
        }

        private static void SetSortOrder(Reguleringsplanbestemmelser reguleringsplanbestemmelser)
        {
            var planbestemmelser = (reguleringsplanbestemmelser.Fellesbestemmelser ?? new List<Fellesbestemmelse>())
                .Concat<Planbestemmelse>(reguleringsplanbestemmelser.Formålsbestemmelser ?? new List<Formålsbestemmelse>())
                .Concat(reguleringsplanbestemmelser.Hensynsbestemmelser ?? new List<Hensynsbestemmelse>())
                .Concat(reguleringsplanbestemmelser.Områdebestemmelser ?? new List<Områdebestemmelse>())
                .Concat(reguleringsplanbestemmelser.Rekkefølgebestemmelser ?? new List<Rekkefølgebestemmelse>())
                .ToList();

            for (int i = 0; i < planbestemmelser.Count; i++)
                planbestemmelser[i].Sorteringsrekkefølge = i + 1;
        }

        private async Task SetCodeListDescriptions(Reguleringsplanbestemmelser reguleringsplanbestemmelser)
        {
            var codeLists = await _codeListService.GetCodeLists();

            reguleringsplanbestemmelser.Plantype.Kodebeskrivelse = codeLists.Plantyper
                .SingleOrDefault(plantype => plantype.Value == reguleringsplanbestemmelser.Plantype.Kodeverdi)?.Label;

            reguleringsplanbestemmelser.Lovreferanse.Kodebeskrivelse = codeLists.Lovreferanser
                .SingleOrDefault(lovreferanse => lovreferanse.Value == reguleringsplanbestemmelser.Lovreferanse.Kodeverdi)?.Label;

            foreach (var bestemmelse in reguleringsplanbestemmelser.Fellesbestemmelser)
            {
                bestemmelse.Tekst.TekstFormat.Kodebeskrivelse = codeLists.Tekstformat
                    .SingleOrDefault(tekstformat => tekstformat.Value == bestemmelse.Tekst.TekstFormat.Kodeverdi)?.Label;
            }

            foreach (var krav in reguleringsplanbestemmelser.KravOmDetaljregulering)
            {
                krav.KravTilDetaljreguleringen.TekstFormat.Kodebeskrivelse = codeLists.Tekstformat
                    .SingleOrDefault(tekstformat => tekstformat.Value == krav.KravTilDetaljreguleringen.TekstFormat.Kodeverdi)?.Label;
            }

            foreach (var bestemmelse in reguleringsplanbestemmelser.Formålsbestemmelser)
            {
                bestemmelse.Tekst.TekstFormat.Kodebeskrivelse = codeLists.Tekstformat
                    .SingleOrDefault(tekstformat => tekstformat.Value == bestemmelse.Tekst.TekstFormat.Kodeverdi)?.Label;

                bestemmelse.GjelderHovedformål.Kodebeskrivelse = codeLists.Hovedformål
                    .SingleOrDefault(hovedformål => hovedformål.Value == bestemmelse.GjelderHovedformål.Kodeverdi)?.Label;
            }

            foreach (var bestemmelse in reguleringsplanbestemmelser.Hensynsbestemmelser)
            {
                bestemmelse.Tekst.TekstFormat.Kodebeskrivelse = codeLists.Tekstformat
                    .SingleOrDefault(tekstformat => tekstformat.Value == bestemmelse.Tekst.TekstFormat.Kodeverdi)?.Label;

                bestemmelse.Hensynskategori.Kodebeskrivelse = codeLists.Hensynskategorier
                    .SingleOrDefault(hensynskategori => hensynskategori.Value == bestemmelse.Hensynskategori.Kodeverdi)?.Label;
            }

            foreach (var bestemmelse in reguleringsplanbestemmelser.Områdebestemmelser)
            {
                bestemmelse.Tekst.TekstFormat.Kodebeskrivelse = codeLists.Tekstformat
                    .SingleOrDefault(tekstformat => tekstformat.Value == bestemmelse.Tekst.TekstFormat.Kodeverdi)?.Label;
            }

            foreach (var bestemmelse in reguleringsplanbestemmelser.Rekkefølgebestemmelser)
            {
                bestemmelse.Tekst.TekstFormat.Kodebeskrivelse = codeLists.Tekstformat
                    .SingleOrDefault(tekstformat => tekstformat.Value == bestemmelse.Tekst.TekstFormat.Kodeverdi)?.Label;

                bestemmelse.Rekkefølgeangivelse.Kodebeskrivelse = codeLists.Rekkefølgeangivelser
                    .SingleOrDefault(angivelse => angivelse.Value == bestemmelse.Rekkefølgeangivelse.Kodeverdi)?.Label;
            }
        }
    }
}
