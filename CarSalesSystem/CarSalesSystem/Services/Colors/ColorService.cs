using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.Colors
{
    public class ColorService : IColorService
    {
        private readonly CarSalesDbContext data;

        public ColorService(CarSalesDbContext data)
          => this.data = data;

        public async Task<ICollection<Color>> GetColorsAsync()
        {
            return await this.data.Colors.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
