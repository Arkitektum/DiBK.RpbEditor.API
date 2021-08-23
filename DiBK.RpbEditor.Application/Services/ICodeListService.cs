using DiBK.RpbEditor.Application.Models.CodeList;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public interface ICodeListService
    {
        Task<CodeLists> GetCodeLists();
    }
}
