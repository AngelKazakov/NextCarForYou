using System.Threading.Tasks;

namespace CarSalesSystem.Services.Shared
{
    public interface IFileService
    {
        public Task DeleteFileFromFileSystemByAdvertisementIdAsync(string advertisementId);

        public Task DeleteFileFromFileSystemAsync(string imageId);
    }
}
