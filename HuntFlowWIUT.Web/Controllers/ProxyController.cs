using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace HuntFlowWIUT.Web.Controllers
{
    [AllowAnonymous]  // Allow anonymous access to this controller
    [Route("api/proxy")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;

        public ProxyController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromQuery] int accountId, IFormFile file, [FromQuery] bool isPhoto = false)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            // Create an HttpClient instance using IHttpClientFactory
            var client = _httpClientFactory.CreateClient();

            // Prepare the Huntflow upload URL (ensure it matches the API version)
            string uploadUrl = $"https://api.huntflow.uz/v2/accounts/{accountId}/upload";

            using var formData = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "file", file.FileName);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
            {
                Content = formData
            };
            if (!isPhoto)
            {
                requestMessage.Headers.Add("X-File-Parse", "true");
            } else
            {
                requestMessage.Headers.Add("X-File-Parse", "false");
                //formData.Add(new StringContent("photo"), "preset");
            }
            // Add required headers
            // Optionally, add an Authorization header if needed:
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "8bd848677a936963960c5b88b78100d76e1dfc494476f320d7ce2344b5ab71c9");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return Content(responseContent, "application/json");
        }



        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto([FromQuery] int accountId, IFormFile file, [FromQuery] bool isPhoto = false)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            // Create an HttpClient instance using IHttpClientFactory
            var client = _httpClientFactory.CreateClient();

            // Prepare the Huntflow upload URL (ensure it matches the API version)
            string uploadUrl = $"https://api.huntflow.uz/v2/accounts/{accountId}/upload";

            using var formData = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "file", file.FileName);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
            {
                Content = formData
            };

            requestMessage.Headers.Add("X-File-Parse", "false");
                //formData.Add(new StringContent("photo"), "preset");
            // Add required headers
            // Optionally, add an Authorization header if needed:
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "8bd848677a936963960c5b88b78100d76e1dfc494476f320d7ce2344b5ab71c9");

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return Content(responseContent, "application/json");
        }
    }
}
