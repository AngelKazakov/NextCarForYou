using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Services.Brands;
using Microsoft.EntityFrameworkCore;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class BrandServiceTests
    {
        [Fact]
        public async Task GetAllBrandsAsyncPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var brandService = new BrandService(dbContext);
            var brand = BuildBrand();

            //Act
            dbContext.Brands.AddRange(new List<Brand>() { brand });
            await dbContext.SaveChangesAsync();
            var result = await brandService.GetAllBrandsAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Contains(brand, result);
        }
    }
}
