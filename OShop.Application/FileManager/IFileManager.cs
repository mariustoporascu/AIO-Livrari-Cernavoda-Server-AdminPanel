using Microsoft.AspNetCore.Http;
using System.IO;

namespace OShop.Application.FileManager
{
    public interface IFileManager
    {
        FileStream ImageStream(string image);
        bool RemoveImage(string image, string type);
        string SaveImage(IFormFile Image, string type);
    }
}
