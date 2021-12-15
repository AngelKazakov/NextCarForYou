using System.Collections.Generic;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Models.Advertisement
{
    public class AdvertisementsQueryModel
    {
        public const int CarsPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public int TotalCars { get; set; }

        public IEnumerable<SearchResultModel> AdvertisementCardViewModels { get; set; }

    }
}
