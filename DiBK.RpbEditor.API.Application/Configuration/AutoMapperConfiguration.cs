using AutoMapper;
using DiBK.RpbEditor.Application.Models.DTO;
using SOSI.Produktspesifikasjon.Reguleringsplanforslag.Planbestemmelser;

namespace DiBK.RpbEditor.Application.Configuration
{
    public class DtoToXmlClassProfile : Profile
    {
        public DtoToXmlClassProfile()
        {
            ReplaceMemberName("ø", "oe");
            ReplaceMemberName("å", "aa");

            CreateMap<Reguleringsplanbestemmelser, ReguleringsplanbestemmelserType>()
                .ForMember(dest => dest.Fellesbestemmelse, opt => opt.MapFrom(src => src.Fellesbestemmelser))
                .ForMember(dest => dest.KravOmDetaljregulering, opt => opt.MapFrom(src => src.KravOmDetaljregulering))
                .ForMember(dest => dest.Formaalsbestemmelse, opt => opt.MapFrom(src => src.Formålsbestemmelser))
                .ForMember(dest => dest.Hensynsbestemmelse, opt => opt.MapFrom(src => src.Hensynsbestemmelser))
                .ForMember(dest => dest.Omraadebestemmelse, opt => opt.MapFrom(src => src.Områdebestemmelser))
                .ForMember(dest => dest.Rekkefoelgebestemmelse, opt => opt.MapFrom(src => src.Rekkefølgebestemmelser))
                .ForMember(dest => dest.JuridiskDokument, opt => opt.MapFrom(src => src.JuridiskeDokumenter))
                .ForMember(dest => dest.VersjonsdatoValue, opt => opt.MapFrom(src => src.Versjonsdato));

            CreateMap<NasjonalArealplanId, NasjonalArealplanIdType>();

            CreateMap<AdministrativEnhet, AdministrativEnhetskodeType>();

            CreateMap<Kode, KodeType>();

            CreateMap<FormatertTekst, FormatertTekstType>()
                .ForPath(dest => dest.TekstFormat.Kodeverdi, src => src.MapFrom(src => "html"))
                .ForPath(dest => dest.TekstFormat.Kodebeskrivelse, src => src.MapFrom(src => "HTML"));

            CreateMap<Planhensikt, PlanensHensiktType>()
                .ForMember(dest => dest.VersjonsdatoValue, opt => opt.MapFrom(src => src.Versjonsdato));

            CreateMap<Fellesbestemmelse, FellesbestemmelseType>()
                .ForMember(dest => dest.VersjonsdatoValue, opt => opt.MapFrom(src => src.Versjonsdato));

            CreateMap<KravOmDetaljregulering, KravOmDetaljreguleringType>();

            CreateMap<Formålsbestemmelse, BestemmelseArealformaalType>()
                .ForMember(dest => dest.VersjonsdatoValue, opt => opt.MapFrom(src => src.Versjonsdato));

            CreateMap<Hensynsbestemmelse, BestemmelseHensynssoneType>()
                .ForMember(dest => dest.VersjonsdatoValue, opt => opt.MapFrom(src => src.Versjonsdato));

            CreateMap<Områdebestemmelse, BestemmelseOmraadeType>()
                .ForMember(dest => dest.VersjonsdatoValue, opt => opt.MapFrom(src => src.Versjonsdato));

            CreateMap<Rekkefølgebestemmelse, BestemmelseRekkefoelgeType>()
                .ForMember(dest => dest.VersjonsdatoValue, opt => opt.MapFrom(src => src.Versjonsdato));

            CreateMap<JuridiskDokument, JuridiskBindendeDokumentType>()
                .ForMember(dest => dest.DokumentetsDatoValue, opt => opt.MapFrom(src => src.DokumentetsDato));
        }
    }

