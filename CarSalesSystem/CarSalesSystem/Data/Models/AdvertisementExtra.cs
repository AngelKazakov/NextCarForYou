namespace CarSalesSystem.Data.Models
{
    public class AdvertisementExtra
    {
        public string AdvertisementId { get; init; }

        public Advertisement Advertisement { get; set; }

        public string ExtrasId { get; init; }

        public Extras Extras { get; set; }
    }
}
