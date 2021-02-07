using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Pages
{
    [ResponseCache(CacheProfileName = "MonthlyRazor")]
    public class ImageModel : PageModel
    {
        private readonly IFileManager _fileManager;

        public ImageModel(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public FileStreamResult OnGet(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }
    }
}
