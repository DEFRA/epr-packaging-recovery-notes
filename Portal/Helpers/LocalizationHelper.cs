using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Portal.Helpers
{
    public class LocalizationHelper<T>
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
            var localizer = _localizerFactory.Create(type.Name, assemblyName.Name);

            var val = localizer.GetString(key.Replace(" ", "-").ToLowerInvariant());
            return val;
        }
    }
}
