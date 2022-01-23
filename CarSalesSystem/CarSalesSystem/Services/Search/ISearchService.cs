using System.Collections.Generic;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Services.Search
{
    public interface ISearchService
    {
        public List<SearchResultModel> SearchVehicles(SearchAdvertisementModel model);

        public List<SearchResultModel> GetLastPublishedAdvertisements();

        public ICollection<SearchResultModel> BuildSearchResultModels(ICollection<Data.Models.Advertisement> advertisements);
    }
}
