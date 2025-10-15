using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace videoTrainingService.API.Controllers

{
    
    [Authorize]
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
        
        /// <summary>
        /// Uploads a video file to the server. The video file is stored inside the Docker container 
        /// and mapped to a local directory on the host machine.
        /// </summary>
        /// <param name="file">
        /// The uploaded video file sent as a form-data parameter.  
        /// Must not be null or empty; otherwise, the request is rejected.</param>
        [Authorize(Roles="Admin, Trainer")]
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

        /// <summary>
        /// Deletes a previously uploaded video file from the server.  
        /// </summary>
        /// <param name="fileName">
        /// The name of the video file to be deleted.  
        /// Must match an existing file in the upload directory.</param>
        [Authorize(Roles="Admin, Trainer")]
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