using Microsoft.AspNetCore.Mvc;

namespace HuntFlowWIUT.Web.Controllers
{
    [Route("accounts/{accountId}/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FilesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(int accountId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", accountId.ToString());
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // For demonstration, generate a dummy file id.
            int fileId = new Random().Next(1000, 9999);

            // Return the file information as JSON.
            return Ok(new
            {
                id = fileId,
                fileName = fileName,
                url = $"/uploads/{accountId}/{fileName}"
            });
        }
    }
}
