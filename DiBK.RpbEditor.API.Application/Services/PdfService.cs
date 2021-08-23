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
        private readonly PdfSettings _settings;
        private readonly ILogger<PdfService> _logger;
        public HttpClient HttpClient { get; }

        public PdfService(
            HttpClient httpClient,
            IOptions<PdfSettings> options,
            ILogger<PdfService> logger)
        {
            HttpClient = httpClient;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<byte[]> GeneratePdf(string html)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, _settings.GeneratorUrl)
                {
                    Content = CreateHttpContent(html),
                };

                request.Headers.Add("X-API-KEY", _settings.GeneratorApiKey);

                using var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Kunne ikke generere PDF!");
                return null;
            }
        }

        private StringContent CreateHttpContent(string html)
        {
            var postData = new JObject
            {
                ["htmlData"] = html,
                ["paper"] = JObject.FromObject(_settings.Paper)
            };

            return new StringContent(postData.ToString(), Encoding.UTF8, "application/json");
        }
    }
}
