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
        private readonly ILogger<HuntflowService> _logger;

        public HuntflowService(HttpClient httpClient, ITokenService tokenService, ILogger<HuntflowService> logger)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _jsonOptions.Converters.Add(new CustomDateTimeConverter("yyyy-MM-dd"));
            _logger = logger;
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


            //Rebuild the payload with only the required properties.
            var payload = new
            {
                first_name = model.FirstName,
                last_name = model.LastName,
                middle_name = model.MiddleName,
                money = model.Money,
                phone = model.Phone,
                email = model.Email,
                skype = model.Skype,
                position = model.Position,
                company = model.Company,
                photo = model.Photo,
                birthday = model.Birthday.ToString("yyyy-MM-dd"),
                externals = model.Externals.Select(e => new
                {
                    auth_type = e.AuthType,
                    account_source = e.AccountSource,
                    data = new { body = e.Data.Body },
                    files = e.Files
                }),
                social = model.Social.Select(s => new
                {
                    social_type = s.SocialType,
                    value = s.Value
                })
            };


            string jsonPayload = JsonSerializer.Serialize(payload, _jsonOptions);
            _logger.LogInformation("Payload JSON: {jsonPayload}", jsonPayload);
            
            
            // Create content and post it
            using var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(endpoint, content);
            _logger.LogInformation("Response: ---------------------------------------------------------------", response);
            // Log the JSON request body
            string jsonRequestBody = JsonSerializer.Serialize(model, _jsonOptions);
            _logger.LogInformation("Request JSON: {json}", jsonRequestBody);
            
            // Serialize the model to JSON so you can inspect it
            string jsonRequestBody2 = JsonSerializer.Serialize(model, _jsonOptions);
            //using var response = await _httpClient.PostAsJsonAsync(endpoint, model, _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error creating applicant. Status Code: {response.StatusCode}. Response: {errorContent}");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApplicantDetail>(responseContent, _jsonOptions);
        }

        public async Task<ApplicantDetail> CreateAcademicApplicantAsync(int accountId, ApplicantCreationAcademicViewModel model)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var endpoint = $"accounts/{accountId}/applicants";


            //Rebuild the payload with only the required properties.
            var payload = new
            {
                first_name = model.FirstName,
                last_name = model.LastName,
                middle_name = "Middle Name",
                money = 0,
                phone = model.Phone,
                email = model.Email,
                skype = "Skype",
                position = model.Position,
                company = "Company",
                photo = model.Photo,
                externals = model.Externals.Select(e => new
                {
                    auth_type = e.AuthType,
                    account_source = e.AccountSource,
                    data = new { body = e.Data.Body ?? ""},
                    files = e.Files
                }),
                social = model.Social.Select(s => new
                {
                    social_type = s.SocialType,
                    value = s.Value
                })
            };


            string jsonPayload = JsonSerializer.Serialize(payload, _jsonOptions);
            _logger.LogInformation("Payload JSON: {jsonPayload}", jsonPayload);


            // Create content and post it
            using var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(endpoint, content);
            _logger.LogInformation("Response: ---------------------------------------------------------------", response);
            // Log the JSON request body
            string jsonRequestBody = JsonSerializer.Serialize(model, _jsonOptions);
            _logger.LogInformation("Request JSON: {json}", jsonRequestBody);

            // Serialize the model to JSON so you can inspect it
            string jsonRequestBody2 = JsonSerializer.Serialize(model, _jsonOptions);
            //using var response = await _httpClient.PostAsJsonAsync(endpoint, model, _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error creating applicant. Status Code: {response.StatusCode}. Response: {errorContent}");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApplicantDetail>(responseContent, _jsonOptions);
        }
    }
}
