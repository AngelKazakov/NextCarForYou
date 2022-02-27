using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services
{
    public interface IColorService
    {
       Task< ICollection<Color>> GetColorsAsync();
    }
}
