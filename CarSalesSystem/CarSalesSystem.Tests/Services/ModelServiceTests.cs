using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Services.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class ModelServiceTests
    {
        [Fact]
        public void GetModelsPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var modelService = new ModelService(dbContext);
            var model = BuiModel();
            dbContext.Models.Add(model);
            dbContext.SaveChanges();

            //Act
            var result = modelService.GetAllModels("testBrandId");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(model.Name, result.ElementAt(0).Name);
        }

        [Fact]
        public async Task GetModelsAsyncPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("dbTest");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var modelService = new ModelService(dbContext);
            var model = BuiModel();
            dbContext.Models.Add(model);
            await dbContext.SaveChangesAsync();

            //Act
            var result = await modelService.GetAllModelsAsync("testBrandId");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(model.Name, result.ElementAt(0).Name);
        }
    }
}
