using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public class Formålsbestemmelse : Planbestemmelse
    {
        public Kode GjelderHovedformål { get; set; }
        public bool FellesForHovedformål { get; set; }
        public List<string> Feltnavn { get; set; }
        public List<string> BestemmelseOmrådenavn { get; set; }
        public List<string> Objektnavn { get; set; }
    }
}
