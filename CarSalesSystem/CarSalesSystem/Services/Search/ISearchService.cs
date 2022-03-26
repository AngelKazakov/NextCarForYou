using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Models.Home;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Services.Search
{
    public interface ISearchService
    {
        public Task<ICollection<SearchResultModel>> SearchVehiclesAsync(SearchAdvertisementModel model, string userId);

        public Task<ICollection<SearchResultModel>> DetailedSearchVehiclesAsync(DetailedSearchAdvertisementModel detailedModel, string userId);

        public Task<ICollection<SearchResultModel>> GetLastPublishedAdvertisementsAsync(string userId);

        public Task<ICollection<SearchResultModel>> BuildSearchResultModelsAsync(ICollection<Data.Models.Advertisement> advertisements, string userId);

        public Task<ICollection<SearchResultModel>> FindAdvertisementsByUserIdAsync(string userId);

        public Task<ICollection<SearchResultModel>> GetUserFavoriteAdvertisementsAsync(string userId);

        public Task<AveragePriceModel> AveragePricesByGivenBrandAndModelAsync(AveragePriceModel priceModel, string userId);

        public Task<DetailedSearchAdvertisementModel> InitDetailedSearchAdvertisementModelAsync();

        public Task<AveragePriceModel> InitAveragePriceModelAsync();

        public Task<HomeViewModel> InitHomeViewModelAsync(string userId);
    }
}
