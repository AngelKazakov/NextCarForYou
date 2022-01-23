using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

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
    }
}
