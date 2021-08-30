using Microsoft.AspNetCore.Html;
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

        public static HtmlString RenderHeader(string numbering, string header)
        {
            var pointCount = numbering.Split(".").Length - 1;
            var headerHtml = GetHeaderHtml(pointCount, header);

            return new HtmlString(headerHtml);
        }

        private static string GetHeaderHtml(int pointCount, string header)
        {
            return pointCount switch
            {
                0 => $"<h2>{header}</h2>",
                1 => $"<h3>{header}</h3>",
                2 => $"<h4>{header}</h4>",
                3 => $"<h5>{header}</h5>",
                4 => $"<h6>{header}</h6>",
                _ => $"<h6>{header}</h6>"
            };
        }
    }
}
