using DiBK.RpbEditor.Application.Models.DTO;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiBK.RpbEditor.API.Application.Helpers
{
    public static class TemplatingHelpers
    {
        private static readonly Regex _urlRegex = 
            new(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", RegexOptions.Compiled);

        public static bool HasValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool HasItems(IEnumerable<object> value)
        {
            return value?.Any() ?? false;
        }

        public static bool IsUrl(string value)
        {
            return _urlRegex.IsMatch(value);
        }

        public static HtmlString RenderHeader(string numbering, string headerText, IEnumerable<string> fieldNames = null)
        {
            var headerHtml = GetHeaderHtml(numbering, headerText, fieldNames);

            return new HtmlString(headerHtml);
        }

        public static HtmlString RenderVersion(string versjonsnummer, DateTime? versjonsdato)
        {
            var version = new List<string>();

            if (!string.IsNullOrWhiteSpace(versjonsnummer))
                version.Add(versjonsnummer);

            if (versjonsdato.HasValue)
                version.Add(versjonsdato.Value.ToString("dd.MM.yyyy"));

            if (!version.Any())
                return null;

            return new HtmlString($@"<p class=""version"">Versjon: {string.Join(" / ", version)}</p>");
        }

        private static string GetHeaderHtml(string numbering, string headerText, IEnumerable<string> fieldNames)
        {
            var pointCount = numbering.Split(".").Length - 1;
            var header = headerText;

            if (!string.IsNullOrWhiteSpace(numbering))
                header = @$"<span class=""numbering"">{numbering}</span>{headerText}";

            if (fieldNames?.Any() ?? false)
                header = @$"{header} <span class=""field-names"">({string.Join(", ", fieldNames)})</span>";

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
