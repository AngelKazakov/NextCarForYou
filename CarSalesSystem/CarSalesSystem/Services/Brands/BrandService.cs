using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly CarSalesDbContext data;

        public BrandService(CarSalesDbContext data)
         => this.data = data;

        public async Task<ICollection<Brand>> GetAllBrandsAsync()
        {
            return await this.data.Brands
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
