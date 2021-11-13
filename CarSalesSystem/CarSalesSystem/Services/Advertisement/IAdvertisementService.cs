using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CarSalesSystem.Services.Advertisement
{
    public interface IAdvertisementService
    {
        void Save(Data.Models.Advertisement advertisement, List<string> extrasIds, IFormFileCollection images);
    }
}
