using System.Collections.Generic;
using System.IO;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using Microsoft.AspNetCore.Http;

namespace CarSalesSystem.Tests.TestDataFactory
{
    public static class TestDataFactory
    {
        public static Advertisement BuildAdvertisement(string id = "15")
        {
            return new Advertisement()
            {
                Id = id,
                Name = "BMW 330CD",
                City = BuildCity(),
                User = BuildUser(),
                Vehicle = BuilVehicle(),
                CarDealerShip = BuildCarDealerShip(),
                VehicleImages = new List<VehicleImage>() { BuildVehicleImage() },
                AdvertisementExtras = new List<AdvertisementExtra>() { BuildAdvertisementExtra() }
            };
        }

        public static City BuildCity()
        {
            return new City()
            {
                Id = "testCityId",
                Name = "fakeName",
                Region = BuildRegion()
            };
        }

        public static AdvertisementExtra BuildAdvertisementExtra()
            => new AdvertisementExtra() { AdvertisementId = "15", Extras = BuildExtras() };

        public static Extras BuildExtras()
            => new Extras()
            { Name = "testExtras", Category = BuildExtrasCategory() };

        public static ExtrasCategory BuildExtrasCategory()
            => new ExtrasCategory() { Name = "testExtrasCategoryName" };

        public static VehicleImage BuildVehicleImage()
            => new VehicleImage() { Id = "testImageId", Name = "testImageName", AdvertisementId = "testAdvertisementId", UniqueName = "testUniqueName", FullPath = "testPath" };
        public static Region BuildRegion()
        => new Region() { Id = "testRegionId", Name = "testName" };

        public static User BuildUser()
            => new User() { FirstName = "firstName", LastName = "lastName" };

        public static Vehicle BuilVehicle()
        {
            return new Vehicle()
            {
                Model = BuiModel(),
                Category = BuildVehicleCategory(),
                EngineType = BuildVehicleEngineType(),
                TransmissionType = BuildTransmissionType(),
                Color = BuildColor(),
                EuroStandard = BuildVehicleEuroStandard(),
            };
        }

        public static Model BuiModel()
            => new Model()
            { Id = "testModelId", Name = "brandName", Brand = BuildBrand() };

        public static Brand BuildBrand()
            => new Brand() { Id = "testBrandId", Name = "testBrandName" };

        public static VehicleCategory BuildVehicleCategory()
            => new VehicleCategory() { Id = "testVehicleCategoryId", Name = "testVehicleCategoryName" };

        public static VehicleEngineType BuildVehicleEngineType()
        => new VehicleEngineType() { Id = "testEngineId", Name = "testEngineName" };

        public static TransmissionType BuildTransmissionType()
            => new TransmissionType() { Id = "testTransmissionId", Name = "transmissionName" };

        public static Color BuildColor()
            => new Color() { Id = "testColorId", Name = "testColorName" };

        public static VehicleEuroStandard BuildVehicleEuroStandard()
            => new VehicleEuroStandard() { Id = "testEuroId", Name = "testEuroName" };

        public static CarDealerShip BuildCarDealerShip()
           => new CarDealerShip() { Address = "testAddress", Email = "testEmail", Name = "testName", Phone = "testPhone", User = BuildUser() };

        public static ICollection<IFormFile> BuildIFormFileImages()
        {
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile firstFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            IFormFile secondFile = new FormFile(stream, 0, stream.Length, "id_from_form2", fileName + "1");

            return new List<IFormFile>() { firstFile, secondFile };
        }


    }
}
