using System.Collections.Generic;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.TechnicalData
{
    public interface ITechnicalService
    {
        ICollection<VehicleEngineType> GetEngineTypes();
        ICollection<TransmissionType> GetTransmissionTypes();
        ICollection<VehicleEuroStandard> GetEuroStandards();
    }
}
