using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace HuntFlowWIUT.Web.Controllers
{
    [Route("api/proxyf")]
    [ApiController]
    [AllowAnonymous] // Allow anonymous access for this endpoint.
    public class FileUploadController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FileUploadController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromQuery] int accountId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            var client = _httpClientFactory.CreateClient();
            string uploadUrl = $"https://api.huntflow.uz/v2/accounts/{accountId}/upload";

            using var formData = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "file", file.FileName);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
            {
                Content = formData
            };

            requestMessage.Headers.Add("X-File-Parse", "true");
            // If needed, you can also forward an Authorization header from your configuration here.

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
