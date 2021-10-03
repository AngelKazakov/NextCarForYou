using System.Collections.Generic;
using CarSalesSystem.Data;

namespace CarSalesSystem.Services.Categories
{
    public interface ICategoryService
    {
        ICollection<VehicleCategory> GetVehicleCategories();
    }
}
