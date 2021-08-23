using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using DiBK.RpbEditor.Application.Exceptions;
using DiBK.RpbEditor.Application.Models.DTO;
using DiBK.RpbEditor.Application.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IConverterService _converterService;
        private readonly string _apiUrl;
        private readonly ILogger<ValidationService> _logger;
        public HttpClient Client { get; }

        public ValidationService(
            HttpClient client,
            IConverterService converterService,
            IConfiguration config,
            ILogger<ValidationService> logger)
        {
            Client = client;
            _converterService = converterService;
            _apiUrl = config["ValidationUrl"];
            _logger = logger;
        }

        public async Task<List<ValidationRule>> ValidateAsync(Reguleringsplanbestemmelser reguleringsplanbestemmelser)
        {
            var xmlString = await _converterService.ToXml(reguleringsplanbestemmelser);
            var multipartContent = new MultipartFormDataContent();
            var stringContent = new StringContent(xmlString);

            multipartContent.Add(stringContent, "planbestemmelser", "planbestemmelser.xml");

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl)
                {
                    Content = multipartContent
                };

                request.Headers.Add("system", "Reguleringsplanbestemmelser Editor");

                using var response = await Client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(content);
                var validationRules = jObject["validationRules"].ToObject<List<ValidationRule>>();

                return validationRules
                    .Where(rule => rule.Status == "FAILED" || rule.Status == "WARNING")
                    .OrderBy(rule => rule.MessageType)
                    .ThenBy(rule => rule.Name)
                    .ToList();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Kunne ikke validere!");
                throw new CouldNotValidateException("Kunne ikke validere planbestemmelsene.");
            }
        }
    }
}
