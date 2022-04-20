using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Services.Regions;
using Microsoft.EntityFrameworkCore;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class RegionServiceTests
    {
        [Fact]
        public async Task GetAllRegionsPositive()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var regionService = new RegionService(dbContext);
            var region = BuildRegion();
            dbContext.Regions.Add(region);
            await dbContext.SaveChangesAsync();

            var result = await regionService.GetAllRegionsAsync();

            Assert.NotNull(result);
            Assert.Equal(region.Id, result.ElementAt(0).Id);
        }

        [Fact]
        public async Task GetAllCitiesAsyncPositive()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("citiesDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var regionService = new RegionService(dbContext);
            var city = BuildCity();
            var region = new Region() { Id = "regionId", Name = "regionName", Cities = new List<City>() { city, new City() { Id = "cityId", Name = "cityName", RegionId = "regionId" } } };
            dbContext.Regions.Add(region);
            await dbContext.SaveChangesAsync();

            var result = await regionService.GetAllCitiesAsync(region.Id);

            Assert.NotNull(result);
            Assert.Equal(region.Id, result.ElementAt(0).RegionId);
        }

        [Fact]
        public void GetAllCitiesPositive()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("citiesTestDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var regionService = new RegionService(dbContext);
            var city = BuildCity();
            var region = new Region() { Id = "regionId", Name = "regionName", Cities = new List<City>() { city, new City() { Id = "cityId", Name = "cityName", RegionId = "regionId" } } };
            dbContext.Regions.Add(region);
            dbContext.SaveChangesAsync();

            var result = regionService.GetAllCities(region.Id);

            Assert.NotNull(result);
            Assert.Equal(region.Id, result.ElementAt(0).RegionId);
        }
    }
}
