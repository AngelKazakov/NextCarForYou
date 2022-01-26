using System.Collections.Generic;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Models.Home
{
    public class HomeViewModel
    {
        public SearchAdvertisementModel SearchAdvertisementModel { get; init; }

        public ICollection<SearchResultModel> LatestPublishedAdvertisements { get; init; }
    }
}
