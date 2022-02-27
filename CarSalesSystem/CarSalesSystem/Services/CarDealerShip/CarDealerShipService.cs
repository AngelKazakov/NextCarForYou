using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSalesSystem.Data;
using CarSalesSystem.Exceptions;
using CarSalesSystem.Infrastructure;
using CarSalesSystem.Models.CarDealership;
using Microsoft.EntityFrameworkCore;

namespace CarSalesSystem.Services.CarDealerShip
{
    public class CarDealerShipService : ICarDealerShipService
    {
        private readonly CarSalesDbContext context;

        public CarDealerShipService(CarSalesDbContext context)
        => this.context = context;

        public async Task<string> CreateDealerShipAsync(Data.Models.CarDealerShip carDealerShip)
        {
            CheckForExistingCarDealerShip(carDealerShip);

            context.CarDealerShips.Add(carDealerShip);
            await context.SaveChangesAsync();

            return carDealerShip.Id;
        }

        public async Task<ICollection<Data.Models.CarDealerShip>> GetAllCarDealershipsByUserIdAsync(string userId)
        {
            var dealerships = await context.CarDealerShips
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return dealerships;
        }

        public async Task<ICollection<Data.Models.CarDealerShip>> GetAllCarDealershipsAsync()
        {
            return await context.CarDealerShips.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<CarDealershipAddFormModel> GetCarDealershipAsync(string dealerId)
        {
            Data.Models.CarDealerShip dealerShip = await context.CarDealerShips.FirstOrDefaultAsync(x => x.Id == dealerId);

            CarDealershipAddFormModel dealershipAddFormModel = new CarDealershipAddFormModel
            {
                Name = dealerShip.Name,
                Address = dealerShip.Address,
                Email = dealerShip.Email,
                Phone = dealerShip.Phone,
                Url = dealerShip.Url,
                ImageForDisplay = dealerShip.ImageLogo,
                Id = dealerId
            };

            return dealershipAddFormModel;
        }

        public async Task<string> UpdateCarDealershipAsync(CarDealershipAddFormModel model)
        {
            var currentDealership = await context.CarDealerShips.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (currentDealership == null) throw new ArgumentNullException(nameof(currentDealership));

            currentDealership.Name = model.Name;
            currentDealership.Address = model.Address;
            currentDealership.Email = model.Email;
            currentDealership.Phone = model.Phone;
            currentDealership.Url = model.Url;

            if (model.Image != null)
            {
                currentDealership.ImageLogo = FormFileToByteArrayConverter.Convert(model.Image);
            }
            else if (model.ImageDeleted)
            {
                currentDealership.ImageLogo = null;
            }

            await this.context.SaveChangesAsync();

            return model.Id;
        }

        public async Task<ICollection<Data.Models.Advertisement>> GetAdvertisementsByDealershipIdAsync(string dealerId)
        {
            return await this.context.Advertisements.Where(x => x.CarDealershipId == dealerId).ToListAsync();
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

            if (!string.IsNullOrEmpty(carDealerShip.Url) && context.CarDealerShips.Any(x => x.Url == carDealerShip.Url))
            {
                throw new DuplicateCarDealerShipException($"Car dealership with website: {carDealerShip.Url} already exists.");
            }
        }
    }
}
