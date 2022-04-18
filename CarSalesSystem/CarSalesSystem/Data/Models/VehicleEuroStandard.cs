using System;

namespace CarSalesSystem.Data.Models
{
    public class VehicleEuroStandard
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
    }
}
