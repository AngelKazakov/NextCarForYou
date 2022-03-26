using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.User
{
    public class UserService : IUserService
    {
        private readonly CarSalesDbContext context;

        public UserService(CarSalesDbContext context)
        => this.context = context;


        public async Task<bool> AddAdvertisementToFavoriteAsync(string advertisementId, string userId)
        {
            if (context.UserFavAdvertisements.Any(x => x.AdvertisementId == advertisementId && x.UserId == userId))
            {
                await RemoveAdvertisementFromFavoriteAsync(advertisementId, userId);
                return false;
            }

            context.UserFavAdvertisements.Add(new UserFavAdvertisement()
            {
                UserId = userId,
                AdvertisementId = advertisementId
            });

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAdvertisementFromFavoriteAsync(string advertisementId, string userId)
        {
            var favAdvertisement =
               await context.UserFavAdvertisements.FirstOrDefaultAsync(x => x.UserId == userId && x.AdvertisementId == advertisementId);

            if (favAdvertisement != null)
            {
                context.UserFavAdvertisements.Remove(favAdvertisement);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
