using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.Regions
{
    public class RegionService : IRegionService
    {
        private readonly CarSalesDbContext data;

        public RegionService(CarSalesDbContext data)
           => this.data = data;

        public async Task<ICollection<Region>> GetAllRegionsAsync()
        {
            return await this.data.Regions
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public ICollection<City> GetAllCities(string regionId)
        {
            return this.data.Cities
                .Where(x => x.RegionId == regionId)
                .OrderBy(x => x.Name)
                .ToList();
        }

        public async Task<ICollection<City>> GetAllCitiesAsync(string regionId)
        {
            return await this.data.Cities
                .Where(x => x.RegionId == regionId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
