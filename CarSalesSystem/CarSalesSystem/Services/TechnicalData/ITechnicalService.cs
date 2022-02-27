using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.TechnicalData
{
    public interface ITechnicalService
    {
        Task<ICollection<VehicleEngineType>> GetEngineTypesAsync();

        Task<ICollection<TransmissionType>> GetTransmissionTypesAsync();

        Task<ICollection<VehicleEuroStandard>> GetEuroStandardsAsync();

        Task<ICollection<ExtrasCategory>> GetExtrasCategoriesAsync();
    }
}
