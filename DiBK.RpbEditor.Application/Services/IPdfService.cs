using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdf(string html);
    }
}
