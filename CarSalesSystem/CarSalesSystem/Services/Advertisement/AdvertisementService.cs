using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using Microsoft.AspNetCore.Http;
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
