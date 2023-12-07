using EPRN.Portal.Helpers.Interfaces;

namespace EPRN.Portal.Helpers
{
    public class QueryStringHelper : IQueryStringHelper
    {
        private const string cultureQueryString = "&culture=";
        private readonly IHttpContextAccessor httpContextAccessor;

        public QueryStringHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string RemoveCultureQueryString()
        {
            if (this.httpContextAccessor == null || this.httpContextAccessor.HttpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null. The operation requires a valid HttpContext.");
            }

            string existingQueryStrings = this.httpContextAccessor.HttpContext.Request.QueryString.ToString().Replace('?', '&');

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
