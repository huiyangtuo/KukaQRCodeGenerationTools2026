using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace KukaQRCodeGenerationTools2026.Api
{
    public class HytHttpClient
    {
        private static readonly HttpClient _httpClient = new();


        public async static Task<T> PostAsync<T>(string url, object param)
        {

            string jsonData = JsonConvert.SerializeObject(param);

            HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Content = content;

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async static Task<T> PostWithTokenAsync<T>(string url, object param, string userToken)
        {

            string jsonData = JsonConvert.SerializeObject(param);

            HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Headers.Authorization = new("Bearer", userToken);
            request.Content = content;

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async static Task<T> GetAsync<T>(string url)
        {
            HttpRequestMessage request = new(HttpMethod.Get, url);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async static Task<T> GetWithTokenAsync<T>(string url, string userToken)
        {
            HttpRequestMessage request = new(HttpMethod.Get, url);
            request.Headers.Authorization = new("Bearer", userToken);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async static Task<T> PutWithTokenAsync<T>(string url, string userToken)
        {
            HttpRequestMessage request = new(HttpMethod.Put, url);
            request.Headers.Authorization = new("Bearer", userToken);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
