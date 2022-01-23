using System.Collections.Generic;
using CarSalesSystem.Models.CarDealership;

namespace CarSalesSystem.Services.CarDealerShip
{
    public interface ICarDealerShipService
    {
        string CreateDealerShip(Data.Models.CarDealerShip carDealerShip);

        public ICollection<Data.Models.CarDealerShip> GetAllCarDealershipsByUserId(string userId);

        public ICollection<Data.Models.CarDealerShip> GetAllCarDealerships();

        public CarDealershipAddFormModel GetCarDealership(string dealerId);

        public string UpdateCarDealership(CarDealershipAddFormModel model);

        public ICollection<Data.Models.Advertisement> GetAdvertisementsByDealershipId(string dealerId);
    }
}
