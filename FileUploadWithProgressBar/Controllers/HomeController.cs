using Microsoft.AspNetCore.Mvc;

namespace FileUploadWithProgressBar.Controllers
{
    public class HomeController : Controller
    {
        public static int Progress { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var buffer = new byte[16 * 1024];

            await using (var output = System.IO.File.Create(GetFullPath(fileName)))
            {
                await using (var input = file.OpenReadStream())
                {
                    var totalReadBytes = 0;
                    int readBytes;

                    while ((readBytes = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalReadBytes += readBytes;
                        Progress = (int)(totalReadBytes / (float)file.Length * 100.0);
                        await Task.Delay(10);
                    }
                }
            }

            return Ok(fileName);
        }

        public string GetFullPath(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/files/");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + fileName;
        }

        [HttpPost]
        public IActionResult GetProgress()
        {
            return Ok(Progress.ToString());
        }
    }
}