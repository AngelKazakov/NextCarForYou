using System.Collections.Generic;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Services.Search
{
    interface ISearchService
    {
        public IEnumerable<SearchResultModel> SearchVehicles(SearchAdvertisementModel model);
    }
}
