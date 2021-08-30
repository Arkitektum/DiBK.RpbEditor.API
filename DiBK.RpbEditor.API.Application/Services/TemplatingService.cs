using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using MimeDetective.InMemory;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiBK.RpbEditor.Application.Services
{
    public class TemplatingService : ITemplatingService
    {
        private readonly ITemplatingSettings _settings;
        private readonly ILogger<TemplatingService> _logger;
        public HttpClient HttpClient { get; }

        public TemplatingService(
            HttpClient httpClient,
            ITemplatingSettings settings,
            ILogger<TemplatingService> logger)
        {
            HttpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task<string> RenderViewAsync<T>(string viewName, T model) where T : class
        {
            var output = await _settings.Engine.CompileRenderAsync(viewName, model);

            var document = new HtmlDocument();
            document.LoadHtml(output);

            await InlineStylesheets(document);
            await Base64EncodeImages(document);

            return document.DocumentNode.OuterHtml;
        }

        private async Task InlineStylesheets(HtmlDocument document)
        {
            var head = document.DocumentNode.SelectSingleNode("//head");

            if (head == null)
                return;

            var links = head.SelectNodes("//link[@rel='stylesheet']");

            if (links == null)
                return;

            foreach (var link in links)
            {
                var uri = link.GetAttributeValue("href", "");

                if (string.IsNullOrWhiteSpace(uri))
                    continue;

                using var stream = await GetStreamFromUri(uri);

                if (stream == null)
                    continue;

                using var reader = new StreamReader(stream);
                var styles = reader.ReadToEnd();

                var styleNode = HtmlNode.CreateNode($"<style type=\"text/css\">{styles}</style>");
                head.InsertBefore(styleNode, link);
                link.Remove();
            }
        }

        private async Task Base64EncodeImages(HtmlDocument document)
        {
            var images = document.DocumentNode.SelectNodes("//img");

            if (images == null)
                return;

            foreach (var image in images)
            {
                var uri = image.GetAttributeValue("src", "");

                if (string.IsNullOrWhiteSpace(uri))
                    continue;

                using var stream = await GetStreamFromUri(uri);

                if (stream == null)
                    continue;

                image.SetAttributeValue("src", Base64EncodeImage(stream));
            }
        }

        private static string Base64EncodeImage(MemoryStream stream)
        {
            var bytes = stream.ToArray();
            var mime = bytes.DetectMimeType();
            var base64 = Convert.ToBase64String(bytes);

            return $"data:image/{mime.Extension};base64, {base64}";
        }

        private async Task<MemoryStream> GetStreamFromUri(string uri)
        {
            if (uri.StartsWith("http"))
                return await LoadExternalResource(uri);

            using var stream = GetResourceStream(uri);

            if (stream == null)
                return null;

            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private async Task<MemoryStream> LoadExternalResource(string uri)
        {
            try
            {
                using var response = await HttpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                var memoryStream = new MemoryStream();

                await stream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Kunne ikke laste ekstern ressurs fra '{uri}'");
                return null;
            }
        }

        private Stream GetResourceStream(string fileName)
        {
            var name = _settings.TemplateAssembly.GetManifestResourceNames().SingleOrDefault(name => name.EndsWith(fileName));

            return name != null ? _settings.TemplateAssembly.GetManifestResourceStream(name) : null;
        }
    }
}