using System;

namespace CarSalesSystem.Data
{
    public static class DataConstants
    {
        public const int AdvertisementNameMinLength = 2;
        public const int AdvertisementNameMaxLength = 512;
        public const int AdvertisementDescriptionMinLength = 10;
        public const int AdvertisementDescriptionMaxLength = 6000;

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
        public const int VehiclePowerMinValue = 45;
        public const int VehiclePowerMaxValue = 2200;
        public const int VehicleMinYear = 1930;
        public const int VehicleMaxYear = 2100;

        public const string ErrorMessageRequiredField = "The field is required.";
        public const string ErrorMessagePriceField = "Please enter a valid price.";
        public const string ErrorMessageVehiclePowerField = "Please enter a valid value for horse power.";
        public const string ErrorMessageMileageField = "Please enter a valid value.";
        public const string ErrorMessageDescriptionField = "The field Description should be between 10 and 6000 symbols.";


        public static readonly string ImagesPath = Environment.CurrentDirectory + "\\Images";
    }
}
