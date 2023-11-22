using Portal.Helpers.Interfaces;

namespace Portal.Helpers
{
    public class QueryStringHelper : IQueryStringHelper
    {
        private const string cultureQueryString = "&culture=";

        public string RemoveCultureQueryString(string existingQueryStrings)
        {
            existingQueryStrings = existingQueryStrings.Replace('?', '&');

            var startIndex = existingQueryStrings.IndexOf(cultureQueryString);

            if (startIndex >= 0)
            {
                // Length of "&culture=" + 5 characters of the culture code (en-GB or cy-GB)
                existingQueryStrings = existingQueryStrings.Remove(startIndex, cultureQueryString.Length + 5); 
            }

            return existingQueryStrings;
        }
    }
}
