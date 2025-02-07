using HuntFlowWIUT.Web.Models;
using System.Net.Http.Headers;

namespace HuntFlowWIUT.Web.Services
{
    public class HuntflowApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string _accessToken;

        public HuntflowApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _accessToken = _config["Huntflow:AccessToken"];
        }

        private async Task EnsureValidToken()
        {
            // Implement token refresh logic here
        }

        public async Task<IEnumerable<Vacancy>> GetVacanciesAsync()
        {
            await EnsureValidToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://huntflow.api/vacancies");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Vacancy>>();
        }
    }

}
