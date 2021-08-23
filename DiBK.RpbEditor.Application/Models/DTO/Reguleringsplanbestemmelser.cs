using System;
using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public class Reguleringsplanbestemmelser
    {
        public NasjonalArealplanId NasjonalArealplanId { get; set; }
        public Kode Plantype { get; set; }
        public string Plannavn { get; set; }
        public Kode Lovreferanse { get; set; }
        public DateTime? Versjonsdato { get; set; }
        public string Versjonsnummer { get; set; }
        public string AlternativReferanse { get; set; }
        public Planhensikt Planhensikt { get; set; }
        public List<Fellesbestemmelse> Fellesbestemmelser { get; set; }
        public List<KravOmDetaljregulering> KravOmDetaljregulering { get; set; }
        public List<Formålsbestemmelse> Formålsbestemmelser { get; set; }
        public List<Hensynsbestemmelse> Hensynsbestemmelser { get; set; }
        public List<Områdebestemmelse> Områdebestemmelser { get; set; }
        public List<Rekkefølgebestemmelse> Rekkefølgebestemmelser { get; set; }
        public List<JuridiskDokument> JuridiskeDokumenter { get; set; }
    }
}
