using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PhotoSauce.MagicScaler;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly string _imagePathCategoryPhoto;
        private readonly string _imagePathSubCategoryPhoto;
        private readonly string _imagePathProductPhoto;
        private readonly string _imagePathUserProfilePhoto;

        public FileManager(IConfiguration config)
        {
            _imagePathCategoryPhoto = config["Path:Images:CategoryPhoto"];
            _imagePathSubCategoryPhoto = config["Path:Images:SubCategoryPhoto"];
            _imagePathProductPhoto = config["Path:Images:ProductPhoto"];
            _imagePathUserProfilePhoto = config["Path:Images:UserProfilePhoto"];
        }

        public FileStream ImageStream(string image)
        {
            if (image.Contains("categoryphoto"))
                return new FileStream(Path.Combine(_imagePathCategoryPhoto, image), FileMode.Open, FileAccess.Read);
            else if (image.Contains("subcategoryphoto"))
                return new FileStream(Path.Combine(_imagePathSubCategoryPhoto, image), FileMode.Open, FileAccess.Read);
            else if (image.Contains("productphoto"))
                return new FileStream(Path.Combine(_imagePathProductPhoto, image), FileMode.Open, FileAccess.Read);
            else
                return new FileStream(Path.Combine(_imagePathUserProfilePhoto, image), FileMode.Open, FileAccess.Read);
        }

        public bool RemoveImage(string image, string type)
        {
            try
            {
                string file = "";
                if (type == "CategoryPhoto")
                    file = Path.Combine(_imagePathCategoryPhoto, image);
                else if (type == "ProductPhoto")
                    file = Path.Combine(_imagePathProductPhoto, image);
                else if (type == "SubCategoryPhoto")
                    file = Path.Combine(_imagePathSubCategoryPhoto, image);
                else
                    file = Path.Combine(_imagePathUserProfilePhoto, image);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public string SaveImage(IFormFile image, string type)
        {
            try
            {
                string save_path = "";
                if (type == "CategoryPhoto")
                    save_path = Path.Combine(_imagePathCategoryPhoto);
                else if (type == "ProductPhoto")
                    save_path = Path.Combine(_imagePathProductPhoto);
                else if (type == "SubCategoryPhoto")
                    save_path = Path.Combine(_imagePathSubCategoryPhoto);
                else
                    save_path = Path.Combine(_imagePathUserProfilePhoto);

                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }

                var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var pathtype = save_path.Split("/", 3)
                    .FirstOrDefault(pathtype => pathtype == "categoryphoto" || pathtype == "subcategoryphoto" || pathtype == "productphoto" || pathtype == "userprofilephoto");
                var fileName = $"{pathtype}_img_{image.FileName.Substring(0, image.FileName.LastIndexOf(".")).Replace(' ', '_')}{mime}";

                //using(var outputFileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                //    await image.CopyToAsync(outputFileStream);
                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, ImageOptions());
                }

                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }

        private ProcessImageSettings ImageOptions() => new ProcessImageSettings
        {

            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 80,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420,
            ResizeMode = CropScaleMode.Contain,
            Height = 360,
            Width = 360
        };
    }
}
