namespace Portal.Helpers
{
    public class QueryStringHelper : IQueryStringHelper
    {
        public string RemoveCultureQueryString(string existingQueryStrings)
        {
            existingQueryStrings = existingQueryStrings.Replace('?', '&');
            var startIndex = existingQueryStrings.IndexOf("&culture");
            if (startIndex >= 0)
            {
                //Length of "&culture=en-GB" or "&culture=cy-GB" is 14
                return existingQueryStrings.Remove(startIndex, 14);
            }
            return existingQueryStrings;
        }
    }
}
