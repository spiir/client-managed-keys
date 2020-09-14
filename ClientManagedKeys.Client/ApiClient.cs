using System;
using System.Net;
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

            if (!httpResponse.IsSuccessStatusCode)
                throw new ApiClientException(httpResponse.StatusCode);

            var responseBody = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
    
    public class ApiClientException : Exception
    { 
        public ApiClientException(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
              case HttpStatusCode.BadRequest:
                    Fault = ApiClientFault.BadRequest;
                    break;
                case HttpStatusCode.Unauthorized:
                    Fault = ApiClientFault.Unauthorized;
                    break;
                case HttpStatusCode.Forbidden:
                    Fault = ApiClientFault.Forbidden;
                    break;
                case HttpStatusCode.NotFound:
                    Fault = ApiClientFault.NotFound;
                    break;
              case HttpStatusCode.TooManyRequests:
                    Fault = ApiClientFault.TooManyRequests;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, null);
            }
        }

        public ApiClientFault Fault { get; }
    }

    public enum ApiClientFault
    {
        BadRequest,
        Unauthorized,
        Forbidden,
        NotFound,
        TooManyRequests
    }

    public interface IApiClient
    {
        Task<byte[]> Sign(string keyId, SignOperationRequest request);
        Task<byte[]> Decrypt(string keyId, DecryptOperationRequest request);
    }
}