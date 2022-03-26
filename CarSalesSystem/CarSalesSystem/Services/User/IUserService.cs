using System.Threading.Tasks;

namespace CarSalesSystem.Services.User
{
    public interface IUserService
    {
        public Task<bool> AddAdvertisementToFavoriteAsync(string advertisementId, string userId);

        public Task<bool> RemoveAdvertisementFromFavoriteAsync(string advertisementId, string userId);
    }
}
