using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace HuntFlowWIUT.Web.Services
{
    public class HuntFlowService : IHuntFlowService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private readonly ILogger<HuntFlowService> _logger;
        private readonly string _baseUrl;

        public HuntFlowService(HttpClient httpClient, ITokenService tokenService, IConfiguration configuration, ILogger<HuntFlowService> logger)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _logger = logger;
            _baseUrl = configuration["HuntFlow:BaseUrl"];
        }

        public async Task<IEnumerable<Candidate>> GetCandidateAsync()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var endpoint = $"{_baseUrl}/candidates";

            try
            {
                var candidates = await _httpClient.GetFromJsonAsync<IEnumerable<Candidate>>(endpoint);
                return candidates;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching candidates from huntflow");
                throw;
            }
        }

        public async Task<VacanciesResponse> GetVacanciesAsync(int accountId, int page = 1, int count = 30, bool mine = false, bool opened = false, string[] state = null)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string endpoint = $"{_baseUrl}/accounts/{accountId}/vacancies";

            var queryParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("count", count.ToString()),
                new KeyValuePair<string, string>("page", page.ToString()),
                new KeyValuePair<string, string>("mine", mine.ToString().ToLower()),
                new KeyValuePair<string, string>("opened", opened.ToString().ToLower())
            };

            if(state != null)
            {
                foreach(var s in state)
                {
                    queryParams.Add(new KeyValuePair<string, string>("state", s));
                }
            }

            string url = QueryHelpers.AddQueryString(endpoint, queryParams);

            try
            {
                var vacanciesResponse = await _httpClient.GetFromJsonAsync<VacanciesResponse>(url);
                return vacanciesResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching vacancies from huntflow");
                throw;
            }
        }
    }
}
