using System;
using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly CarSalesDbContext data;

        public BrandService(CarSalesDbContext data)
         => this.data = data;


        public ICollection<Brand> GetAllBrands()
        {
            return this.data.Brands
                .OrderBy(x => x.Name)
                .ToList();

        }
    }
}
