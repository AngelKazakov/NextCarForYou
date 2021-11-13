using System;
using System.Collections.Generic;
using System.IO;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Advertisement;

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
                    BrandId = model.Brand,
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
    }
}
