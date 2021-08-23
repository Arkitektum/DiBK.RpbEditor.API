using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiBK.RpbEditor.Application.Services;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static async Task<HtmlString> RenderViewAsync<TModel>(this IHtmlHelper<TModel> html, string viewName) where TModel : class
        {
            var templateService = (ITemplatingService) html.ViewContext.HttpContext.RequestServices.GetService(typeof(ITemplatingService));
            var output = await templateService.RenderViewAsync(viewName, html.ViewData.Model);

            return new HtmlString(output);
        }

        public static HtmlString RenderHeader(this IHtmlHelper html, string numbering, string header)
        {
            var pointCount = numbering.Split(".").Length - 1;
            var headerContent = $@"<span class=""numbering"">§ {numbering}</span>{header}";
            var headerFormat = GetHeaderFormat(pointCount);
            
            return new HtmlString(string.Format(headerFormat, headerContent));
        }

        private static string GetHeaderFormat(int pointCount)
        {
            return pointCount switch
            {
                0 => "<h2>{0}</h2>",
                1 => "<h3>{0}</h3>",
                2 => "<h4>{0}</h4>",
                3 => "<h5>{0}</h5>",
                4 => "<h6>{0}</h6>",
                _ => "",
            };
        }
    }
}
