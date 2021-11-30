using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.CarDealership;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.ExtrasCategory;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;

namespace CarSalesSystem.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Brand, BrandFormModel>();
            this.CreateMap<VehicleCategory, CategoryFormModel>();
            this.CreateMap<Color, ColorFormModel>();
            this.CreateMap<Region, RegionFormModel>();
            this.CreateMap<TransmissionType, TransmissionFormModel>();
            this.CreateMap<VehicleEngineType, EngineFormModel>();
            this.CreateMap<VehicleEuroStandard, EuroStandardFormModel>();
            this.CreateMap<ExtrasCategory, ExtrasCategoryFormModel>();
            this.CreateMap<Extras, AddExtrasFormModel>();
            this.CreateMap<CarDealershipAddFormModel, CarDealerShip>();
        }
    }
}
