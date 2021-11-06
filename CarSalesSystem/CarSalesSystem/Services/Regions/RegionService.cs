using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Regions
{
    public class RegionService : IRegionService
    {
        private readonly CarSalesDbContext data;

        public RegionService(CarSalesDbContext data)
           => this.data = data;

        public ICollection<Region> GetAllRegions()
        {
            return this.data.Regions
                .OrderBy(x => x.Name)
                .ToList();
        }

        public ICollection<City> GetAllCities(string regionId)
        {
            return this.data.Cities
                .Where(x => x.RegionId == regionId)
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}
