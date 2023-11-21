using Newtonsoft.Json;

namespace Portal.Services
{
    public abstract class BaseService
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public BaseService(
            string baseUrl,
            IHttpClientFactory httpClientFactory)
        {
            _baseUrl = string.IsNullOrWhiteSpace(baseUrl) ? throw new ArgumentNullException(nameof(baseUrl)) : baseUrl;
            _httpClient = httpClientFactory.CreateClient();

            if (_baseUrl.EndsWith("/"))
                _baseUrl = _baseUrl.TrimEnd('/');
        }

        protected async Task<T> Get<T>(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{url}/";

            
            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            return await Send<T>(msg)!;
        }

        protected async Task<T> Post<T>(string url, object payload)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{payload}";

            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(payload))
            };

            return await Send<T>(msg)!;
        }

        protected async Task Post(string url, object payload)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            url = $"{_baseUrl}/{payload}";

            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(payload))
            };

            await Send(msg)!;
        }

        protected async Task<T> Send<T>(HttpRequestMessage requestMessage)
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

        protected async Task Send(HttpRequestMessage requestMessage)
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
