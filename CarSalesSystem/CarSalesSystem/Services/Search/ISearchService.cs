using System.Collections.Generic;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Services.Search
{
    public interface ISearchService
    {
        public ICollection<SearchResultModel> SearchVehicles(SearchAdvertisementModel model);

        public ICollection<SearchResultModel> DetailedSearchVehicles(DetailedSearchAdvertisementModel detailedModel);

        public ICollection<SearchResultModel> GetLastPublishedAdvertisements();

        public ICollection<SearchResultModel> BuildSearchResultModels(ICollection<Data.Models.Advertisement> advertisements);

        public ICollection<SearchResultModel> FindAdvertisementsByUserId(string userId);

        public AveragePriceModel AveragePricesByGivenBrandAndModel(AveragePriceModel priceModel);

    }
}
