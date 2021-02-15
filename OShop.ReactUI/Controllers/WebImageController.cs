using Microsoft.AspNetCore.Mvc;
using OShop.Application.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.ReactUI.Controllers
{
    [Route("[controller]")]
    public class WebImageController : Controller
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
