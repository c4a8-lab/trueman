using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Trueman.Core.Extensions;

namespace Trueman.Core.Clients.TemporaryAccessPass
{
    public class TemporaryAccessPassClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TemporaryAccessPassClient> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;

        public TemporaryAccessPassClient(HttpClient httpClient, ILogger<TemporaryAccessPassClient> logger, ITokenAcquisition tokenAcquisition)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://graph.microsoft.com/beta/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task DeleteTapAsync(string userPrincipalName, string id)
        {

            _logger.LogInformation($"Deleting tap for {userPrincipalName}");
            var accessToken = await _tokenAcquisition.GetAccessTokenForAppAsync("https://graph.microsoft.com/.default");

            var request = new HttpRequestMessage(HttpMethod.Delete, $"users/{userPrincipalName}/authentication/temporaryAccessPassMethods/{id}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            _logger.LogInformation("Sending request");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            _logger.LogInformation("Response received");

            await _logger.LogHttpResponseAsync(response);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error occurred while deleting TAP");
                _logger.LogError($"Status code is {response.StatusCode}");
                _logger.LogError($"Reason phrase is {response.ReasonPhrase}");
            }
            response.EnsureSuccessStatusCode();
        }

        public async Task<TapData> GetTapAsync(string userPrincipalName, string id)
        {

            _logger.LogInformation($"Getting tap for {userPrincipalName} with id {id}");
            var accessToken = await _tokenAcquisition.GetAccessTokenForAppAsync("https://graph.microsoft.com/.default");

            var request = new HttpRequestMessage(HttpMethod.Get, $"users/{userPrincipalName}/authentication/temporaryAccessPassMethods/{id}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            _logger.LogInformation("Sending request");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            _logger.LogInformation("Response received");

            await _logger.LogHttpResponseAsync(response);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error occurred while fetching TAP");
                _logger.LogError($"Status code is {response.StatusCode}");
                _logger.LogError($"Reason phrase is {response.ReasonPhrase}");
            }
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TapData>(content);
        }
        public async Task<TapData> GetTapFromListAsync(string userPrincipalName)
        {

            _logger.LogInformation($"Getting all taps for {userPrincipalName}. It is always one.");
            var accessToken = await _tokenAcquisition.GetAccessTokenForAppAsync("https://graph.microsoft.com/.default");

            var request = new HttpRequestMessage(HttpMethod.Get, $"users/{userPrincipalName}/authentication/temporaryAccessPassMethods");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            _logger.LogInformation("Sending request");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            _logger.LogInformation("Response received");

            await _logger.LogHttpResponseAsync(response);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error occurred while fetching TAP");
                _logger.LogError($"Status code is {response.StatusCode}");
                _logger.LogError($"Reason phrase is {response.ReasonPhrase}");
            }
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var taps = JsonConvert.DeserializeObject<TapListRoot>(content);

            return taps.Items.FirstOrDefault();
        }

        public async Task<TapData> CreateTapAsync(TapToCreateDto model)
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(new { model.IsUsableOnce }), Encoding.UTF8, "application/json");

            _logger.LogInformation($"Creating tap. Data is {JsonConvert.SerializeObject(model)}");
            var accessToken = await _tokenAcquisition.GetAccessTokenForAppAsync("https://graph.microsoft.com/.default");

            var request = new HttpRequestMessage(HttpMethod.Post, $"users/{model.UserPrincipalName}/authentication/temporaryAccessPassMethods")
            {
                Content = requestBody
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            _logger.LogInformation("Sending request");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            _logger.LogInformation("Response received");

            await _logger.LogHttpResponseAsync(response);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error occurred while creating TAP");
                _logger.LogError($"Status code is {response.StatusCode}");
                _logger.LogError($"Reason phrase is {response.ReasonPhrase}");
            }
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var info = JsonConvert.DeserializeObject<TapData>(content);

            return info;
        }
    }
}
