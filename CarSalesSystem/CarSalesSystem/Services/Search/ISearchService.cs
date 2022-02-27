using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Services.Search
{
    public interface ISearchService
    {

        public Task<ICollection<SearchResultModel>> SearchVehiclesAsync(SearchAdvertisementModel model);

        public Task<ICollection<SearchResultModel>> DetailedSearchVehiclesAsync(DetailedSearchAdvertisementModel detailedModel);

        public Task<ICollection<SearchResultModel>> GetLastPublishedAdvertisementsAsync();

        public Task<ICollection<SearchResultModel>> BuildSearchResultModelsAsync(ICollection<Data.Models.Advertisement> advertisements);

        public Task<ICollection<SearchResultModel>> FindAdvertisementsByUserIdAsync(string userId);

        public Task<AveragePriceModel> AveragePricesByGivenBrandAndModelAsync(AveragePriceModel priceModel);
    }
}
