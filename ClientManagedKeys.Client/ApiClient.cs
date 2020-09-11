using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientManagedKeys.Models;
using Newtonsoft.Json;

namespace ClientManagedKeys.Client
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<byte[]> Sign(string keyId, SignOperationRequest request)
        {
            var obj = await PerformRequest<SignOperationResponse>(keyId, "sign", request);
            return obj.Payload;
        }

        public async Task<byte[]> Decrypt(string keyId, DecryptOperationRequest request)
        {
            var obj = await PerformRequest<DecryptOperationResponse>(keyId, "decrypt", request);
            return obj.Payload;
        }
        private async Task<T> PerformRequest<T>(string keyId, string operation, object request)
        {
            var requestBody = JsonConvert.SerializeObject(request);

            var httpResponse = await _httpClient.PostAsync($"/v1/{keyId}/{operation}",
                new StringContent(requestBody, Encoding.UTF8, "application/json"));

            var responseBody = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }

    public interface IApiClient
    {
        Task<byte[]> Sign(string keyId, SignOperationRequest request);
        Task<byte[]> Decrypt(string keyId, DecryptOperationRequest request);
    }
}