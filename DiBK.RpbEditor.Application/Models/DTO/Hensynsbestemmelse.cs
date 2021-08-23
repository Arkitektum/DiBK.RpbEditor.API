using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public class Hensynsbestemmelse : Planbestemmelse
    {
        public Kode Hensynskategori { get; set; }
        public List<string> HensynSonenavn { get; set; }
    }
}
