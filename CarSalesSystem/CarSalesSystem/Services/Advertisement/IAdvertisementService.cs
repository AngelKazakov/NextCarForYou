using System.Collections.Generic;
using CarSalesSystem.Models.Advertisement;
using Microsoft.AspNetCore.Http;

namespace CarSalesSystem.Services.Advertisement
{
    public interface IAdvertisementService
    {
        void Save(Data.Models.Advertisement advertisement, List<string> extrasIds, IFormFileCollection images);

        AdvertisementViewModel GetAdvertisementById(string advertisementId);
    }
}
