using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.Shared
{
    public class FileService : IFileService
    {
        private readonly CarSalesDbContext context;

        public FileService(CarSalesDbContext context)
         =>   this.context = context;
        

        public async Task DeleteFileFromFileSystemByAdvertisementIdAsync(string advertisementId)
        {
            List<string> imageIds = new List<string>();

            var advertisement = await context.Advertisements
                .Include(x => x.VehicleImages)
                .FirstOrDefaultAsync(x => x.Id == advertisementId);

            if (advertisement == null)
            {
                throw new ArgumentNullException(advertisementId, "Record not found.");
            }

            imageIds.AddRange(advertisement.VehicleImages.Select(x => x.Id));

            foreach (var imageId in imageIds)
            {
                await DeleteFileFromFileSystemAsync(imageId);
            }
        }

        public async Task DeleteFileFromFileSystemAsync(string imageId)
        {
            var file = await context.Images.FirstOrDefaultAsync(x => x.Id == imageId);

            if (file == null) return;

            if (File.Exists(file.FullPath))
            {
                File.Delete(file.FullPath);
            }
            context.Images.Remove(file);
            await context.SaveChangesAsync();
        }
    }
}
