using System.Collections.Generic;
using CarSalesSystem.Models.Advertisement;
using Microsoft.AspNetCore.Http;

namespace CarSalesSystem.Services.Advertisement
{
    public interface IAdvertisementService
    {
        void Save(Data.Models.Advertisement advertisement, List<string> extrasIds, IFormFileCollection images);

        void Edit(AdvertisementViewModel advertisement, string advertisementId, string userId);

        void Delete(string Id, string UserId);

        AdvertisementViewModel GetAdvertisementById(string advertisementId);
    }
}
