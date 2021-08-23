using System;
using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.CodeList
{
    public class CodeList
    {
        public List<CodeListItem> CodeListItems { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
