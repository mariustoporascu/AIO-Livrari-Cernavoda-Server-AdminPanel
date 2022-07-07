using Microsoft.AspNetCore.Mvc;
using LivroManage.Application.FileManager;

namespace LivroManage.UI.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class WebImageController : ControllerBase
    {
        private readonly IFileManager _fileManager;

        public WebImageController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        [HttpGet("GetImage/{image}")]
        public FileStreamResult Get(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }
    }
}
