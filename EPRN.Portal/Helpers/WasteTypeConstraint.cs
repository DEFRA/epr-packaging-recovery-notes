using EPRN.Portal.Constants;
using System.Text.RegularExpressions;

namespace EPRN.Portal.Helpers
{
    /// <summary>
    /// This class ensures that when using waste type in a url
    /// that it matches any of the possible values for a WasteType
    /// (i.e. Exporter or Reprocessor
    /// </summary>
    public class WasteTypeConstraint : IRouteConstraint
    {
        public readonly string Pattern;

        public WasteTypeConstraint()
        {
            // Generate a pattern based on your enumeration values
            Pattern = string.Join("|", Enum.GetNames(typeof(WasteType)));
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object typeValue;
            if (values.TryGetValue(routeKey, out typeValue) && typeValue != null)
            {
                return Regex.IsMatch(typeValue.ToString(), Pattern, RegexOptions.IgnoreCase);
            }
            return false;
        }
    }
}
