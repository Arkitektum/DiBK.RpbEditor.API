using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DiBK.RpbEditor.Application.Services;
using System;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeListController : BaseController
    {
        private readonly ICodeListService _codeListService;

        public CodeListController(
            ICodeListService codeListService,
            ILogger<ConvertController> logger) : base(logger)
        {
            _codeListService = codeListService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCodeLists()
        {
            try
            {
                var codeLists = await _codeListService.GetCodeLists();

                return Ok(codeLists);
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
