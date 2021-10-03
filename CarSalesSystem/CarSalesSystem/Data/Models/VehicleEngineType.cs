using System;

namespace CarSalesSystem.Data
{
    public class VehicleEngineType
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
    }
}
