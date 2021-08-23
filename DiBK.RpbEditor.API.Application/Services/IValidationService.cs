using DiBK.RpbEditor.Application.Models.DTO;
using DiBK.RpbEditor.Application.Models.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public interface IValidationService
    {
        Task<List<ValidationRule>> ValidateAsync(Reguleringsplanbestemmelser reguleringsplanbestemmelser);
    }
}
