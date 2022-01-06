using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public class PdfService : IPdfService
    {
        private readonly HttpClient _httpClient;
        private readonly PdfSettings _settings;
        private readonly ILogger<PdfService> _logger;

        public PdfService(
            HttpClient httpClient,
            IOptions<PdfSettings> options,
            ILogger<PdfService> logger)
        {
            _httpClient = httpClient;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<byte[]> GeneratePdfAsync(string htmlString)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, _settings.ApiUrl)
                {
                    Content = CreateFormData(htmlString)
                };

                request.Headers.Add("X-API-KEY", _settings.ApiKey);

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Kunne ikke generere PDF!");
                return null;
            }
        }

        private MultipartFormDataContent CreateFormData(string htmlString)
        {
            var formData = new MultipartFormDataContent
            {
                { new StringContent(htmlString), "htmlString" },
                { new StringContent(JObject.FromObject(_settings.Paper).ToString(), Encoding.UTF8, "application/json"), "options" }
            };

            formData.Headers.ContentType.MediaType = "multipart/form-data";

            return formData;
        }
    }
}
