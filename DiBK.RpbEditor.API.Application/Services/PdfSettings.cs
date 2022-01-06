namespace DiBK.RpbEditor.Application.Services
{
    public class PdfSettings
    {
        public static string SectionName => "PdfSettings";
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public PaperSettings Paper { get; set; } = new PaperSettings();

        public class PaperSettings
        {
            public string PaperWidth { get; set; }
            public string PaperHeight { get; set; }
            public string MarginTop { get; set; }
            public string MarginRight { get; set; }
            public string MarginBottom { get; set; }
            public string MarginLeft { get; set; }
        }
    }
}
