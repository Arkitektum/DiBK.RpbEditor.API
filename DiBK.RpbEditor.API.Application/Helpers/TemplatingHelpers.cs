using System.Collections.Generic;
using System.Linq;

namespace DiBK.RpbEditor.API.Application.Helpers
{
    public static class TemplatingHelpers
    {
        public static bool HasValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool HasValue(IEnumerable<object> value)
        {
            return value?.Any() ?? false;
        }
    }
}
