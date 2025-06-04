using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;


namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpPost("write-log")]
        public IActionResult WriteLog([FromBody] string message)
        {
            string logDirectory = @"C:\LogPath";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            string logFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt");

            try
            {
                // Write the log message with a timestamp
                using (StreamWriter sw = new StreamWriter(logFilePath, append: true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }

                return Ok("Log written successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to write log: {ex.Message}");
            }
        }
    }
}
