using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Advertisement;
using Microsoft.AspNetCore.Http;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Infrastructure
{
    public static class AdvertisementCustomMapper
    {
        public static ICollection<VehicleImage> CreateVehicleImages(ICollection<IFormFile> images)
        {
            List<VehicleImage> vehicleImages = new List<VehicleImage>();

            foreach (var image in images)
            {
                if (image != null)
                {
                    string randomFileName = Path.GetRandomFileName();

                    var vehicleImage = new VehicleImage()
                    {
                        Name = image.FileName,
                        UniqueName = randomFileName,
                    };

                    vehicleImages.Add(vehicleImage);
                }
            }

            return vehicleImages;
        }

        public static Advertisement Map(AdvertisementAddFormModel model, AdvertisementAddFormModelStep2 model2, string userId, List<string> extrasIds)
        {
            foreach (var extra in model.SelectedExtras)
            {
                extrasIds.Add(extra);
            }

            ICollection<VehicleImage> vehicleImages = CreateVehicleImages(model2.Images);

            Advertisement advertisement = new Advertisement()
            {
                Price = model.Price,
                Name = model2.Name,
                CityId = model.CityFormModel.Id,
                CarDealershipId = model.DealershipFormModel.Id,

                Vehicle = new Vehicle()
                {
                    Power = model.Power,
                    ModelId = model.ModelFormModel.Id,
                    ColorId = model.ColorFormModel.Id,
                    CategoryId = model.CategoryFormModel.Id,
                    Year = model.Year,
                    TransmissionTypeId = model.TransmissionTypeFormModel.Id,
                    EngineTypeId = model.EngineTypeFormModel.Id,
                    EuroStandardId = model.EuroStandardFormModel.Id,
                    Mileage = model.Mileage,
                    Month = model.Month
                },
                Description = model2.Description,
                CreatedOnDate = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow,
                UserId = userId,
                VehicleImages = vehicleImages
            };

            return advertisement;
        }

        public static AdvertisementViewModel Map(Advertisement advertisement, string userId)
        {
            AdvertisementViewModel advertisementViewModel = new AdvertisementViewModel()
            {
                Name = advertisement.Name,
                Price = advertisement.Price,
                Power = advertisement.Vehicle.Power,
                Year = advertisement.Vehicle.Year,
                RegionName = advertisement.City.Region.Name,
                AdvertisementId = advertisement.Id,
                CategoryId = advertisement.Vehicle.CategoryId,
                CategoryName = advertisement.Vehicle.Category.Name,
                CityId = advertisement.CityId,
                CityName = advertisement.City.Name,
                ColorId = advertisement.Vehicle.ColorId,
                ColorName = advertisement.Vehicle.Color.Name,
                CreatedOn = advertisement.CreatedOnDate.ToString("HH:mm dd MMMM yyyy", CultureInfo.InvariantCulture),
                EngineTypeId = advertisement.Vehicle.EngineTypeId,
                EngineTypeName = advertisement.Vehicle.EngineType.Name,
                EuroStandardId = advertisement.Vehicle.EuroStandardId,
                EuroStandardName = advertisement.Vehicle.EuroStandard.Name,
                LastModifiedOn = advertisement.LastModifiedOn.ToString("HH:mm dd MMMM yyyy", CultureInfo.InvariantCulture),
                Mileage = advertisement.Vehicle.Mileage,
                ModelId = advertisement.Vehicle.ModelId,
                ModelName = advertisement.Vehicle.Model.Name,
                RegionId = advertisement.City.RegionId,
                TransmissionTypeId = advertisement.Vehicle.TransmissionTypeId,
                TransmissionTypeName = advertisement.Vehicle.TransmissionType.Name,
                VehicleId = advertisement.VehicleId,
                Description = advertisement.Description,
                UserPhone = advertisement.User.PhoneNumber,
                IsAllowedToEdit = !string.IsNullOrEmpty(userId) && userId == advertisement.UserId,
                Month = GetMonthNameByNumber(advertisement.Vehicle.Month)
            };

            if (advertisement.CarDealerShip != null)
            {
                advertisementViewModel.DealershipName = advertisement.CarDealerShip.Name;
                advertisementViewModel.DealershipPhone = advertisement.CarDealerShip.Phone;
            }

            advertisementViewModel.Images.AddRange(ReadImagesAsByteArray(advertisement.Id));

            Dictionary<string, List<string>> advertisementExtras = new Dictionary<string, List<string>>();

            foreach (var advertisementExtra in advertisement.AdvertisementExtras)
            {
                string key = advertisementExtra.Extras.Category.Name;

                if (advertisementExtras.ContainsKey(key))
                {
                    List<string> extrasNames = advertisementExtras.GetValueOrDefault(key);
                    extrasNames.Add(advertisementExtra.Extras.Name);
                }
                else
                {
                    advertisementExtras.Add(key, new List<string> { advertisementExtra.Extras.Name });
                }
            }

            advertisementViewModel.Extras = advertisementExtras;

            return advertisementViewModel;
        }

        public static List<byte[]> ReadImagesAsByteArray(string advertisementId)
        {
            List<byte[]> readImages = new List<byte[]>();

            string fullPath = ImagesPath + "/Advertisement" + advertisementId;

            if (Directory.Exists(fullPath))
            {
                DirectoryInfo directory = new DirectoryInfo(fullPath);

                FileInfo[] images = directory.GetFiles();

                if (images.Any())
                {
                    foreach (var image in images)
                    {
                        byte[] imageBytes = File.ReadAllBytes(fullPath + "/" + image.Name);
                        readImages.Add(imageBytes);
                    }
                }
            }
            return readImages;
        }

        private static string GetMonthNameByNumber(int numberOfMonth)
        {
            switch (numberOfMonth)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";

                default:
                    return "January";
            }

        }
    }
}
