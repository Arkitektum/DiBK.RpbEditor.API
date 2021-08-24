using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DiBK.RpbEditor.Application.Exceptions;
using System;

namespace DiBK.RpbEditor.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private readonly ILogger<ControllerBase> _logger;

        protected BaseController(
            ILogger<ControllerBase> logger)
        {
            _logger = logger;
        }

        protected IActionResult HandleException(Exception exception)
        {
            _logger.LogError(exception.ToString());

            return exception switch
            {
                CouldNotDeserializeXmlException ex => BadRequest(ex.Message),
                CouldNotValidateException ex => BadRequest(ex.Message),
                Exception => BadRequest("En systemfeil har oppstått."),
                _ => null,
            };
        }
    }
}
