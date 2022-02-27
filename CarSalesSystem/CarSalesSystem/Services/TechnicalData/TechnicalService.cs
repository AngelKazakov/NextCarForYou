using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.TechnicalData
{
    public class TechnicalService : ITechnicalService
    {
        private readonly CarSalesDbContext data;

        public TechnicalService(CarSalesDbContext data)
         => this.data = data;

        public async Task<ICollection<VehicleEngineType>> GetEngineTypesAsync()
        {
            return await this.data.Engines.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<ICollection<TransmissionType>> GetTransmissionTypesAsync()
        {
            return await this.data.Transmissions.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<ICollection<VehicleEuroStandard>> GetEuroStandardsAsync()
        {
            return await this.data.EuroStandards
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<ICollection<ExtrasCategory>> GetExtrasCategoriesAsync()
        {
            return await this.data.Categories
                .Include(i => i.Extras)
                .ToListAsync();
        }
    }
}
