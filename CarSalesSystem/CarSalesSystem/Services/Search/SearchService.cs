using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Enums;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Search;
using Microsoft.EntityFrameworkCore;
using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Services.Search
{
    public class SearchService : ISearchService
    {
        //TODO: handle errors with try-catch

        private readonly CarSalesDbContext context;

        public SearchService(CarSalesDbContext context)
         => this.context = context;

        public List<SearchResultModel> SearchVehicles(SearchAdvertisementModel model)
        {
            return BuildSearchResultModels(FindAdvertisements(model)).ToList();
        }

        public List<SearchResultModel> GetLastPublishedAdvertisements()
        {
            //TODO: Take 6 newest advertisements instead of 2
            return BuildSearchResultModels(context.Advertisements.OrderByDescending(x => x.CreatedOnDate).Take(2).ToList()).ToList();
        }

        private List<Data.Models.Advertisement> FindAdvertisements(SearchAdvertisementModel searchModel)
        {
            var query = context.Advertisements
                .Include(a => a.Vehicle.Model)
                .Include(a => a.Vehicle.EngineType)
                .Include(a => a.Vehicle.TransmissionType)
                .Include(a => a.City)
                .Include(a => a.City.Region)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchModel.Brand))
            {
                query = query.Where(b => b.Vehicle.Model.BrandId == searchModel.Brand);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Model))
            {
                query = query.Where(m => m.Vehicle.ModelId == searchModel.Model && m.Vehicle.Model.BrandId == searchModel.Brand);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.EngineType))
            {
                query = query.Where(e => e.Vehicle.EngineType.Id == searchModel.EngineType);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.TransmissionType))
            {
                query = query.Where(t => t.Vehicle.TransmissionType.Id == searchModel.TransmissionType);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.City))
            {
                query = query.Where(c => c.City.Id == searchModel.City && c.City.RegionId == searchModel.Region);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Region))
            {
                query = query.Where(r => r.City.RegionId == searchModel.Region);
            }

            if (searchModel.Year >= 1930 && searchModel.Year <= DateTime.Now.Year)
            {
                query = query.Where(y => y.Vehicle.Year >= searchModel.Year);
            }

            if (searchModel.MaximumPrice > 0)
            {
                query = query.Where(p => p.Price <= searchModel.MaximumPrice);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.OrderBy))
            {
                OrderByValues orderBy = (OrderByValues)Enum.Parse(typeof(OrderByValues), searchModel.OrderBy);

                if (orderBy.Equals(OrderByValues.Brand_Model_Price))
                {
                    query = query
                        .OrderBy(x => x.Vehicle.Model.Brand.Name)
                        .ThenBy(x => x.Vehicle.Model.Name)
                        .ThenBy(x => x.Price);
                }
                else if (orderBy.Equals(OrderByValues.Price))
                {
                    query = query.OrderBy(x => x.Price);
                }
                else if (orderBy.Equals(OrderByValues.Year))
                {
                    query = query.OrderByDescending(x => x.Vehicle.Year);
                }
                else if (orderBy.Equals(OrderByValues.Newest))
                {
                    query = query.OrderByDescending(x => x.CreatedOnDate);
                }
            }


            return query.ToList();
        }

        public ICollection<SearchResultModel> BuildSearchResultModels(ICollection<Data.Models.Advertisement> advertisements)
        {
            var results = new List<SearchResultModel>();

            foreach (var advertisement in advertisements)
            {
                Data.Models.Advertisement loadedAdvertisement = context.Advertisements
                    .Include(c => c.City)
                    .Include(v => v.Vehicle)
                    .FirstOrDefault(a => a.Id == advertisement.Id);

                var resultModel = new SearchResultModel()
                {
                    AdvertisementId = loadedAdvertisement.Id,
                    City = loadedAdvertisement.City.Name,
                    Mileage = loadedAdvertisement.Vehicle.Mileage,
                    Name = loadedAdvertisement.Name,
                    Price = loadedAdvertisement.Price,
                    Region = context.Regions.FirstOrDefault(r => r.Id == loadedAdvertisement.City.RegionId).Name,
                    Year = loadedAdvertisement.Vehicle.Year,
                    CreatedOn = loadedAdvertisement.CreatedOnDate.ToString("HH:mm, dd.MM.yyyy")
                };

                string fullPath = ImagesPath + "/Advertisement" + loadedAdvertisement.Id;

                if (Directory.Exists(fullPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(fullPath);

                    FileInfo[] images = directory.GetFiles();

                    if (images.Any())
                    {
                        resultModel.Image = File.ReadAllBytes(fullPath + "/" + images[0].Name);
                    }
                }

                results.Add(resultModel);
            }

            return results;
        }
    }
}
