using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiBK.RpbEditor.Application.Services;

namespace DiBK.RpbEditor.Web.Configuration
{
    public static class PdfConfig
    {
        public static void AddPdfGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PdfSettings>(configuration.GetSection(PdfSettings.SectionName));

            services.AddTransient<IPdfService, PdfService>();

            services.AddHttpClient<PdfService>();
        }
    }
}
