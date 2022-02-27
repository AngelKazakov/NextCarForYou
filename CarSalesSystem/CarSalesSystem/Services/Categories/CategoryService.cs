using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly CarSalesDbContext data;

        public CategoryService(CarSalesDbContext data)
        => this.data = data;

        public async Task<ICollection<VehicleCategory>> GetVehicleCategoriesAsync()
        {
            return await this.data.VehicleCategories.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
