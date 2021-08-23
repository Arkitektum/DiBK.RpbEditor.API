using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public class KravOmDetaljregulering
    {
        public List<string> Feltnavn { get; set; }
        public FormatertTekst KravTilDetaljreguleringen { get; set; }
        public List<string> BestemmelseOmrådenavn { get; set; }
        public List<string> HensynSonenavn { get; set; }
    }
}
