using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.Models
{
    public class ModelService : IModelService
    {
        private readonly CarSalesDbContext data;

        public ModelService(CarSalesDbContext data)
         => this.data = data;


        public ICollection<Model> GetAllModels(string brandId)
        {
            return this.data.Models
                .Where(x => x.BrandId == brandId)
                .OrderBy(x => x.Name)
                .ToList();
        }

        public async Task<ICollection<Model>> GetAllModelsAsync(string brandId)
        {
            return await this.data.Models
                .Where(x => x.BrandId == brandId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
