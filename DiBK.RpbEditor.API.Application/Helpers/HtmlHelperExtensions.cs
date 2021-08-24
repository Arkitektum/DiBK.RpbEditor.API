using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiBK.RpbEditor.Application.Services;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static async Task<HtmlString> RenderViewAsync<TModel>(this IHtmlHelper<TModel> html, string viewName) where TModel : class
        {
            var templateService = (ITemplatingService) html.ViewContext.HttpContext.RequestServices.GetService(typeof(ITemplatingService));
            var output = await templateService.RenderViewAsync(viewName, html.ViewData.Model);

            return new HtmlString(output);
        }

        public static HtmlString RenderHeader(this IHtmlHelper _, string numbering, string header)
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
