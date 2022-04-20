using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.EntityFrameworkCore;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class TechnicalServiceTests
    {
        [Fact]
        public async Task GetEngineTypesPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("engineTypesDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var firstEngine = BuildVehicleEngineType();
            var secondEngine = new VehicleEngineType() { Id = "engineId", Name = "Petrol" };
            dbContext.Engines.Add(firstEngine);
            dbContext.Engines.Add(secondEngine);
            await dbContext.SaveChangesAsync();
            var technicalService = new TechnicalService(dbContext);

            //Act
            var result = await technicalService.GetEngineTypesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Contains(firstEngine, result);
            Assert.Contains(secondEngine, result);
        }

        [Fact]
        public async Task GetEuroStandardsPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("euroDb");
            var context = new CarSalesDbContext(optionsBuilder.Options);
            var firstEuro = BuildVehicleEuroStandard();
            var secondEuro = new VehicleEuroStandard() { Id = "secondEuro", Name = "secondName" };
            context.EuroStandards.Add(firstEuro);
            context.EuroStandards.Add(secondEuro);
            await context.SaveChangesAsync();
            var technicalService = new TechnicalService(context);

            //Act
            var result = await technicalService.GetEuroStandardsAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Contains(firstEuro, result);
            Assert.Contains(secondEuro, result);
        }

        [Fact]
        public async Task GetVehicleTransmissionsPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("transmissionTestDb");
            var context = new CarSalesDbContext(optionsBuilder.Options);
            var manualTransmission = new TransmissionType() { Id = "transOneId", Name = "Manual" };
            var automaticTransmission = new TransmissionType() { Id = "transTwoId", Name = "Automatic" };
            context.Transmissions.Add(manualTransmission);
            context.Transmissions.Add(automaticTransmission);
            await context.SaveChangesAsync();
            var technicalService = new TechnicalService(context);

            //Act
            var result = await technicalService.GetTransmissionTypesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Contains(manualTransmission, result);
            Assert.Contains(automaticTransmission, result);
        }

        [Fact]
        public async Task GetExtrasCategoriesPositive()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("extrasCategoryTestDb");
            var context = new CarSalesDbContext(optionsBuilder.Options);
            var firstExtraCategory = BuildExtrasCategory();
            context.Categories.Add(firstExtraCategory);
            await context.SaveChangesAsync();
            var technicalService = new TechnicalService(context);

            //Act
            var result = await technicalService.GetExtrasCategoriesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Contains(firstExtraCategory, result);
        }
    }
}
