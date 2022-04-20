using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Services.Categories;
using Microsoft.EntityFrameworkCore;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetAllCategoriesPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var categoryService = new CategoryService(dbContext);
            var vehicleCategory = BuildVehicleCategory();

            //Act
            dbContext.VehicleCategories.Add(vehicleCategory);
            await dbContext.SaveChangesAsync();
            var result = categoryService.GetVehicleCategoriesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(vehicleCategory.Name, result.Result.ElementAt(0).Name);
        }
    }
}
