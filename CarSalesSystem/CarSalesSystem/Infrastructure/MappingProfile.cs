using AutoMapper;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Transmission;

namespace CarSalesSystem.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Brand, AddBrandFormModel>();
            this.CreateMap<VehicleCategory, AddCategoryFormModel>();
            this.CreateMap<Color, AddColorFormModel>();
            this.CreateMap<Region, AddRegionFormModel>();
            this.CreateMap<TransmissionType, AddTransmissionFormModel>();
            this.CreateMap<VehicleEngineType, AddEngineFormModel>();
            this.CreateMap<VehicleEuroStandard, AddEuroStandardFormModel>();
        }
    }
}
