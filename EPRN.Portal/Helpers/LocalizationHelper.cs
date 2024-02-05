using EPRN.Portal.Helpers.Interfaces;
using Microsoft.Extensions.Localization;
using System.Reflection;
using System.Xml.Linq;

namespace EPRN.Portal.Helpers
{
    public class LocalizationHelper<T> : ILocalizationHelper<T>
    {
        private readonly IStringLocalizerFactory _localizerFactory;

        public LocalizationHelper(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory ?? throw new ArgumentNullException(nameof(localizerFactory));
        }
        public string GetString(string key)
        {
            var type = typeof(T);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName ?? string.Empty);

            //var name = "EPRN.Portal.Resources.PRNS.ViewPRNResources";
            //var localizer = _localizerFactory.Create(name, assemblyName.Name);
            var localizer = _localizerFactory.Create(type.FullName, assemblyName.Name);

            var val = localizer.GetString(key.Replace(" ", "-"));
            return val.Value;
        }
    }
}
