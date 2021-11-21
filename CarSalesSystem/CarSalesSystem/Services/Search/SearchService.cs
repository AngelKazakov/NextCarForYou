using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Enums;
using CarSalesSystem.Models.Search;

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
            return BuildSearchResultModels(FindAdvertisements(model));
        }

        public List<SearchResultModel> GetLastPublishedAdvertisements()
        {
            return BuildSearchResultModels(context.Advertisements.OrderByDescending(x => x.CreatedOnDate).Take(6).ToList());
        }

        private List<Data.Models.Advertisement> FindAdvertisements(SearchAdvertisementModel searchModel)
        {
            var query = context.Advertisements.ToList();

            if (!string.IsNullOrWhiteSpace(searchModel.Brand))
            {
                query.Where(b => b.Vehicle.Model.Brand.Name == searchModel.Brand);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Model))
            {
                query.Where(m => m.Vehicle.Model.Name == searchModel.Model && m.Vehicle.Model.Brand.Name == searchModel.Brand);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.EngineType))
            {
                query.Where(e => e.Vehicle.EngineType.Name == searchModel.EngineType);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.TransmissionType))
            {
                query.Where(t => t.Vehicle.TransmissionType.Name == searchModel.TransmissionType);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.City))
            {
                query.Where(c => c.City.Name == searchModel.City && c.City.Region.Name == searchModel.Region);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Region))
            {
                query.Where(r => r.City.Region.Name == searchModel.Region);
            }

            if (searchModel.Year >= 1930 && searchModel.Year <= DateTime.Now.Year)
            {
                query.Where(y => y.Vehicle.Year >= searchModel.Year);
            }

            if (searchModel.MaximumPrice > 0)
            {
                query.Where(p => p.Price <= searchModel.MaximumPrice);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.OrderBy))
            {
                OrderByValues orderBy = (OrderByValues)Enum.Parse(typeof(OrderByValues), searchModel.OrderBy);

                if (orderBy.Equals(OrderByValues.Brand_Model_Price))
                {
                    query
                        .OrderBy(x => x.Vehicle.Model.Brand.Name)
                        .ThenBy(x => x.Vehicle.Model.Name)
                        .ThenBy(x => x.Price);
                }
                else if (orderBy.Equals(OrderByValues.Price))
                {
                    query.OrderBy(x => x.Price);
                }
                else if (orderBy.Equals(OrderByValues.Year))
                {
                    query.OrderByDescending(x => x.Vehicle.Year);
                }
                else if (orderBy.Equals(OrderByValues.Newest))
                {
                    query.OrderByDescending(x => x.CreatedOnDate);
                }
            }

            return query;
        }

        private List<SearchResultModel> BuildSearchResultModels(List<Data.Models.Advertisement> advertisements)
        {
            var results = new List<SearchResultModel>();

            foreach (var advertisement in advertisements)
            {
                Data.Models.Advertisement loadedAdvertisement = context.Advertisements.FirstOrDefault(a => a.Id == advertisement.Id);
                context.Entry(loadedAdvertisement).Reference(a => a.City).Load();
                context.Entry(loadedAdvertisement).Reference(a => a.Vehicle).Load();


                var resultModel = new SearchResultModel()
                {
                    AdvertisementId = advertisement.Id,
                    City = advertisement.City.Name,
                    Mileage = advertisement.Vehicle.Mileage,
                    Name = advertisement.Name,
                    Price = advertisement.Price,
                    Region = context.Regions.FirstOrDefault(r => r.Id == advertisement.City.RegionId).Name,
                    Year = advertisement.Vehicle.Year,
                    CreatedOn = advertisement.CreatedOnDate.ToString("HH:mm, dd.MM.yyyy")
                };

                string fullPath = ImagesPath + "/Advertisement" + advertisement.Id;

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
