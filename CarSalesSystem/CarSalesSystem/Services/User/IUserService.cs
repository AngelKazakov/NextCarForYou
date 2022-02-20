namespace CarSalesSystem.Services.User
{
    public interface IUserService
    {
        public bool AddAdvertisementToFavorite(string advertisementId,string userId);

        public bool RemoveAdvertisementFromFavorite(string advertisementId,string userId);
    }
}
