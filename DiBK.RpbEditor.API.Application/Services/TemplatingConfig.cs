using DiBK.RpbEditor.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;
using System;
using System.Net.Http.Headers;

namespace DiBK.RpbEditor.API.Application.Services
{
    public static class TemplatingConfig
    {
        public static void AddTemplating(this IServiceCollection services, Action<TemplatingSettings> settings)
        {
            services.Configure(settings);

            var templatingSettings = new TemplatingSettings();
            settings.Invoke(templatingSettings);

            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(templatingSettings.TemplateAssembly, templatingSettings.RootNamespace)
                .UseMemoryCachingProvider()
                .Build();
            
            services.AddSingleton(engine);

            services.AddHttpClient<ITemplatingService, TemplatingService>(client =>
            {
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromDays(7)
                };
            });
        }
    }
}
