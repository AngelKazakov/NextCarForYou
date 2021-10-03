using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Colors
{
    public class ColorService : IColorService
    {
        private readonly CarSalesDbContext data;

        public ColorService(CarSalesDbContext data)
          => this.data = data;

        public ICollection<Color> GetColors()
        {
            return this.data.Colors.ToList();
        }
    }
}
