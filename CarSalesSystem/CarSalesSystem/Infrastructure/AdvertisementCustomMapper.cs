using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Advertisement;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Infrastructure
{
    public class AdvertisementCustomMapper
    {
        public static Advertisement Map(AdvertisementAddFormModel model, AdvertisementAddFormModelStep2 model2, string userId, List<string> extrasIds)
        {
            foreach (var extra in model.SelectedExtras)
            {
                extrasIds.Add(extra);
            }

            List<VehicleImage> vehicleImages = new List<VehicleImage>();

            foreach (var image in model2.Images)
            {
                if (image != null)
                {
                    var vehicleImage = new VehicleImage()
                    {
                        Name = image.FileName,
                        UniqueName = Path.GetRandomFileName(),
                    };

                    vehicleImages.Add(vehicleImage);
                }
            }

            Advertisement advertisement = new Advertisement()
            {
                Price = model.Price,
                Name = model2.Name,
                CityId = model.CityId,

                Vehicle = new Vehicle()
                {
                    Power = model.Power,
                    ModelId = model.Model,
                    ColorId = model.Color,
                    CategoryId = model.Category,
                    Year = model.Year,
                    TransmissionTypeId = model.TransmissionType,
                    EngineTypeId = model.EngineType,
                    EuroStandardId = model.EuroStandard,
                    Mileage = model.Mileage,
                },
                Description = model2.Description,
                CreatedOnDate = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow,
                UserId = userId,
                VehicleImages = vehicleImages
            };


            return advertisement;
        }

        public static AdvertisementViewModel Map(Advertisement advertisement)
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
                UserPhone = advertisement.User.PhoneNumber
            };

            if (advertisement.CarDealerShip != null)
            {
                advertisementViewModel.DealershipName = advertisement.CarDealerShip.Name;
                advertisementViewModel.DealershipPhone = advertisement.CarDealerShip.Phone;
            }

            string fullPath = ImagesPath + "/Advertisement" + advertisement.Id;

            if (Directory.Exists(fullPath))
            {
                DirectoryInfo directory = new DirectoryInfo(fullPath);

                FileInfo[] images = directory.GetFiles();

                if (images.Any())
                {
                    foreach (var image in images)
                    {
                        byte[] imageBytes = File.ReadAllBytes(fullPath + "/" + image.Name);
                        advertisementViewModel.Images.Add(imageBytes);
                    }
                }
            }

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
    }
}
