using System;

namespace DiBK.RpbEditor.Application.Models.DTO
{
    public class JuridiskDokument
    {
        public string Tittel { get; set; }
        public string Dokumentreferanse { get; set; }
        public string Sammendrag { get; set; }
        public string Rapportnummer { get; set; }
        public DateTime? DokumentetsDato { get; set; }
    }
}
