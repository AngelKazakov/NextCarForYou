using System.Collections.Generic;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Exceptions;
using CarSalesSystem.Models.CarDealership;

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

        public ICollection<Data.Models.CarDealerShip> GetAllCarDealershipsByUserId(string userId)
        {
            var dealerships = context.CarDealerShips
                .Where(x => x.UserId == userId)
                .ToList();

            return dealerships;
        }

        public ICollection<Data.Models.CarDealerShip> GetAllCarDealerships()
        {
            return context.CarDealerShips.ToList();
        }

        public CarDealershipAddFormModel GetCarDealership(string dealerId)
        {
            Data.Models.CarDealerShip dealerShip = context.CarDealerShips.FirstOrDefault(x => x.Id == dealerId);

            CarDealershipAddFormModel dealershipAddFormModel = new CarDealershipAddFormModel
            {
                Name = dealerShip.Name,
                Address = dealerShip.Address,
                Email = dealerShip.Email,
                Phone = dealerShip.Phone,
                Url = dealerShip.Url,
                Id = dealerId
            };

            return dealershipAddFormModel;
        }
        //TODO: Test
        public string UpdateCarDealership(CarDealershipAddFormModel model)
        {
            var currentDealership = context.CarDealerShips.FirstOrDefault(x => x.Id == model.Id);

            currentDealership.Name = model.Name;
            currentDealership.Address = model.Address;
            currentDealership.Email = model.Email;
            currentDealership.Phone = model.Phone;
            currentDealership.Url = model.Url;

            this.context.SaveChanges();

            return model.Id;
        }

        public ICollection<Data.Models.Advertisement> GetAdvertisementsByDealershipId(string dealerId)
        {
            return this.context.Advertisements.Where(x => x.CarDealershipId == dealerId).ToList();
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
