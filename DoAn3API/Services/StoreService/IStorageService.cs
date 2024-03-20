using System.IO;
using System.Threading.Tasks;

namespace DoAn3API.Services.StoreService
{
    public interface IStorageService
    {
        string GetUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);
    }
}
