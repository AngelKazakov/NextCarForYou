namespace CarSalesSystem.Data.Models
{
    public class AdvertisementCategory
    {
        public string AdvertisementId { get; init; }

        public Advertisement Advertisement { get; set; }

        public string ExtrasCategoryId { get; init; }

        public ExtrasCategory ExtrasCategory { get; set; }
    }
}
