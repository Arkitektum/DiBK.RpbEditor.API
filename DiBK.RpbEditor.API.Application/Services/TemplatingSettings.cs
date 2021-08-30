using RazorLight;
using System.Reflection;

namespace DiBK.RpbEditor.Application.Services
{
    public interface ITemplatingSettings
    {
        RazorLightEngine Engine { get; set; }
        Assembly TemplateAssembly { get; set; }
        string RootNamespace { get; set; }
    }

    public class TemplatingSettings : ITemplatingSettings
    {
        public RazorLightEngine Engine { get; set; }
        public Assembly TemplateAssembly { get; set; }
        public string RootNamespace { get; set; }
    }
}
