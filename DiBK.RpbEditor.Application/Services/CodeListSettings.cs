namespace DiBK.RpbEditor.Application.Services
{
    public class CodeListSettings
    {
        public static readonly string SectionName = "CodeListSettings";
        public DataSource Hensynskategori { get; set; }
        public DataSource Hovedformål { get; set; }
        public DataSource Plantype { get; set; }
        public DataSource Rekkefølgeangivelse { get; set; }
        public DataSource Tekstformat { get; set; }

        public class DataSource
        {
            public string Url { get; set; }
            public int CacheDays { get; set; }
        }
    }
}
