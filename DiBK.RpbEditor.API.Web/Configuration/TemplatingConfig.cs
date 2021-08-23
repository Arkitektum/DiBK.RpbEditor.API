using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using DiBK.RpbEditor.Application.Services;
using System;
using System.Net.Http.Headers;
using System.Reflection;

namespace DiBK.RpbEditor.Web.Configuration
{
    public static class TemplatingConfig
    {
        private static readonly Assembly _viewAssembly = Assembly.Load("DiBK.RpbEditor.Application");

        public static void AddTemplating(this IServiceCollection services, IMvcBuilder mvcBuilder, Action<TemplatingSettings> options)
        {
            mvcBuilder
                .AddRazorRuntimeCompilation()
                .AddApplicationPart(_viewAssembly);

            services.Configure(options);

            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(new EmbeddedFileProvider(_viewAssembly));
            });

            services.AddTransient<ITemplatingService, TemplatingService>();
            
            services.AddHttpClient<TemplatingService>(client =>
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
