using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.TechnicalData
{
    public class TechnicalService : ITechnicalService
    {
        private readonly CarSalesDbContext data;

        public TechnicalService(CarSalesDbContext data)
         => this.data = data;

        public ICollection<VehicleEngineType> GetEngineTypes()
        {
            return this.data.Engines.ToList();
        }

        public ICollection<TransmissionType> GetTransmissionTypes()
        {
            return this.data.Transmissions.ToList();
        }

        public ICollection<VehicleEuroStandard> GetEuroStandards()
        {
            return this.data.EuroStandards.OrderBy(x => x.Name).ToList();
        }
    }
}
