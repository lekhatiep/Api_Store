using DoAn3API.Dto.Firebase;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DoAn3API.Services.Firebase
{
    public interface IFirebaseService
    {
        Task<string> UploadFileAsync(FileUploadDto fileUpload);
        Task<string> UploadFileAsync(IFormFile file);
        Task<int> DeleteFileAsync(string fileName);
    }
}
