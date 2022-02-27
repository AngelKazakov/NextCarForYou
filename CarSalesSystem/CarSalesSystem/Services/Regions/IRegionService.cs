using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Regions
{
    public interface IRegionService
    {
        Task<ICollection<Region>> GetAllRegionsAsync();

        ICollection<City> GetAllCities(string regionId);

        Task<ICollection<City>> GetAllCitiesAsync(string regionId);
    }
}
