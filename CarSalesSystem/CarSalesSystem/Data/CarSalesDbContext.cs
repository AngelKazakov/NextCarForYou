using CarSalesSystem.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Data
{
    public class CarSalesDbContext : IdentityDbContext<User>
    {
        public CarSalesDbContext(DbContextOptions<CarSalesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; init; }

        public DbSet<Advertisement> Advertisements { get; init; }

        public DbSet<CarDealerShip> CarDealerShips { get; init; }

        public DbSet<ExtrasCategory> Categories { get; init; }

        public DbSet<Extras> Extras { get; init; }

        public DbSet<Region> Regions { get; init; }

        public DbSet<City> Cities { get; init; }

        public DbSet<Brand> Brands { get; init; }

        public DbSet<Model> Models { get; init; }

        public DbSet<VehicleImage> Images { get; init; }

        public DbSet<TransmissionType> Transmissions { get; init; }

        public DbSet<VehicleEuroStandard> EuroStandards { get; init; }

        public DbSet<VehicleCategory> VehicleCategories { get; init; }

        public DbSet<VehicleEngineType> Engines { get; init; }

        public DbSet<Color> Colors { get; init; }

        public DbSet<AdvertisementExtra> AdvertisementsExtras { get; init; }

        public DbSet<UserFavAdvertisement> UserFavAdvertisementsExtras { get; init; }

        //TODO check deleting...
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Advertisement>()
                .Property(a => a.Price)
                .HasColumnType("decimal");


            builder.Entity<AdvertisementExtra>().
                HasKey(x => new { x.AdvertisementId, x.ExtrasId });

            builder
                .Entity<Advertisement>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder
                .Entity<Advertisement>()
                .HasMany(a => a.VehicleImages)
                .WithOne(x => x.Advertisement)
                .HasForeignKey(x => x.AdvertisementId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
