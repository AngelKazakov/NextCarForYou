using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Services.User;
using Microsoft.EntityFrameworkCore;
using Xunit;

using static CarSalesSystem.Tests.TestDataFactory.TestDataFactory;

namespace CarSalesSystem.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task AddAdvertisementToFavoritePositive()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("addToFavTesDb");
            var context = new CarSalesDbContext(optionsBuilder.Options);
            var advertisement = BuildAdvertisement();
            var user = BuildUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var userService = new UserService(context);

            bool result = await userService.AddAdvertisementToFavoriteAsync(advertisement.Id, user.Id);

            Assert.True(result);
            Assert.Equal(user.FavAdvertisements.ElementAt(0).AdvertisementId, advertisement.Id);
        }

        [Fact]
        public async Task AddAdvertisementToFavoriteNegative()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("addToFavTesDb");
            var context = new CarSalesDbContext(optionsBuilder.Options);
            var advertisement = BuildAdvertisement();
            var user = BuildUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var userService = new UserService(context);

            bool resultTrue = await userService.AddAdvertisementToFavoriteAsync(advertisement.Id, user.Id);
            bool resultFalse = await userService.AddAdvertisementToFavoriteAsync(advertisement.Id, user.Id);

            Assert.False(resultFalse);
        }

        [Fact]
        public async Task RemoveAdvertisementFromFavoritePositive()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("removeFromFavTestDb");
            var context = new CarSalesDbContext(optionsBuilder.Options);
            var advertisement = BuildAdvertisement();
            var user = BuildUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var userService = new UserService(context);

            var addAdvertisementResult = await userService.AddAdvertisementToFavoriteAsync(advertisement.Id, user.Id);
            var removeAdvertisementResult = await userService.RemoveAdvertisementFromFavoriteAsync(advertisement.Id, user.Id);

            Assert.True(removeAdvertisementResult);
        }

        [Fact]
        public async Task RemoveAdvertisementFromFavoriteNegative()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarSalesDbContext>().UseInMemoryDatabase("removeFromFavTestDb");
            var context = new CarSalesDbContext(optionsBuilder.Options);
            var advertisement = BuildAdvertisement();
            var user = BuildUser();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var userService = new UserService(context);

            var removeAdvertisementResult = await userService.RemoveAdvertisementFromFavoriteAsync(advertisement.Id, user.Id);

            Assert.False(removeAdvertisementResult);
        }
    }
}
