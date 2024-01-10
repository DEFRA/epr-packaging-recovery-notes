using System.Globalization;

namespace EPRN.Portal.Constants
{
    public static class CultureConstants
    {
        public static readonly CultureInfo English = new CultureInfo("en-GB");
        public static readonly CultureInfo Welsh = new CultureInfo("cy-GB");
    }

    public static class Strings
    {
        public static class ApiEndPoints
        {
            public const string Journey = "Journey";
            public const string Waste = "Waste";
            public const string PRN = "PRN";
        }
    }
}
