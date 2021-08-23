using System;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public abstract class Planbestemmelse
    {
        public string Nummerering { get; set; }
        public string Overskrift { get; set; }
        public FormatertTekst Tekst { get; set; }
        public int Sorteringsrekkefølge { get; set; }
        public DateTime? Versjonsdato { get; set; }
        public string Versjonsnummer { get; set; }
        public string AlternativReferanse { get; set; }
    }
}
