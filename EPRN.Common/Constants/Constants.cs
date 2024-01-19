using System.Globalization;

namespace EPRN.Common.Constants;

public static class CultureConstants
{
    public static readonly CultureInfo English = new("en-GB");
    public static readonly CultureInfo Welsh = new("cy-GB");
}

public static class Strings
{
    public static class ApiEndPoints
    {
        public const string Journey = "Journey";
        public const string Waste = "Waste";
        public const string PRN = "PRN";
        public const string Returns = "Returns";
    }
    public static class QueryStrings
    {
        public const string ReturnToAnswers = "rtap";
        public const string ReturnToAnswersYes = "y";

    }
}