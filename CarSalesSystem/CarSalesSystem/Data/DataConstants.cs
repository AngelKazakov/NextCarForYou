using System;

namespace CarSalesSystem.Data
{
    public static class DataConstants
    {
        public const int AdvertisementNameMaxLength = 512;
        public const int AdvertisementDescriptionMinLength = 10;
        public const int AdvertisementDescriptionMaxLength = 10000;

        public const int CarDealerShipNameMaxLength = 128;
        public const int CarDealerAddressMaxLength = 512;

        public const int CityNameMaxLength = 128;

        public const int ExtrasNameMaxLength = 128;

        public const int CategoryNameMaxLength = 128;

        public const int RegionNameMaxLength = 256;

        public const int UserFirstNameMaxLength = 64;
        public const int UserLastNameMaxLength = 64;

        public const int VehicleBrandNameMaxLength = 64;
        public const int VehicleModelNameMaxLength = 64;
        public const int VehiclePowerMaxValue = 2200;

        public static readonly string ImagesPath = Environment.CurrentDirectory + "\\Images";
    }
}
