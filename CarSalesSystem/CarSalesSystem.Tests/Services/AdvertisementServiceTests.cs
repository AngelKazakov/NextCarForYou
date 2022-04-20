using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Services;
using CarSalesSystem.Services.Advertisement;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.CarDealerShip;
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class AdvertisementServiceTests
    {
        [Fact]
        public async Task GetShouldReturnCorrectAdvertisement()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            dbContext.Advertisements.Add(BuildAdvertisement("15"));
            await dbContext.SaveChangesAsync();
            var advertisementService =
                new AdvertisementService(dbContext, null, null, null, null, null, null, null, null, null);

            //Act
            var advertisement = await advertisementService.GetAdvertisementByIdAsync("15");

            //Assert
            Assert.NotNull(advertisement);
            Assert.Equal("BMW 330CD", advertisement.Name);
        }

        [Fact]
        public async Task GetNotExistingAdvertisement()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
            var dbContext = new CarSalesDbContext(optionsBuilder.Options);
            var advertisementService =
                new AdvertisementService(dbContext, null, null, null, null, null, null, null, null, null);

            //Act
            var advertisement = await advertisementService.GetAdvertisementByIdAsync("13");

            //Assert
            Assert.Null(advertisement);
        }

        [Fact]
        public async Task InitAdvertisementAddFormModelPositive()
        {
            //Arrange Mocks
            var brand = BuildBrand();
            var vehicleCategory = BuildVehicleCategory();
            var color = BuildColor();
            var region = BuildRegion();
            var engine = BuildVehicleEngineType();
            var transmission = BuildTransmissionType();
            var euroStandard = BuildVehicleEuroStandard();
            var extrasCategory = BuildExtrasCategory();
            var dealership = BuildCarDealerShip();

            var brandServiceMock = new Mock<IBrandService>();
            brandServiceMock.Setup(x => x.GetAllBrandsAsync()).ReturnsAsync(new List<Brand>() { brand });
            var brandService = brandServiceMock.Object;

            var technicalServiceMock = new Mock<ITechnicalService>();
            technicalServiceMock.Setup(x => x.GetEngineTypesAsync()).ReturnsAsync(new List<VehicleEngineType>()
                { engine });
            technicalServiceMock.Setup(x => x.GetEuroStandardsAsync()).ReturnsAsync(new List<VehicleEuroStandard>()
                { euroStandard });
            technicalServiceMock.Setup(x => x.GetTransmissionTypesAsync())
                .ReturnsAsync(new List<TransmissionType>() { transmission });
            technicalServiceMock.Setup(x => x.GetExtrasCategoriesAsync())
                .ReturnsAsync(new List<ExtrasCategory>() { extrasCategory });
            var technicalService = technicalServiceMock.Object;

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(x => x.GetVehicleCategoriesAsync())
                .ReturnsAsync(new List<VehicleCategory>() { vehicleCategory });
            var categoryService = categoryServiceMock.Object;

            var colorServiceMock = new Mock<IColorService>();
            colorServiceMock.Setup(x => x.GetColorsAsync()).ReturnsAsync(new List<Color>() { color });
            var colorService = colorServiceMock.Object;

            var regionServiceMock = new Mock<IRegionService>();
            regionServiceMock.Setup(x => x.GetAllRegionsAsync()).ReturnsAsync(new List<Region>() { region });
            var regionService = regionServiceMock.Object;

            var dealerShipServiceMock = new Mock<ICarDealerShipService>();
            dealerShipServiceMock.Setup(x => x.GetAllCarDealershipsByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<CarDealerShip>() { dealership });
            var dealerShipService = dealerShipServiceMock.Object;

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var advertisementService = new AdvertisementService(null, brandService, technicalService, categoryService, regionService, colorService, null, dealerShipService, mapper, null);

            //Act
            var result = await advertisementService.InitAdvertisementAddFormModel("userTest");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(brand.Name, result.Brands.ElementAt(0).Name);
            Assert.Equal(vehicleCategory.Name, result.VehicleCategories.ElementAt(0).Name);
            Assert.Equal(color.Name, result.Colors.ElementAt(0).Name);
            Assert.Equal(region.Name, result.Regions.ElementAt(0).Name);
            Assert.Equal(engine.Name, result.EngineTypes.ElementAt(0).Name);
            Assert.Equal(transmission.Name, result.TransmissionTypes.ElementAt(0).Name);
            Assert.Equal(euroStandard.Name, result.EuroStandards.ElementAt(0).Name);
            Assert.Equal(extrasCategory.Name, result.Extras.ElementAt(0).Name);
            Assert.Equal(dealership.Name, result.Dealerships.ElementAt(0).Name);
        }

        //[Fact]
        //public async Task SaveAdvertisementSuccessfullyToDb()
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("testDb");
        //    var dbContext = new CarSalesDbContext(optionsBuilder.Options);

        //    var advertisementService =
        //        new AdvertisementService(dbContext, null, null, null, null, null, null, null, null, null);

        //    await advertisementService.SaveAsync(BuildAdvertisement(), new List<string>() { "dsc", "abs" }, null);
        //}
    }
}