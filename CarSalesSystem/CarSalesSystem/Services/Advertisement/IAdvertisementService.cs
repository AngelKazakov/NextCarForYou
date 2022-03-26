using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Models.Advertisement;
using Microsoft.AspNetCore.Http;

namespace CarSalesSystem.Services.Advertisement
{
    public interface IAdvertisementService
    {
        public Task<AdvertisementAddFormModel> GetAdvertisementAddFormModel(string userId);

        public Task<string> SaveAsync(Data.Models.Advertisement advertisement, List<string> extrasIds, ICollection<IFormFile> images);

        public Task<string> EditAsync(AdvertisementAddFormModel advertisement, AdvertisementAddFormModelStep2 advertisementStep2, string userId);

        public Task DeleteAsync(string Id, string UserId);

        public Task<Data.Models.Advertisement> GetAdvertisementByIdAsync(string advertisementId);

        public Task<AdvertisementAddFormModel> GetRecordDataAsync(string advertisementId, string userId);

        public Task<AdvertisementAddFormModelStep2> GetRecordDataStep2Async(string advertisementId);
    }
}
