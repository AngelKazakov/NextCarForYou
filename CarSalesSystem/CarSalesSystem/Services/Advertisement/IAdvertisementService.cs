using System.Collections.Generic;
using CarSalesSystem.Models.Advertisement;
using Microsoft.AspNetCore.Http;

namespace CarSalesSystem.Services.Advertisement
{
    public interface IAdvertisementService
    {
        void Save(Data.Models.Advertisement advertisement, List<string> extrasIds, ICollection<IFormFile> images);

        string Edit(AdvertisementAddFormModel advertisement, AdvertisementAddFormModelStep2 advertisementStep2, string userId);

        void Delete(string Id, string UserId);

        Data.Models.Advertisement GetAdvertisementById(string advertisementId);

        public AdvertisementAddFormModel GetRecordData(string advertisementId, string userId);

        public AdvertisementAddFormModelStep2 GetRecordDataStep2(string advertisementId);
    }
}
