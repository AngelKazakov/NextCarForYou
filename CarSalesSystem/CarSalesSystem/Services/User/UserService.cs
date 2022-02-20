using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.User
{
    public class UserService : IUserService
    {
        private readonly CarSalesDbContext context;

        public UserService(CarSalesDbContext context)
        => this.context = context;


        public bool AddAdvertisementToFavorite(string advertisementId, string userId)
        {
            if (context.UserFavAdvertisements.Any(x => x.AdvertisementId == advertisementId && x.UserId == userId))
            {
                return false;
            }

            context.UserFavAdvertisements.Add(new UserFavAdvertisement()
            {
                UserId = userId,
                AdvertisementId = advertisementId
            });

            context.SaveChanges();

            return true;
        }

        public bool RemoveAdvertisementFromFavorite(string advertisementId, string userId)
        {
            var favAdvertisement =
                context.UserFavAdvertisements.FirstOrDefault(x => x.UserId == userId && x.AdvertisementId == advertisementId);

            if (favAdvertisement != null)
            {
                context.UserFavAdvertisements.Remove(favAdvertisement);
                context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
