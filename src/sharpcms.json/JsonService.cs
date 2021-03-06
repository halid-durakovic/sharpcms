using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace sharpcms.json
{
    public class JsonService : IJsonService
    {
        private static readonly ConcurrentDictionary<string, string> Headers;

        private readonly HttpClient _client;

        private readonly JsonSerializerSettings _settings;

        private bool _successOnly;

        static JsonService()
        {
            Headers = new ConcurrentDictionary<string, string>();
        }

        public JsonService()
        {
            _client = new HttpClient();

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        }

        public JsonService(HttpClient client)
        {
            _client = client;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        }

        public async Task<T> Get<T>(string uri, object data = null)
        {
            var result = await Get(uri, data);

            if (_successOnly)
                result.EnsureSuccessStatusCode();

            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> Get(string uri, object data = null)
        {
            var fullUri = uri + data.ToQueryString();

            var requestMessage = CreateRequest(HttpMethod.Get, fullUri);

            return await _client.SendAsync(requestMessage);
        }

        public async Task<string> GetString(string uri, object data = null)
        {
            var fullUri = uri + data.ToQueryString();

            var requestMessage = CreateRequest(HttpMethod.Get, fullUri);

            var result = await _client.SendAsync(requestMessage);

            if (_successOnly)
                result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsStringAsync();
        }

        public async Task<T> Post<T>(string uri, object data = null, bool dontSerialize = false)
        {
            var result = await Post(uri, data, dontSerialize);

            if (_successOnly)
                result.EnsureSuccessStatusCode();

            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> Post(string uri, object data = null, bool dontSerialize = false)
        {
            var request = CreateRequest(HttpMethod.Post, uri);

            if (dontSerialize)
            {
                request.Content = new StringContent(data.SafeToString(), Encoding.UTF8, "application/json");
            }
            else
            {
                if (data != null)
                    request.Content = SerializeRequest(data);
            }

            return await _client.SendAsync(request);
        }

        public async Task<T> Put<T>(string uri, object data = null, bool dontSerialize = false)
        {
            var result = await Put(uri, data, dontSerialize);

            if (_successOnly)
                result.EnsureSuccessStatusCode();

            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> Put(string uri, object data = null, bool dontSerialize = false)
        {
            var request = CreateRequest(HttpMethod.Put, uri);

            if (dontSerialize)
            {
                request.Content = new StringContent(data.SafeToString(), Encoding.UTF8, "application/json");
            }
            else
            {
                if (data != null)
                    request.Content = SerializeRequest(data);
            }

            return await _client.SendAsync(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var result = await Delete(uri);

            if (_successOnly)
                result.EnsureSuccessStatusCode();

            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> Delete(string uri)
        {
            var request = CreateRequest(HttpMethod.Delete, uri);

            request.Content = SerializeRequest();

            return await _client.SendAsync(request);
        }

        public void SetHeader(string header, string value)
        {
            Headers[header] = value;
        }

        public void ClearHeader(string header)
        {
            Headers[header] = null;
        }

        public void ClearHeaders()
        {
            Headers.Clear();
        }

        public void EnableOnlySuccessOnlyMode(bool successOnly = true)
        {
            this._successOnly = successOnly;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private HttpContent SerializeRequest(object data = null)
        {
            StringContent request;

            if (data == null)

                request = new StringContent(string.Empty);

            else

                request = new StringContent(JsonConvert.SerializeObject(data, _settings));

            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return request;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string uri)
        {
            var requestMessage = new HttpRequestMessage(method, uri);

            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var headerValue in Headers.Where(xy => !string.IsNullOrEmpty(xy.Value)))
                requestMessage.Headers.Add(headerValue.Key, headerValue.Value);

            return requestMessage;
        }

        private async Task<T> DeserialiseResponse<T>(HttpResponseMessage result)
        {
            var payload = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(payload, _settings);
        }
    }
}