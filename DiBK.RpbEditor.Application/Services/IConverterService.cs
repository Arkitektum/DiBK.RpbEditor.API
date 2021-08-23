using DiBK.RpbEditor.Application.Models.DTO;
using System.IO;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public interface IConverterService
    {
        Task<string> ToXml(Reguleringsplanbestemmelser reguleringsplanbestemmelser);
        Task<string> ToHtml(Reguleringsplanbestemmelser reguleringsplanbestemmelser);
        Task<byte[]> ToPdf(Reguleringsplanbestemmelser reguleringsplanbestemmelser);
        Reguleringsplanbestemmelser FromXml(Stream xmlStream);
    }
}
