using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Services.Colors;
using Microsoft.EntityFrameworkCore;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class ColorServiceTests
    {
        [Fact]
        public async Task GetAllColorsPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var colorService = new ColorService(dbContext);
            var color = BuildColor();
            dbContext.Colors.Add(color);
            await dbContext.SaveChangesAsync();

            //Act
            var result = await colorService.GetColorsAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(color.Name, result.ElementAt(0).Name);
        }
    }
}
