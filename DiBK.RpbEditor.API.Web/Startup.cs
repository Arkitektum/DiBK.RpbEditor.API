using DiBK.RpbEditor.Application.Configuration;
using DiBK.RpbEditor.Application.Services;
using DiBK.RpbEditor.Web.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DiBK.RpbEditor
{
    public class Startup
    {
        private static readonly Assembly _applicationAssembly = Assembly.Load("DiBK.RpbEditor.Application");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            var mvcBuilder = services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlanbestemmelserEditor", Version = "v1" });
            });

            services.AddTemplating(mvcBuilder, options => options.TemplateAssembly = _applicationAssembly);
            services.AddPdfGenerator(Configuration);

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new DtoToXmlClassProfile());
                config.AddProfile(new XmlClassToDtoProfile());
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<IConverterService, ConverterService>();
            services.AddTransient<ICodeListService, CodeListService>();
            services.AddTransient<IValidationService, ValidationService>();

            services.AddHttpClient<CodeListService>();
            services.AddHttpClient<ValidationService>();

            services.Configure<CodeListSettings>(Configuration.GetSection(CodeListSettings.SectionName));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DiBK Planbestemmelser Editor v1"));
            }

            app.UseCors(options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
