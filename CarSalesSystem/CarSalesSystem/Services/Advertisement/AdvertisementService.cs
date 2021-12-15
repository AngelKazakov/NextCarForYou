using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.Advertisement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Services.Advertisement
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly CarSalesDbContext context;

        public AdvertisementService(CarSalesDbContext context)
        => this.context = context;

        public void Save(Data.Models.Advertisement advertisement, List<string> extrasIds, IFormFileCollection images)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    this.context.Advertisements.Add(advertisement);
                    context.SaveChanges();

                    var advertisementId = advertisement.Id;

                    foreach (var extraId in extrasIds)
                    {
                        AdvertisementExtra advertisementExtra = new AdvertisementExtra()
                        {
                            AdvertisementId = advertisementId,
                            ExtrasId = extraId
                        };

                        context.AdvertisementsExtras.Add(advertisementExtra);
                    }

                    context.SaveChanges();

                    SaveImages(images, advertisementId).GetAwaiter().GetResult();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                }
            }
        }

        public void Edit(AdvertisementViewModel advertisement, string advertisementId, string userId)
        {
            throw new NotImplementedException();
        }

        public void Delete(string Id, string UserId)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                string advertisementUserId = context.Advertisements.Where(x => x.Id == Id).Select(x => x.UserId)
                    .FirstOrDefault();

                if (advertisementUserId != UserId)
                {
                    throw new Exception("You do not have permission to delete this advertisement.");
                }

                var path = "Advertisement" + Id;

                var dir = new DirectoryInfo(ImagesPath + "\\" + path);
                dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                dir.Delete(true);

                context.Advertisements.Remove(context.Advertisements.First(adv => adv.Id == Id));
                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public AdvertisementViewModel GetAdvertisementById(string advertisementId)
        {
            Data.Models.Advertisement advertisement =
                context.Advertisements
                    .Include(x => x.Vehicle)
                    .Include(x => x.City)
                    .Include(x => x.City.Region)
                    .Include(x => x.Vehicle.Color)
                    .Include(x => x.Vehicle.EuroStandard)
                    .Include(x => x.Vehicle.EngineType)
                    .Include(x => x.Vehicle.Category)
                    .Include(x => x.Vehicle.TransmissionType)
                    .Include(x => x.Vehicle.Model)
                    .Include(x => x.CarDealerShip)
                    .Include(x => x.User)
                    .Include(x => x.AdvertisementExtras)
                    .ThenInclude(x => x.Extras)
                    .ThenInclude(x => x.Category)
                    .FirstOrDefault(x => x.Id == advertisementId);

            return AdvertisementCustomMapper.Map(advertisement);
        }

        private async Task SaveImages(IFormFileCollection files, string advertisementId)
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(ImagesPath);

            DirectoryInfo subDirectoryInfo = directoryInfo.CreateSubdirectory($"Advertisement{advertisementId}");

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    await using var stream = new FileStream(Path.Combine(subDirectoryInfo.FullName, formFile.FileName), FileMode.Create, FileAccess.Write);
                    await formFile.CopyToAsync(stream);
                }
            }
        }
    }
}
