using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public class Områdebestemmelse : Planbestemmelse
    {
        public List<string> BestemmelseOmrådenavn { get; set; }
        public List<string> Feltnavn { get; set; }
        public List<string> Objektnavn { get; set; }
    }
}
