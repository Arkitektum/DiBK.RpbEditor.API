using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using DiBK.RpbEditor.Application.Models.CodeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static DiBK.RpbEditor.Application.Services.CodeListSettings;

namespace DiBK.RpbEditor.Application.Services
{
    public class CodeListService : ICodeListService
    {
        private static CodeList _hensynskategorier;
        private static CodeList _hovedformål;        
        private static CodeList _plantyper;
        private static CodeList _rekkefølgeangivelser;
        private static CodeList _tekstformat;

        private readonly CodeListSettings _settings;
        private readonly ILogger<CodeListService> _logger;
        public HttpClient Client { get; }

        public CodeListService(
            HttpClient client,
            IOptions<CodeListSettings> options,
            ILogger<CodeListService> logger)
        {
            Client = client;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<CodeLists> GetCodeLists()
        {
            return new CodeLists
            {
                Hensynskategorier = await GetHensynskategorier(),
                Hovedformål = await GetHovedformål(),
                Lovreferanser = GetLovreferanser(),
                Plantyper = (await GetPlantyper())
                    .Where(plantype => plantype.Value == "34" || plantype.Value == "35")
                    .ToList(),
                Rekkefølgeangivelser = await GetRekkefølgeangivelser(),
                Tekstformat = (await GetTekstformat())
                    .Where(tekstformat => tekstformat.Value == "html")
                    .ToList()
            };
        }

        private async Task<List<CodeListItem>> GetHensynskategorier()
        {
            if (IsValid(_hensynskategorier, _settings.Hensynskategori))
                return _hensynskategorier.CodeListItems;
            
            _hensynskategorier = await FetchData(_settings.Hensynskategori);

            return _hensynskategorier?.CodeListItems;
        }

        private async Task<List<CodeListItem>> GetHovedformål()
        {
            if (IsValid(_hovedformål, _settings.Hovedformål))
                return _hovedformål.CodeListItems;

            _hovedformål = await FetchData(_settings.Hovedformål);

            return _hovedformål?.CodeListItems;
        }

        private async Task<List<CodeListItem>> GetPlantyper()
        {
            if (IsValid(_plantyper, _settings.Plantype))
                return _plantyper.CodeListItems;

            _plantyper = await FetchData(_settings.Plantype);

            return _plantyper?.CodeListItems;
        }

        private async Task<List<CodeListItem>> GetRekkefølgeangivelser()
        {
            if (IsValid(_rekkefølgeangivelser, _settings.Rekkefølgeangivelse))
                return _rekkefølgeangivelser.CodeListItems;

            _rekkefølgeangivelser = await FetchData(_settings.Rekkefølgeangivelse);

            return _rekkefølgeangivelser?.CodeListItems;
        }

        private async Task<List<CodeListItem>> GetTekstformat()
        {
            if (IsValid(_tekstformat, _settings.Tekstformat))
                return _tekstformat.CodeListItems;

            _tekstformat = await FetchData(_settings.Tekstformat);

            return _tekstformat?.CodeListItems;
        }

        private static List<CodeListItem> GetLovreferanser()
        {
            return new List<CodeListItem>
            {
                new CodeListItem("6", "PBL 2008")
            };
        }

        private async Task<CodeList> FetchData(DataSource dataSource)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, dataSource.Url);
                request.Headers.Add("Accept", "application/json");

                using var response = await Client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var stringContent = await response.Content.ReadAsStringAsync();
                
                var jObject = JObject.Parse(stringContent);
                var containedItems = jObject["containeditems"] as JArray;

                var codeListItems = containedItems
                    .Select(item => new CodeListItem(item.Value<string>("codevalue"), item.Value<string>("label")))
                    .OrderBy(item => item.Label)
                    .ToList();

                return new CodeList
                {
                    CodeListItems = codeListItems,
                    LastUpdated = DateTime.Now
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Kunne ikke laste ned data fra {dataSource.Url}");
                return null;
            }
        }

        private static bool IsValid(CodeList codeList, DataSource dataSource)
        {
            if (codeList == null)
                return false;

            var sinceLastUpdate = DateTime.Now.Subtract(codeList.LastUpdated).TotalDays;

            return sinceLastUpdate < dataSource.CacheDays;
        }
    }
}
