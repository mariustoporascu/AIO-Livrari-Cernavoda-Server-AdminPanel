using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace OShop.Application.FileManager
{
    public interface IFileManager
    {
        FileStream ImageStream(string image);
        bool RemoveImage(string image, string type);
        Task<string> SaveImage(IFormFile Image, string type);
    }
}
