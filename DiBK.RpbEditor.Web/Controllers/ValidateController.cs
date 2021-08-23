using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DiBK.RpbEditor.Application.Models.DTO;
using DiBK.RpbEditor.Application.Services;
using System;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateController : BaseController
    {
        private readonly IValidationService _validationService;

        public ValidateController(
            IValidationService validationService,
            ILogger<ValidateController> logger) : base(logger)
        {
            _validationService = validationService;
        }

        [HttpPost]
        public async Task<IActionResult> Validate(Reguleringsplanbestemmelser reguleringsplanbestemmelser)
        {
            try
            {
                if (reguleringsplanbestemmelser == null)
                    return BadRequest();

                var result = await _validationService.ValidateAsync(reguleringsplanbestemmelser);

                return Ok(result);
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }
    }
}
