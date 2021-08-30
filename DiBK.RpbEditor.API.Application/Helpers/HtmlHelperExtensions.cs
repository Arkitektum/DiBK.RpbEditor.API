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


    }
}