    public class XmlClassToDtoProfile : Profile
    {
        public XmlClassToDtoProfile()
        {
            ReplaceMemberName("oe", "ø");
            ReplaceMemberName("aa", "å");

            CreateMap<ReguleringsplanbestemmelserType, Reguleringsplanbestemmelser>()
                .ForMember(dest => dest.Fellesbestemmelser, opt => opt.MapFrom(src => src.Fellesbestemmelse))
                .ForMember(dest => dest.KravOmDetaljregulering, opt => opt.MapFrom(src => src.KravOmDetaljregulering))
                .ForMember(dest => dest.Formålsbestemmelser, opt => opt.MapFrom(src => src.Formaalsbestemmelse))
                .ForMember(dest => dest.Hensynsbestemmelser, opt => opt.MapFrom(src => src.Hensynsbestemmelse))
                .ForMember(dest => dest.Områdebestemmelser, opt => opt.MapFrom(src => src.Omraadebestemmelse))
                .ForMember(dest => dest.Rekkefølgebestemmelser, opt => opt.MapFrom(src => src.Rekkefoelgebestemmelse))
                .ForMember(dest => dest.JuridiskeDokumenter, opt => opt.MapFrom(src => src.JuridiskDokument))
                .ForMember(dest => dest.Plantype, opt => opt.NullSubstitute(new KodeType { Kodeverdi = "34" }))
                .ForMember(dest => dest.Lovreferanse, opt => opt.NullSubstitute(new KodeType { Kodeverdi = "6" }));

            CreateMap<NasjonalArealplanIdType, NasjonalArealplanId>();

            CreateMap<AdministrativEnhetskodeType, AdministrativEnhet>();

            CreateMap<KodeType, Kode>();

            CreateMap<FormatertTekstType, FormatertTekst>()
                .ForPath(dest => dest.TekstFormat.Kodeverdi, src => src.MapFrom(src => "html"))
                .ForPath(dest => dest.TekstFormat.Kodebeskrivelse, src => src.MapFrom(src => "HTML"));

            CreateMap<PlanensHensiktType, Planhensikt>()
                .ForMember(dest => dest.Tekst, opt => opt.NullSubstitute(new FormatertTekstType()));

            CreateMap<FellesbestemmelseType, Fellesbestemmelse>()
                .ForMember(dest => dest.Tekst, opt => opt.NullSubstitute(new FormatertTekstType()));

            CreateMap<KravOmDetaljreguleringType, KravOmDetaljregulering>()
                .ForMember(dest => dest.KravTilDetaljreguleringen, opt => opt.NullSubstitute(new FormatertTekstType()));

            CreateMap<BestemmelseArealformaalType, Formålsbestemmelse>()
                .ForMember(dest => dest.Tekst, opt => opt.NullSubstitute(new FormatertTekstType()))
                .ForMember(dest => dest.GjelderHovedformål, opt => opt.NullSubstitute(new KodeType { Kodeverdi = "1" }));

            CreateMap<BestemmelseHensynssoneType, Hensynsbestemmelse>()
                .ForMember(dest => dest.Tekst, opt => opt.NullSubstitute(new FormatertTekstType()))
                .ForMember(dest => dest.Hensynskategori, opt => opt.NullSubstitute(new KodeType { Kodeverdi = "5" }));

            CreateMap<BestemmelseOmraadeType, Områdebestemmelse>()
                .ForMember(dest => dest.Tekst, opt => opt.NullSubstitute(new FormatertTekstType()));

            CreateMap<BestemmelseRekkefoelgeType, Rekkefølgebestemmelse>()
                .ForMember(dest => dest.Tekst, opt => opt.NullSubstitute(new FormatertTekstType()))
                .ForMember(dest => dest.Rekkefølgeangivelse, opt => opt.NullSubstitute(new KodeType { Kodeverdi = "forRammetillatelse" }));

            CreateMap<JuridiskBindendeDokumentType, JuridiskDokument>();
        }
    }
}
