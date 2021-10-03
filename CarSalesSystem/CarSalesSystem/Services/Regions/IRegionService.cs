using System.Collections.Generic;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Regions
{
    public interface IRegionService
    {
        ICollection<Region> GetAllRegions();

        ICollection<City> GetAllCities(string regionId);
    }
}
