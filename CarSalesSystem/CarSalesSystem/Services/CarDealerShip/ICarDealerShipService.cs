using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Models.CarDealership;

namespace CarSalesSystem.Services.CarDealerShip
{
    public interface ICarDealerShipService
    {
        public Task<string> CreateDealerShipAsync(Data.Models.CarDealerShip carDealerShip);

        public Task<ICollection<Data.Models.CarDealerShip>> GetAllCarDealershipsByUserIdAsync(string userId);

        public Task<ICollection<Data.Models.CarDealerShip>> GetAllCarDealershipsAsync();

        public Task<CarDealershipAddFormModel> GetCarDealershipAsync(string dealerId);

        public Task<string> UpdateCarDealershipAsync(CarDealershipAddFormModel model);

        public Task<ICollection<Data.Models.Advertisement>> GetAdvertisementsByDealershipIdAsync(string dealerId);
    }
}
