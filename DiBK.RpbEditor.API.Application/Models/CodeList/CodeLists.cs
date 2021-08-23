using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.CodeList
{
    public class CodeLists
    {
        public List<CodeListItem> Hensynskategorier { get; set; }
        public List<CodeListItem> Hovedformål { get; set; }
        public List<CodeListItem> Lovreferanser { get; set; }
        public List<CodeListItem> Plantyper { get; set; }
        public List<CodeListItem> Rekkefølgeangivelser { get; set; }
        public List<CodeListItem> Tekstformat { get; set; }
    }
}
