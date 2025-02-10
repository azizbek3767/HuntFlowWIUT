using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Services.Interfaces;

namespace HuntFlowWIUT.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        private string _accessToken;
        private string _refreshToken;
        private DateTime _accessTokenExpiresAt;

        public TokenService(HttpClient httpClient, IConfiguration configuration, ILogger<TokenService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;

            _accessToken = _configuration["Huntflow:AccessToken"];
            _refreshToken = _configuration["Huntflow:RefreshToken"];

            _accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(10);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (DateTime.UtcNow >= _accessTokenExpiresAt.AddSeconds(-60))
            {
                _logger.LogInformation("Access token is expiring soon; refreshing the token.");
                await RefreshTokenAsync();
            }

            return _accessToken;
        }

        public async Task RefreshTokenAsync()
        {
            var refreshEndpoint = "token/refresh";

            var requestData = new TokenRefreshRequest
            {
                RefreshToken = _refreshToken
            };

            var response = await _httpClient.PostAsJsonAsync(refreshEndpoint, requestData);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenRefreshResponse>();

                if (tokenResponse != null)
                {
                    _accessToken = tokenResponse.AccessToken;
                    _refreshToken = tokenResponse.RefreshToken;
                    _accessTokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

                    _logger.LogInformation("Token refreshed successfully. New access token expires in {ExpiresIn} seconds.", tokenResponse.ExpiresIn);
                }
                else
                {
                    _logger.LogError("Token refresh failed: the token response was null.");
                    throw new Exception("Token refresh failed: empty response.");
                }
            }
            else
            {
                _logger.LogError("Token refresh failed with status code: {StatusCode}.", response.StatusCode);
                throw new Exception($"Token refresh failed with status code: {response.StatusCode}");
            }
        }
    }
}
