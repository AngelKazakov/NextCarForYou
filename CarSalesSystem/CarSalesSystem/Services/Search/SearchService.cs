using System.Collections.Generic;
using CarSalesSystem.Data;
using CarSalesSystem.Models.Search;

namespace CarSalesSystem.Services.Search
{
    public class SearchService : ISearchService
    {
        private readonly CarSalesDbContext context;

        public SearchService(CarSalesDbContext context)
         => this.context = context;


        public IEnumerable<SearchResultModel> SearchVehicles(SearchAdvertisementModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
