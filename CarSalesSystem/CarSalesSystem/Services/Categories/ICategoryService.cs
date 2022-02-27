using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Data;

namespace CarSalesSystem.Services.Categories
{
    public interface ICategoryService
    {
       Task< ICollection<VehicleCategory>> GetVehicleCategoriesAsync();
    }
}
