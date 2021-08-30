using Microsoft.AspNetCore.Http;
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
    public class ConvertController : BaseController
    {
        private readonly IConverterService _converterService;

        public ConvertController(
            IConverterService converterService,
            ILogger<ConvertController> logger) : base(logger)
        {
            _converterService = converterService;
        }

        [HttpPost("ModelFromXml")]
        public IActionResult ModelFromXml(IFormFile file)
        {
            try
            {
                if (file == null)
                    return BadRequest();

                var model = _converterService.FromXml(file.OpenReadStream());

                return Ok(model);
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }

        [HttpPost("ModelToXml")]
        public async Task<IActionResult> ModelToXml(Reguleringsplanbestemmelser model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var xmlString = await _converterService.ToXml(model);

                return new ContentResult
                {
                    Content = xmlString,
                    ContentType = "application/xml",
                    StatusCode = 200
                };
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }

        [HttpPost("ModelToHtml")]
        public async Task<IActionResult> ModelToHtml(Reguleringsplanbestemmelser model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var html = await _converterService.ToHtml(model);

                if (html == null)
                    return BadRequest();

                return new ContentResult { ContentType = "text/html", Content = html };
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }

        [HttpPost("ModelToPdf")]
        public async Task<IActionResult> ModelToPdf(Reguleringsplanbestemmelser model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var pdfData = await _converterService.ToPdf(model);

                if (pdfData == null)
                    return BadRequest();

                return new FileContentResult(pdfData, "application/octet-stream");
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }

        [HttpPost("XmlToPdf")]
        public async Task<IActionResult> XmlToPdf(IFormFile file)
        {
            try
            {
                if (file == null)
                    return BadRequest();

                var pdfData = await _converterService.ToPdf(file.OpenReadStream());

                if (pdfData == null)
                    return BadRequest();

                return new FileContentResult(pdfData, "application/octet-stream");
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }

        [HttpPost("XmlToHtml")]
        public async Task<IActionResult> XmlToHtml(IFormFile file)
        {
            try
            {
                if (file == null)
                    return BadRequest();

                var html = await _converterService.ToHtml(file.OpenReadStream());

                if (html == null)
                    return BadRequest();

                return new ContentResult { ContentType = "text/html", Content = html };
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
