using Newtonsoft.Json;

namespace Portal.Services
{
    /// <summary>
    /// http client base class for making http calls to web API RESTful services
    /// 
    /// In the fullness of time this will include the code to perform the auth between
    /// Portal and web APIs. But we don't have the capability of doing that yet
    /// </summary>
    public abstract class BaseHttpService
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public BaseHttpService(
            string baseUrl,
            IHttpClientFactory httpClientFactory)
        {
            // do basic checks on parameters
            _baseUrl = string.IsNullOrWhiteSpace(baseUrl) ? throw new ArgumentNullException(nameof(baseUrl)) : baseUrl;

            if (httpClientFactory == null) 
                throw new ArgumentNullException(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient();

            if (_baseUrl.EndsWith("/"))
                _baseUrl = _baseUrl.TrimEnd('/');
        }

        /// <summary>
        /// Performs an Http GET returning the specified object
        /// </summary>
        protected async Task<T> Get<T>(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            return await Send<T>(CreateMessage(url, null, HttpMethod.Get));
        }

        /// <summary>
        /// Performs an Http POST returning the speicified object
        /// </summary>
        protected async Task<T> Post<T>(string url, object payload)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{payload}";

            return await Send<T>(CreateMessage(url, payload, HttpMethod.Post));
        }

        /// <summary>
        /// Performs an Http POST without returning any data
        /// </summary>
        protected async Task Post(string url, object payload)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{payload}";

            await Send(CreateMessage(url, payload, HttpMethod.Post));
        }

        /// <summary>
        /// Performs an Http PUT returning the speicified object
        /// </summary>
        protected async Task<T> Put<T>(string url, object payload)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{payload}";

            return await Send<T>(CreateMessage(url, payload, HttpMethod.Put));
        }

        /// <summary>
        /// Performs an Http PUT without returning any data
        /// </summary>
        protected async Task Put(string url, object payload)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{payload}";

            await Send(CreateMessage(url, payload, HttpMethod.Put));
        }

        private HttpRequestMessage CreateMessage(string url, object? payload, HttpMethod httpMethod)
        {
            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = httpMethod
            };

            if (payload != null)
            {
                msg.Content = new StringContent(JsonConvert.SerializeObject(payload));
            }

            return msg;
        }

        private async Task<T> Send<T>(HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(responseStream);
                var content = await streamReader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(content))
                    return default!;

                return JsonConvert.DeserializeObject<T>(content)!;
            }
            else
            {
                // for now we don't know how we're going to handle errors specifically,
                // so we'll just throw an error with the error code
                throw new Exception($"Error occurred calling API with error code: {response.StatusCode}. Message: {response.ReasonPhrase}");
            }
        }

        private async Task Send(HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                // for now we don't know how we're going to handle errors specifically,
                // so we'll just throw an error with the error code
                throw new Exception($"Error occurred calling API with error code: {response.StatusCode}. Message: {response.ReasonPhrase}");
            }
        }
    }
}
