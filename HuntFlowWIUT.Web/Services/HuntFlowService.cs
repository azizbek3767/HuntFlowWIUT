using HuntFlowWIUT.Web.Extensions;
using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Models.ViewModels;
using HuntFlowWIUT.Web.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Text.Json;
using static HuntFlowWIUT.Web.Services.Interfaces.IHuntFlowService;

namespace HuntFlowWIUT.Web.Services
{
    public class HuntflowService : IHuntflowService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private readonly JsonSerializerOptions _jsonOptions;

        public HuntflowService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _jsonOptions.Converters.Add(new CustomDateTimeConverter("yyyy-MM-dd"));
        }

        public async Task<VacancyListResponse> GetVacanciesAsync(int accountId, int page = 1, int count = 30)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var endpoint = $"accounts/{accountId}/vacancies";
            using var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error retrieving vacancies. Status Code: {response.StatusCode}. Response: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<VacancyListResponse>(content, _jsonOptions);
        }

        public async Task<VacancyDetail> GetVacancyDetailAsync(int accountId, int vacancyId)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var endpoint = $"accounts/{accountId}/vacancies/{vacancyId}";
            using var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error retrieving vacancy details. Status Code: {response.StatusCode}. Response: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<VacancyDetail>(content, _jsonOptions);
        }

        public async Task<ApplicantDetail> CreateApplicantAsync(int accountId, ApplicantCreationViewModel model)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var endpoint = $"accounts/{accountId}/applicants";
            using var response = await _httpClient.PostAsJsonAsync(endpoint, model, _jsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error creating applicant. Status Code: {response.StatusCode}. Response: {errorContent}");
            }
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApplicantDetail>(content, _jsonOptions);
        }
    }
}
