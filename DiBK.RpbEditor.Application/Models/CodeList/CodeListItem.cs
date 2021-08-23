namespace DiBK.RpbEditor.Application.Models.CodeList
{
    public class CodeListItem
    {
        public string Value { get; set; }
        public string Label { get; set; }

        public CodeListItem()
        {
        }

        public CodeListItem(string value, string label)
        {
            Value = value;
            Label = label;
        }
    }
}
