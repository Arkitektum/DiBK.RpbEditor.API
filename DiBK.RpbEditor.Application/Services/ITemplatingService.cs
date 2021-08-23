using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public interface ITemplatingService
    {
        Task<string> RenderViewAsync<T>(string viewName, T model) where T : class;
    }
}
