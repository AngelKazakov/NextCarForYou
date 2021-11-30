using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Exceptions;

namespace CarSalesSystem.Services.CarDealerShip
{
    public class CarDealerShipService : ICarDealerShipService
    {
        private readonly CarSalesDbContext context;

        public CarDealerShipService(CarSalesDbContext context)
        => this.context = context;


        public string CreateDealerShip(Data.Models.CarDealerShip carDealerShip)
        {
            CheckForExistingCarDealerShip(carDealerShip);

            context.CarDealerShips.Add(carDealerShip);
            context.SaveChanges();

            return carDealerShip.Id;

        }

        private void CheckForExistingCarDealerShip(Data.Models.CarDealerShip carDealerShip)
        {
            if (context.CarDealerShips.Any(x => x.Name == carDealerShip.Name))
            {
                throw new DuplicateCarDealerShipException($"Car dealership with name: {carDealerShip.Name} already exists.");
            }

            if (context.CarDealerShips.Any(x => x.Email == carDealerShip.Email))
            {
                throw new DuplicateCarDealerShipException($"Car dealership with email: {carDealerShip.Email} already exists.");
            }

            if (context.CarDealerShips.Any(x => x.Url == carDealerShip.Url))
            {
                throw new DuplicateCarDealerShipException($"Car dealership with website: {carDealerShip.Url} already exists.");
            }
        }
    }
}
