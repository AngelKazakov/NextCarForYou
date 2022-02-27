using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Brands
{
    public interface IBrandService
    {
        Task<ICollection<Brand>> GetAllBrandsAsync();
    }
}
