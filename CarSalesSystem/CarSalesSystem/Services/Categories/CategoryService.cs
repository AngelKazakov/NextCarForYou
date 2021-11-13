using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Data;

namespace CarSalesSystem.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly CarSalesDbContext data;

        public CategoryService(CarSalesDbContext data)
        => this.data = data;


        public ICollection<VehicleCategory> GetVehicleCategories()
        {
            return this.data.VehicleCategories.ToList();
        }
    }
}
