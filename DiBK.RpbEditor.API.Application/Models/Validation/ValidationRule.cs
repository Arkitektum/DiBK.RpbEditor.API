using System.Collections.Generic;

namespace DiBK.RpbEditor.Application.Models.Validation
{
    public class ValidationRule
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Documentation { get; set; }
        public string MessageType { get; set; }
        public string Status { get; set; }
        public List<ValidationRuleMessage> Messages { get; set; }
    }

    public class ValidationRuleMessage
    {
        public string Message { get; set; }
    }
}
