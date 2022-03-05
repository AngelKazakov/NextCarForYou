using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.Advertisement;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.City;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.ExtrasCategory;
using CarSalesSystem.Models.Model;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.CarDealerShip;
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.TechnicalData;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Services.Advertisement
{

    public class AdvertisementService : IAdvertisementService
    {
        private readonly CarSalesDbContext context;
        private readonly IBrandService brandService;
        private readonly ITechnicalService technicalService;
        private readonly IModelService modelService;
        private readonly ICategoryService categoryService;
        private readonly IRegionService regionService;
        private readonly IColorService colorService;
        private readonly ICarDealerShipService dealerShipService;
        private readonly IMapper mapper;

        public AdvertisementService(CarSalesDbContext context,
            IBrandService brandService,
            ITechnicalService technicalService,
            ICategoryService categoryService,
            IRegionService regionService,
            IColorService colorService,
            IModelService modelService,
            ICarDealerShipService dealerShipService,
            IMapper mapper)
        {
            this.context = context;
            this.brandService = brandService;
            this.mapper = mapper;
            this.dealerShipService = dealerShipService;
            this.modelService = modelService;
            this.colorService = colorService;
            this.regionService = regionService;
            this.categoryService = categoryService;
            this.technicalService = technicalService;
        }

        public async Task<string> SaveAsync(Data.Models.Advertisement advertisement, List<string> extrasIds, ICollection<IFormFile> images)
        {
            await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                this.context.Advertisements.Add(advertisement);
                await context.SaveChangesAsync();

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

                await SaveImagesAsync(images, advertisementId, advertisement.VehicleImages);

                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return advertisementId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error occurred.");
                throw ex;
            }
        }

        public async Task<string> EditAsync(AdvertisementAddFormModel advertisementStep1, AdvertisementAddFormModelStep2 advertisementStep2, string userId)
        {
            var currentAdvertisement = await context.Advertisements
                .Include(x => x.Vehicle)
                .Include(x => x.VehicleImages)
                .Include(x => x.AdvertisementExtras)
                .FirstOrDefaultAsync(x => x.Id == advertisementStep1.Id);

            if (currentAdvertisement == null)
            {
                throw new Exception("Record not found.");
            }

            if (currentAdvertisement.UserId != userId)
            {
                throw new Exception("You do not have permission to edit this advertisement.");
            }

            await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await UpdateAdvertisement(currentAdvertisement, advertisementStep1, advertisementStep2);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex);
                throw ex;
            }

            return currentAdvertisement.Id;
        }

        public async Task DeleteAsync(string Id, string UserId)
        {
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                string advertisementUserId = await context.Advertisements
                    .Where(x => x.Id == Id)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();

                if (advertisementUserId != UserId)
                {
                    throw new Exception("You do not have permission to delete this advertisement.");
                }

                Data.Models.Advertisement advertisement = context.Advertisements.Include(x => x.Vehicle).First(adv => adv.Id == Id);
                context.Advertisements.Remove(advertisement);
                context.Vehicles.Remove(advertisement.Vehicle);
                await context.SaveChangesAsync();

                var path = "Advertisement" + Id;

                var dir = new DirectoryInfo(ImagesPath + "\\" + path);
                dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                dir.Delete(true);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }

        public async Task<Data.Models.Advertisement> GetAdvertisementByIdAsync(string advertisementId)
        {
            Data.Models.Advertisement advertisement =
              await context.Advertisements
                    .Include(x => x.Vehicle)
                    .Include(x => x.Vehicle.Model.Brand)
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
                    .Include(x => x.VehicleImages)
                    .Include(x => x.AdvertisementExtras)
                    .ThenInclude(x => x.Extras)
                    .ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == advertisementId);


            return advertisement;
        }

        public async Task<AdvertisementAddFormModel> GetRecordDataAsync(string advertisementId, string userId)
        {
            var model = await GetAdvertisementByIdAsync(advertisementId);

            var advertisementAddFormModel = new AdvertisementAddFormModel
            {
                Id = advertisementId,
                BrandFormModel = new BrandFormModel { Id = model.Vehicle.Model.BrandId, Name = model.Vehicle.Model.Brand.Name },
                ModelFormModel = new ModelFormModel { Name = model.Vehicle.Model.Name, Id = model.Vehicle.ModelId },
                CategoryFormModel = new CategoryFormModel { Name = model.Vehicle.Category.Name, Id = model.Vehicle.CategoryId },
                ColorFormModel = new ColorFormModel { Name = model.Vehicle.Color.Name, Id = model.Vehicle.ColorId },
                RegionFormModel = new RegionFormModel { Name = model.City.Region.Name, Id = model.City.RegionId },
                CityFormModel = new CityFormModel { Name = model.City.Name, Id = model.CityId },
                DealershipFormModel = model.CarDealershipId == null ? null : new CarDealershipFormModel { Name = model.CarDealerShip.Name, Id = model.CarDealershipId },
                EngineTypeFormModel = new EngineFormModel { Name = model.Vehicle.EngineType.Name, Id = model.Vehicle.EngineTypeId },
                Mileage = model.Vehicle.Mileage,
                Power = model.Vehicle.Power,
                Price = model.Price,
                TransmissionTypeFormModel = new TransmissionFormModel { Name = model.Vehicle.TransmissionType.Name, Id = model.Vehicle.TransmissionTypeId },
                EuroStandardFormModel = new EuroStandardFormModel { Name = model.Vehicle.EuroStandard.Name, Id = model.Vehicle.EuroStandardId },
                Year = model.Vehicle.Year,
                Month = model.Vehicle.Month,
                Colors = mapper.Map<ICollection<Color>, ICollection<ColorFormModel>>(await colorService.GetColorsAsync()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<EuroStandardFormModel>>(await technicalService.GetEuroStandardsAsync()),
                Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(await regionService.GetAllRegionsAsync()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<CategoryFormModel>>(await categoryService.GetVehicleCategoriesAsync()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                Models = mapper.Map<ICollection<Model>, ICollection<ModelFormModel>>(await modelService.GetAllModelsAsync(model.Vehicle.Model.BrandId)),
                Cities = mapper.Map<ICollection<City>, ICollection<CityFormModel>>(await regionService.GetAllCitiesAsync(model.City.RegionId)),
                Extras = mapper.Map<ICollection<ExtrasCategory>, ICollection<ExtrasCategoryFormModel>>(await technicalService.GetExtrasCategoriesAsync()),
                Dealerships = mapper.Map<ICollection<Data.Models.CarDealerShip>, ICollection<CarDealershipViewModel>>(await dealerShipService.GetAllCarDealershipsByUserIdAsync(userId))
            };


            List<string> SelectedExtras = model.AdvertisementExtras.Select(x => x.Extras.Name).ToList();

            foreach (var category in advertisementAddFormModel.Extras)
            {
                foreach (var extra in category.Extras)
                {
                    if (SelectedExtras.Contains(extra.Name))
                    {
                        extra.Checked = true;
                    }
                }
            }

            return advertisementAddFormModel;
        }

        public async Task<AdvertisementAddFormModelStep2> GetRecordDataStep2Async(string advertisementId)
        {
            Data.Models.Advertisement model = await GetAdvertisementByIdAsync(advertisementId);

            var advertisementAddFormModel2 = new AdvertisementAddFormModelStep2
            {
                Id = advertisementId,
                Name = model.Name,
                Description = model.Description,
            };

            Dictionary<string, byte[]> idsToImages = new Dictionary<string, byte[]>();
            foreach (var vehicleImage in model.VehicleImages)
            {
                if (!string.IsNullOrWhiteSpace(vehicleImage.FullPath))
                {
                    idsToImages.Add(vehicleImage.Id, await File.ReadAllBytesAsync(vehicleImage.FullPath));
                }
            }

            advertisementAddFormModel2.ImagesForDisplay = idsToImages;

            return advertisementAddFormModel2;
        }

        private async Task DeleteFileFromFileSystemAsync(string id)
        {
            var file = await context.Images.FirstOrDefaultAsync(x => x.Id == id);

            if (file == null) return;

            if (File.Exists(file.FullPath))
            {
                File.Delete(file.FullPath);
            }
            context.Images.Remove(file);
            await context.SaveChangesAsync();
        }

        private async Task SaveImagesAsync(ICollection<IFormFile> files, string advertisementId, ICollection<VehicleImage> vehicleImages)
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(ImagesPath);

            DirectoryInfo subDirectoryInfo = directoryInfo.CreateSubdirectory($"Advertisement{advertisementId}");

            Dictionary<string, VehicleImage> images = new Dictionary<string, VehicleImage>();

            foreach (var image in vehicleImages)
            {
                images.Add(image.Name, image);
            }

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = images.GetValueOrDefault(formFile.FileName)!.UniqueName + Path.GetExtension(formFile.FileName).ToLower();
                    string fullPath = Path.Combine(subDirectoryInfo.FullName, fileName);
                    await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    await formFile.CopyToAsync(stream);

                    images.GetValueOrDefault(formFile.FileName)!.FullPath = fullPath;
                }
            }
        }

        private async Task UpdateAdvertisement(Data.Models.Advertisement advertisement, AdvertisementAddFormModel addModelStep1, AdvertisementAddFormModelStep2 addModelStep2)
        {
            advertisement.Vehicle.ModelId = addModelStep1.ModelFormModel.Id;
            advertisement.CityId = addModelStep1.CityFormModel.Id;
            advertisement.Price = addModelStep1.Price;
            advertisement.Name = addModelStep2.Name;
            advertisement.Description = addModelStep2.Description;
            advertisement.LastModifiedOn = DateTime.UtcNow;
            advertisement.Vehicle.Power = addModelStep1.Power;
            advertisement.Vehicle.ColorId = addModelStep1.ColorFormModel.Id;
            advertisement.Vehicle.Mileage = addModelStep1.Mileage;
            advertisement.Vehicle.Year = addModelStep1.Year;
            advertisement.Vehicle.CategoryId = addModelStep1.CategoryFormModel.Id;
            advertisement.Vehicle.EngineTypeId = addModelStep1.EngineTypeFormModel.Id;
            advertisement.Vehicle.TransmissionTypeId = addModelStep1.TransmissionTypeFormModel.Id;
            advertisement.Vehicle.EuroStandardId = addModelStep1.EuroStandardFormModel.Id;
            advertisement.CarDealershipId = addModelStep1.DealershipFormModel.Id;

            List<AdvertisementExtra> extras = new List<AdvertisementExtra>();

            foreach (var selectedExtraId in addModelStep1.SelectedExtras)
            {
                var advertisementExtra = advertisement.AdvertisementExtras.FirstOrDefault(x => x.ExtrasId == selectedExtraId);

                if (advertisementExtra == null)
                {
                    extras.Add(new AdvertisementExtra()
                    {
                        AdvertisementId = advertisement.Id,
                        ExtrasId = selectedExtraId
                    });
                }
            }

            context.AdvertisementsExtras.AddRange(extras);

            var extrasForRemove = new List<AdvertisementExtra>();

            foreach (var advertisementExtra in advertisement.AdvertisementExtras)
            {
                var selectedExtra = addModelStep1.SelectedExtras.FirstOrDefault(x => x == advertisementExtra.ExtrasId);

                if (selectedExtra == null)
                {
                    extrasForRemove.Add(advertisementExtra);
                }
            }

            foreach (var extraToRemove in extrasForRemove)
            {
                advertisement.AdvertisementExtras.Remove(extraToRemove);
            }

            var vehicleImages = AdvertisementCustomMapper.CreateVehicleImages(addModelStep2.Images);

            foreach (var img in vehicleImages)
            {
                img.AdvertisementId = advertisement.Id;
            }

            await SaveImagesAsync(addModelStep2.Images, addModelStep1.Id, vehicleImages);

            context.Images.AddRange(vehicleImages);

            if (addModelStep2.ImagesForDeletion != null)
            {
                var imagesForDelete = addModelStep2.ImagesForDeletion.Split(',').ToList();

                foreach (var imgId in imagesForDelete)
                {
                    await DeleteFileFromFileSystemAsync(imgId);
                }
            }
        }
    }
}
