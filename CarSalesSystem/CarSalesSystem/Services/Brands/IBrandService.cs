using System.Collections.Generic;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Brands
{
    public interface IBrandService
    {
        ICollection<Brand> GetAllBrands();
    }
}
