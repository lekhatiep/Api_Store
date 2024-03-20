using DoAn3API.Contanst;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace DoAn3API.Services.StoreService
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private const string FOLDER_NAME = SystemConstant.ProductSettings.USET_CONTENT_FOLDER_NAME;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, FOLDER_NAME);

        }

        public string GetUrl(string fileName)
        {
            return $"/{FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(fileName))
            {
                await Task.Run(() =>
                {
                    File.Delete(filePath);
                });
            }
        }


    }
}
