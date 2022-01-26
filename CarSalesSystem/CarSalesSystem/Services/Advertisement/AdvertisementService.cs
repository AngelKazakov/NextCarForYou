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

        public string Save(Data.Models.Advertisement advertisement, List<string> extrasIds, ICollection<IFormFile> images)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
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

                SaveImages(images, advertisementId, advertisement.VehicleImages).GetAwaiter().GetResult();

                context.SaveChanges();

                transaction.Commit();

                return advertisementId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Error occurred.");
                throw ex;
            }
        }

        public string Edit(AdvertisementAddFormModel advertisementStep1, AdvertisementAddFormModelStep2 advertisementStep2, string userId)
        {
            var currentAdvertisement = context.Advertisements
                .Include(x => x.Vehicle)
                .Include(x => x.VehicleImages)
                .Include(x => x.AdvertisementExtras)
                .FirstOrDefault(x => x.Id == advertisementStep1.Id);

            if (currentAdvertisement == null)
            {
                throw new Exception("Record not found.");
            }

            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                UpdateAdvertisement(currentAdvertisement, advertisementStep1, advertisementStep2);
                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                throw ex;
            }

            return currentAdvertisement.Id;
        }

        public void Delete(string Id, string UserId)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                string advertisementUserId = context.Advertisements
                    .Where(x => x.Id == Id)
                    .Select(x => x.UserId)
                    .FirstOrDefault();

                if (advertisementUserId != UserId)
                {
                    throw new Exception("You do not have permission to delete this advertisement.");
                }

                Data.Models.Advertisement advertisement = context.Advertisements.Include(x => x.Vehicle).First(adv => adv.Id == Id);
                context.Advertisements.Remove(advertisement);
                context.Vehicles.Remove(advertisement.Vehicle);
                context.SaveChanges();

                var path = "Advertisement" + Id;

                var dir = new DirectoryInfo(ImagesPath + "\\" + path);
                dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                dir.Delete(true);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public Data.Models.Advertisement GetAdvertisementById(string advertisementId)
        {
            Data.Models.Advertisement advertisement =
                context.Advertisements
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
                    .FirstOrDefault(x => x.Id == advertisementId);


            return advertisement;
        }

        public AdvertisementAddFormModel GetRecordData(string advertisementId, string userId)
        {
            var model = GetAdvertisementById(advertisementId);

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
                Colors = mapper.Map<ICollection<Color>, ICollection<ColorFormModel>>(colorService.GetColors()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(technicalService.GetEngineTypes()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<EuroStandardFormModel>>(technicalService.GetEuroStandards()),
                Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(regionService.GetAllRegions()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<CategoryFormModel>>(categoryService.GetVehicleCategories()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(technicalService.GetTransmissionTypes()),
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(brandService.GetAllBrands()),
                Models = mapper.Map<ICollection<Model>, ICollection<ModelFormModel>>(modelService.GetAllModels(model.Vehicle.Model.BrandId)),
                Cities = mapper.Map<ICollection<City>, ICollection<CityFormModel>>(regionService.GetAllCities(model.City.RegionId)),
                Extras = mapper.Map<ICollection<ExtrasCategory>, ICollection<ExtrasCategoryFormModel>>(technicalService.GetExtrasCategories()),
                Dealerships = mapper.Map<ICollection<Data.Models.CarDealerShip>, ICollection<CarDealershipViewModel>>(dealerShipService.GetAllCarDealershipsByUserId(userId))
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

        public AdvertisementAddFormModelStep2 GetRecordDataStep2(string advertisementId)
        {
            Data.Models.Advertisement model = GetAdvertisementById(advertisementId);


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
                    idsToImages.Add(vehicleImage.Id, File.ReadAllBytes(vehicleImage.FullPath));
                }
            }

            advertisementAddFormModel2.ImagesForDisplay = idsToImages;

            return advertisementAddFormModel2;
        }

        private void DeleteFileFromFileSystem(string id)
        {
            var file = context.Images.FirstOrDefault(x => x.Id == id);

            if (file == null) return;

            if (File.Exists(file.FullPath))
            {
                File.Delete(file.FullPath);
            }
            context.Images.Remove(file);
            context.SaveChanges();
        }

        private async Task SaveImages(ICollection<IFormFile> files, string advertisementId, ICollection<VehicleImage> vehicleImages)
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
                    var fileName = images.GetValueOrDefault(formFile.FileName).UniqueName + Path.GetExtension(formFile.FileName).ToLower();
                    string fullPath = Path.Combine(subDirectoryInfo.FullName, fileName);
                    await using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    await formFile.CopyToAsync(stream);

                    images.GetValueOrDefault(formFile.FileName).FullPath = fullPath;
                }
            }
        }

        private void UpdateAdvertisement(Data.Models.Advertisement advertisement,
            AdvertisementAddFormModel addModelStep1, AdvertisementAddFormModelStep2 addModelStep2)
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

            SaveImages(addModelStep2.Images, addModelStep1.Id, vehicleImages).GetAwaiter().GetResult();

            context.Images.AddRange(vehicleImages);

            if (addModelStep2.ImagesForDeletion != null)
            {
                var imagesForDelete = addModelStep2.ImagesForDeletion.Split(',').ToList();

                foreach (var imgId in imagesForDelete)
                {
                    DeleteFileFromFileSystem(imgId);
                }
            }
        }
    }
}
