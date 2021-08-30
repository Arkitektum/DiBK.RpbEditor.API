using DiBK.RpbEditor.Application.Models.DTO;
using System.IO;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public interface IConverterService
    {
        Reguleringsplanbestemmelser FromXml(Stream xmlStream);
        Task<string> ToXml(Reguleringsplanbestemmelser reguleringsplanbestemmelser);
        Task<string> ToHtml(Reguleringsplanbestemmelser reguleringsplanbestemmelser);
        Task<string> ToHtml(Stream xmlStream);
        Task<byte[]> ToPdf(Reguleringsplanbestemmelser reguleringsplanbestemmelser);
        Task<byte[]> ToPdf(Stream xmlStream);
    }
}
