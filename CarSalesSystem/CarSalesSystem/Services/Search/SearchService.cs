using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Enums;
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

        public ICollection<SearchResultModel> SearchVehicles(SearchAdvertisementModel model)
        {
            return BuildSearchResultModels(FindAdvertisements(model, false));
        }

        public ICollection<SearchResultModel> DetailedSearchVehicles(DetailedSearchAdvertisementModel detailedModel)
        {
            return BuildSearchResultModels(FindAdvertisements(detailedModel, true));
        }

        public ICollection<SearchResultModel> GetLastPublishedAdvertisements()
        {
            return BuildSearchResultModels(context.Advertisements
                .OrderByDescending(x => x.CreatedOnDate)
                .Take(6)
                .ToList());
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

        public ICollection<SearchResultModel> FindAdvertisementsByUserId(string userId)
        {
            ICollection<Data.Models.Advertisement> advertisements =
                context.Advertisements.Where(x => x.UserId == userId).ToList();

            return BuildSearchResultModels(advertisements);
        }

        public AveragePriceModel AveragePricesByGivenBrandAndModel(AveragePriceModel priceModel)
        {
            var searchModel = new DetailedSearchAdvertisementModel()
            {
                Model = priceModel.Model,
                Brand = priceModel.Brand,
                Year = priceModel.Year,
                TransmissionType = priceModel.TransmissionType,
                EngineType = priceModel.EngineType
            };

            ICollection<Data.Models.Advertisement> advertisements = FindAdvertisements(searchModel, true).ToList();

            var advertisementsAveragePrice = advertisements.DefaultIfEmpty().Average(p => p == null ? 0 : p.Price);

            priceModel.AveragePrice = Convert.ToDecimal(advertisementsAveragePrice.ToString("0.##"));

            priceModel.Advertisements = BuildSearchResultModels(advertisements.Take(3).ToList());

            return priceModel;
        }

        private List<Data.Models.Advertisement> FindAdvertisements(SearchAdvertisementModel searchModel, bool isDetailedSearch)
        {
            var query = context.Advertisements
                .Include(a => a.Vehicle.Model)
                .ThenInclude(x => x.Brand)
                .Include(a => a.Vehicle.EngineType)
                .Include(a => a.Vehicle.TransmissionType)
                .Include(a => a.City)
                .Include(a => a.City.Region)
                .Include(x => x.Vehicle.Color)
                .Include(x => x.Vehicle.Category)
                .Include(x => x.Vehicle.EuroStandard)
                .Include(x => x.AdvertisementExtras)
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


            if (isDetailedSearch)
            {
                DetailedSearchAdvertisementModel detailed = (DetailedSearchAdvertisementModel)searchModel;

                if (!string.IsNullOrEmpty(detailed.Brand2))
                {
                    query = query.Where(x => x.Vehicle.Model.BrandId == detailed.Brand2);
                }

                if (!string.IsNullOrEmpty(detailed.Model2))
                {
                    // query = query.Where(m => m.Vehicle.ModelId == searchModel.Model && m.Vehicle.Model.BrandId == searchModel.Brand);
                    query = query.Where(x => x.Vehicle.ModelId == detailed.Model);
                }

                if (detailed.MaximumPrice != 0 && detailed.MaximumPrice >= detailed.MinPrice)
                {
                    query = query.Where(x => x.Price <= detailed.MaximumPrice && x.Price >= detailed.MinPrice);
                }

                if (detailed.MaxMileage != 0 && detailed.MaxMileage > 0)
                {
                    query = query.Where(x => x.Vehicle.Mileage <= detailed.MaxMileage);
                }

                if (!string.IsNullOrEmpty(detailed.EuroStandard))
                {
                    query = query.Where(x => x.Vehicle.EuroStandardId == detailed.EuroStandard);
                }

                if (!string.IsNullOrEmpty(detailed.Color))
                {
                    query = query.Where(x => x.Vehicle.ColorId == detailed.Color);
                }

                if (!string.IsNullOrEmpty(detailed.Category))
                {
                    query = query.Where(x => x.Vehicle.CategoryId == detailed.Category);
                }

                if (detailed.Year > 0 && detailed.MaxYear is > 0 and <= VehicleMaxYear)
                {
                    query = query.Where(x => x.Vehicle.Year >= detailed.Year && x.Vehicle.Year <= detailed.MaxYear);
                }

                if (detailed.SelectedExtras.Count > 0)
                {
                    foreach (var selectedExtra in detailed.SelectedExtras)
                    {
                        // query = query.Where(p => detailed.SelectedExtras.Intersect(p.AdvertisementExtras.Select(a => a.ExtrasId)).Any());

                        query = query.Where(i => i.AdvertisementExtras.Select(e => e.ExtrasId).Contains(selectedExtra));
                    }
                }

                if (detailed.AdvertisementsWithImagesOnly)
                {
                    query = query.Where(x => x.VehicleImages.Count > 0);
                }

                if (detailed.AdvertisementOwnerSearchCriteria == AdvertisementOwnerSearchCriteria.Dealer)
                {
                    query = query.Where(x => x.CarDealershipId != null);
                }

                else if (detailed.AdvertisementOwnerSearchCriteria == AdvertisementOwnerSearchCriteria.Private)
                {
                    query = query.Where(x => x.CarDealershipId == null);
                }
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
    }
}
