namespace CarSalesSystem.Models.Search
{
    public class SearchResultModel
    {
        public string AdvertisementId { get; init; }

        public string Name { get; init; }

        public decimal Price { get; init; }

        public int Year { get; init; }

        public int Mileage { get; init; }

        public string Region { get; init; }

        public string City { get; init; }

        public string CreatedOn { get; init; }

        public bool IsFavorite { get; set; }

        public byte[] Image { get; set; }
    }
}
