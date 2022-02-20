namespace CarSalesSystem.Data.Models
{
    public class UserFavAdvertisement
    {
        public string AdvertisementId { get; init; }

        public Advertisement Advertisement { get; init; }

        public string UserId { get; init; }

        public User User { get; init; }
    }
}
