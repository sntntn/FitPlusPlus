using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace videoTrainingService.API.Controllers

{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly string _uploadsPath;

        public UploadController(IWebHostEnvironment env)
        {
            _uploadsPath = Path.Combine(env.ContentRootPath, "uploads");
            if (!Directory.Exists(_uploadsPath))
            {
                Directory.CreateDirectory(_uploadsPath);
            }
        }

        [HttpPost("video")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadVideo([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var filePath = Path.Combine(_uploadsPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FileName = file.FileName });
        }

        [HttpDelete("video/delete/{fileName}")]
        public IActionResult DeleteVideo(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return BadRequest("Invalid file name.");

                var filePath = Path.Combine(_uploadsPath, fileName);

                if (!System.IO.File.Exists(filePath))
                    return NotFound("File not found.");

                System.IO.File.Delete(filePath);

                return Ok(new { message = $"File '{fileName}' successfully deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting file: {ex.Message}");
            }
        }
    }
}