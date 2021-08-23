using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public class Rekkefølgebestemmelse : Planbestemmelse
    {
        public List<string> BestemmelseOmrådenavn { get; set; }
        public List<string> Feltnavn { get; set; }
        public List<string> HensynSonenavn { get; set; }
        public Kode Rekkefølgeangivelse { get; set; }
        public List<string> Objektnavn { get; set; }
    }
}
