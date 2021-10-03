using System.Collections.Generic;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services
{
    public interface IColorService
    {
        ICollection<Color> GetColors();
    }
}
