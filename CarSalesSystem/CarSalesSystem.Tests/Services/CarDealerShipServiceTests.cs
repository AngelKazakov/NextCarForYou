using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Exceptions;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Services.CarDealerShip;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NuGet.Packaging;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class CarDealerShipServiceTests
    {
        [Fact]
        public async Task CreateDealershipPositive()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("createDealership");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealership = BuildCarDealerShip();
            await context.SaveChangesAsync();
            var dealerService = new CarDealerShipService(context, null);

            var result = await dealerService.CreateDealerShipAsync(dealership);
            var str = result;

            Assert.NotNull(result);
            Assert.Equal(dealership.Id, str);
            Assert.Contains(dealership, context.CarDealerShips);
        }

        [Fact]
        public async Task UpdateCarDealershipPositive()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("updateDealershipDb");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealerShip = new CarDealerShip() { Id = "dealershipId", Name = "dealerName", Address = "dealerAddress", Phone = "phoneNum", Email = "emailAddress", UserId = "userId" };
            context.CarDealerShips.Add(dealerShip);
            await context.SaveChangesAsync();
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.jpg");
            var dealerShipFormModel =
                new CarDealershipAddFormModel() { Id = "dealershipId", Name = "dealerUpdatedName", Address = "dealerUpdatedAddress", Phone = "phoneUpdated", Email = "emailUpdated", Image = file };
            var dealerService = new CarDealerShipService(context, null);
            await dealerService.UpdateCarDealershipAsync(dealerShipFormModel);

            Assert.Equal("dealerUpdatedName", dealerShip.Name);
            Assert.NotEmpty(dealerShip.ImageLogo);
        }

        [Fact]
        public async Task UpdateCarDealershipWhenImageIsDeletedShouldBeNullInDb()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("updateDealershipWithNullImageDb");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealerShip = new CarDealerShip() { Id = "dealershipId", Name = "dealerName", Address = "dealerAddress", Phone = "phoneNum", Email = "emailAddress", UserId = "userId" };
            context.CarDealerShips.Add(dealerShip);
            await context.SaveChangesAsync();
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.jpg");
            var dealerShipFormModel =
                new CarDealershipAddFormModel() { Id = "dealershipId", Name = "dealerUpdatedName", Address = "dealerUpdatedAddress", Phone = "phoneUpdated", Email = "emailUpdated", ImageDeleted = true };
            var dealerService = new CarDealerShipService(context, null);
            await dealerService.UpdateCarDealershipAsync(dealerShipFormModel);

            Assert.Null(dealerShip.ImageLogo);
        }

        [Fact]
        public async Task DeleteCarDealershipPositive()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("deleteDealershipDb")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealerShip =
             new CarDealerShip() { Id = "dealershipId", Name = "dealerName", Address = "dealerAddress", Phone = "phoneNum", Email = "emailAddress", UserId = "userId" };
            context.CarDealerShips.Add(dealerShip);
            await context.SaveChangesAsync();


            var dealershipService = new CarDealerShipService(context, null);
            await dealershipService.DeleteCarDealershipAsync("dealershipId", "userId");

            Assert.DoesNotContain(dealerShip, context.CarDealerShips);
        }

        [Fact]
        public async Task DeleteCarDealershipWithNotCorrectUserId()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("deleteDealershipWithNoCorrectUserIdDb")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealerShip =
                new CarDealerShip() { Id = "dealershipId", Name = "dealerName", Address = "dealerAddress", Phone = "phoneNum", Email = "emailAddress", UserId = "userId" };
            context.CarDealerShips.Add(dealerShip);
            await context.SaveChangesAsync();


            var dealershipService = new CarDealerShipService(context, null);

            var action = async () => await dealershipService.DeleteCarDealershipAsync("dealershipId", "idUser");

            var caughtException = Assert.ThrowsAsync<Exception>(action);

            Assert.Equal("You do not have permission to delete this dealership.", caughtException.Result.Message);
        }

        [Fact]
        public async Task DeleteCarDealershipWhenDoesNotExist()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("deleteDealershipWhenDoesNotExist")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealerShip =
                new CarDealerShip() { Id = "dealershipId", Name = "dealerName", Address = "dealerAddress", Phone = "phoneNum", Email = "emailAddress", UserId = "userId" };
            context.CarDealerShips.Add(dealerShip);
            await context.SaveChangesAsync();

            var dealershipService = new CarDealerShipService(context, null);

            var action = async () => await dealershipService.DeleteCarDealershipAsync("dealerId", "userId");

            var caughtException = Assert.ThrowsAsync<ArgumentNullException>(action);

            Assert.Equal("dealerId", caughtException.Result.ParamName);
            Assert.True(caughtException.Result.Message.Contains("Dealership does not exist."));
        }

        //[Fact]
        //public async Task DeleteCarDealershipWithAllAdvertisements()
        //{
        //    var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("deleteDealershipWithAllAdvertisements")
        //        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        //    var context = new CarSalesDbContext(optBuilder.Options);
        //    var advertisement = BuildAdvertisement("11");
        //    context.Advertisements.Add(advertisement);
        //    await context.SaveChangesAsync();
        //    var dealerShip =
        //        new CarDealerShip()
        //        {
        //            Id = "dealershipId",
        //            Name = "dealerName",
        //            Address = "dealerAddress",
        //            Phone = "phoneNum",
        //            Email = "emailAddress",
        //            UserId = "userId",
        //            Advertisements = new List<Advertisement>() { advertisement }
        //        };

        //    context.CarDealerShips.Add(dealerShip);
        //    await context.SaveChangesAsync();

        //    var dealershipService = new CarDealerShipService(context, null);

        //    await dealershipService.DeleteCarDealershipAsync("dealershipId", "userId");

        //    Assert.Empty(dealerShip.Advertisements);
        //}

        [Fact]
        public async Task GetAllCarDealershipsByIdPositive()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("getDealershipsByIdDb");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealershipOne = new CarDealerShip() { Id = "dealershipOne", Name = "dealerName", Address = "dealerAddress", Phone = "phoneOne", Email = "emailOne", UserId = "userId" };
            var dealershipTwo = new CarDealerShip() { Id = "dealershipTwo", Name = "dealerNameTwo", Address = "dealerAddressTwo", Phone = "phoneTwo", Email = "emailTwo", UserId = "userIdTwo" };
            context.CarDealerShips.Add(dealershipOne);
            context.CarDealerShips.Add(dealershipTwo);
            await context.SaveChangesAsync();
            var dealerService = new CarDealerShipService(context, null);

            var result = await dealerService.GetCarDealershipAsync("dealershipTwo");

            Assert.NotNull(result);
            Assert.Equal("dealershipTwo", dealershipTwo.Id);

        }

        [Fact]
        public async Task GetAllCarDealershipsByUserIdPositive()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("getDealershipsDb");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealershipOne = new CarDealerShip() { Name = "dealerName", Address = "dealerAddress", Phone = "phoneOne", Email = "emailOne", UserId = "userId" };
            var dealershipTwo = new CarDealerShip() { Name = "dealerNameTwo", Address = "dealerAddressTwo", Phone = "phoneTwo", Email = "emailTwo", UserId = "userIdTwo" };
            context.CarDealerShips.Add(dealershipOne);
            context.CarDealerShips.Add(dealershipTwo);
            await context.SaveChangesAsync();
            var dealerService = new CarDealerShipService(context, null);

            var result = await dealerService.GetAllCarDealershipsByUserIdAsync("userIdTwo");

            Assert.NotNull(result);
            Assert.Contains(dealershipTwo, result);
            Assert.DoesNotContain(dealershipOne, result);

        }

        [Fact]
        public async Task GetAllCarDealershipsPositive()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("getDealershipsDb");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealership = BuildCarDealerShip();
            context.CarDealerShips.Add(dealership);
            await context.SaveChangesAsync();
            var dealerService = new CarDealerShipService(context, null);

            var result = await dealerService.GetAllCarDealershipsAsync();

            Assert.NotNull(result);
            Assert.Contains(dealership, result);

        }

        [Fact]
        public async Task GetAdvertisementsByDealershipIdAsyncPositive()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("getAdvertisementsByDealershipIdDb");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealership = BuildCarDealerShip();
            var advertisementOne = BuildAdvertisement();
            context.Advertisements.Add(advertisementOne);
            advertisementOne.CarDealershipId = "dealershipId";
            await context.SaveChangesAsync();
            var dealerService = new CarDealerShipService(context, null);

            var result = await dealerService.GetAdvertisementsByDealershipIdAsync("dealershipId");

            Assert.NotNull(result);
            Assert.Contains(advertisementOne, result);

        }

        [Fact]
        public async Task CreateDealershipThrowsExceptionForAlreadyExistingWithTheSameName()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("existingName");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealership = BuildCarDealerShip();
            var dealerService = new CarDealerShipService(context, null);

            var firstResult = await dealerService.CreateDealerShipAsync(dealership);

            var action = async () => await dealerService.CreateDealerShipAsync(dealership);

            var caughtException = Assert.ThrowsAsync<DuplicateCarDealerShipException>(action);
            Assert.Equal($"Car dealership with name: {dealership.Name} already exists.", caughtException.Result.Message);
        }

        [Fact]
        public async Task CreateDealershipThrowsExceptionForAlreadyExistingWithTheSameEmail()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("existingEmail");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealership = BuildCarDealerShip();
            var dealerService = new CarDealerShipService(context, null);

            var firstResult = await dealerService.CreateDealerShipAsync(dealership);

            dealership.Name = string.Empty;
            dealership.Url = string.Empty;

            var action = async () => await dealerService.CreateDealerShipAsync(dealership);

            var caughtException = Assert.ThrowsAsync<DuplicateCarDealerShipException>(action);
            Assert.Equal($"Car dealership with email: {dealership.Email} already exists.", caughtException.Result.Message);
        }

        [Fact]
        public async Task CreateDealershipThrowsExceptionForAlreadyExistingWithTheSameWebSite()
        {
            var optBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("existingUrl");
            var context = new CarSalesDbContext(optBuilder.Options);
            var dealership = BuildCarDealerShip();
            var dealerService = new CarDealerShipService(context, null);
            dealership.Url = "https://mobile.bg";
            var firstResult = await dealerService.CreateDealerShipAsync(dealership);
            dealership.Name = string.Empty;
            dealership.Email = string.Empty;
            var action = async () => await dealerService.CreateDealerShipAsync(dealership);

            var caughtException = Assert.ThrowsAsync<DuplicateCarDealerShipException>(action);
            Assert.Equal($"Car dealership with website: {dealership.Url} already exists.", caughtException.Result.Message);
        }
    }
}
