using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CarSalesSystem.Data;
using CarSalesSystem.Data.Enums;
using CarSalesSystem.Data.Models;
using CarSalesSystem.Models.Brand;
using CarSalesSystem.Models.Category;
using CarSalesSystem.Models.Color;
using CarSalesSystem.Models.Engine;
using CarSalesSystem.Models.EuroStandard;
using CarSalesSystem.Models.ExtrasCategory;
using CarSalesSystem.Models.Home;
using CarSalesSystem.Models.Model;
using CarSalesSystem.Models.Region;
using CarSalesSystem.Models.Search;
using CarSalesSystem.Models.Transmission;
using CarSalesSystem.Services.Brands;
using CarSalesSystem.Services.Categories;
using CarSalesSystem.Services.Models;
using CarSalesSystem.Services.Regions;
using CarSalesSystem.Services.TechnicalData;

using static CarSalesSystem.Data.DataConstants;

namespace CarSalesSystem.Services.Search
{
    public class SearchService : ISearchService
    {
        //TODO: handle errors with try-catch

        private readonly CarSalesDbContext context;
        private readonly IMapper mapper;
        private readonly IModelService modelService;
        private readonly IBrandService brandService;
        private readonly ICategoryService categoryService;
        private readonly IColorService colorService;
        private readonly IRegionService regionService;
        private readonly ITechnicalService technicalService;

        public SearchService(CarSalesDbContext context, IBrandService brandService, IMapper mapper,
             IModelService modelService, ICategoryService categoryService,
            IColorService colorService, IRegionService regionService, ITechnicalService technicalService)
        {
            this.context = context;
            this.brandService = brandService;
            this.mapper = mapper;
            this.modelService = modelService;
            this.categoryService = categoryService;
            this.colorService = colorService;
            this.regionService = regionService;
            this.technicalService = technicalService;
        }

        public async Task<ICollection<SearchResultModel>> SearchVehiclesAsync(SearchAdvertisementModel model, string userId)
        {
            return await BuildSearchResultModelsAsync(await FindAdvertisementsAsync(model, false), userId);
        }

        public async Task<ICollection<SearchResultModel>> DetailedSearchVehiclesAsync(DetailedSearchAdvertisementModel detailedModel, string userId)
        {
            return await BuildSearchResultModelsAsync(await FindAdvertisementsAsync(detailedModel, true), userId);
        }

        public async Task<ICollection<SearchResultModel>> GetLastPublishedAdvertisementsAsync(string userId)
        {
            var advertisements = await context.Advertisements.OrderByDescending(x => x.CreatedOnDate).Take(6).ToListAsync();

            return await BuildSearchResultModelsAsync(advertisements, userId);

        }

        public async Task<ICollection<SearchResultModel>> BuildSearchResultModelsAsync(ICollection<Data.Models.Advertisement> advertisements, string userId)
        {
            var results = new List<SearchResultModel>();

            foreach (var advertisement in advertisements)
            {
                Data.Models.Advertisement loadedAdvertisement = await context.Advertisements
                    .Include(c => c.City)
                    .Include(v => v.Vehicle)
                    .Include(x => x.FavoriteAdvertisements)
                    .FirstOrDefaultAsync(a => a.Id == advertisement.Id);

                if (loadedAdvertisement != null)
                {
                    var resultModel = new SearchResultModel()
                    {
                        AdvertisementId = loadedAdvertisement.Id,
                        City = loadedAdvertisement.City.Name,
                        Mileage = loadedAdvertisement.Vehicle.Mileage,
                        Name = loadedAdvertisement.Name,
                        Price = loadedAdvertisement.Price,
                        Region = context.Regions.FirstOrDefaultAsync(r => r.Id == loadedAdvertisement.City.RegionId).Result?.Name,
                        Year = loadedAdvertisement.Vehicle.Year,
                        CreatedOn = loadedAdvertisement.CreatedOnDate.ToString("HH:mm, dd.MM.yyyy"),
                        IsFavorite = loadedAdvertisement.FavoriteAdvertisements
                            .Any(x => x.AdvertisementId == loadedAdvertisement.Id && x.UserId == userId)
                    };

                    string fullPath = ImagesPath + "/Advertisement" + loadedAdvertisement.Id;

                    if (Directory.Exists(fullPath))
                    {
                        DirectoryInfo directory = new DirectoryInfo(fullPath);

                        FileInfo[] images = directory.GetFiles();

                        if (images.Any())
                        {
                            resultModel.Image = await File.ReadAllBytesAsync(fullPath + "/" + images[0].Name);
                        }
                    }

                    results.Add(resultModel);
                }
            }

            return results;
        }

        public async Task<ICollection<SearchResultModel>> FindAdvertisementsByUserIdAsync(string userId)
        {
            ICollection<Data.Models.Advertisement> advertisements =
             await context.Advertisements.Where(x => x.UserId == userId).ToListAsync();

            return await BuildSearchResultModelsAsync(advertisements, userId);
        }

        public async Task<ICollection<SearchResultModel>> GetUserFavoriteAdvertisementsAsync(string userId)
        {
            ICollection<string> favAdvertisementsIds = await context.UserFavAdvertisements
                .Where(x => x.UserId == userId).Select(x => x.AdvertisementId).ToListAsync();

            var favAdvertisements = await context.Advertisements.Where(x => favAdvertisementsIds.Contains(x.Id)).ToListAsync();

            return await BuildSearchResultModelsAsync(favAdvertisements, userId);
        }

        public async Task<AveragePriceModel> AveragePricesByGivenBrandAndModelAsync(AveragePriceModel priceModel, string userId)
        {
            var searchModel = new DetailedSearchAdvertisementModel()
            {
                Model = priceModel.Model,
                Brand = priceModel.Brand,
                Year = priceModel.Year,
                TransmissionType = priceModel.TransmissionType,
                EngineType = priceModel.EngineType
            };

            ICollection<Data.Models.Advertisement> advertisements = (await FindAdvertisementsAsync(searchModel, true)).ToList();

            var advertisementsAveragePrice = advertisements.DefaultIfEmpty().Average(p => p == null ? 0 : p.Price);

            priceModel.AveragePrice = Convert.ToDecimal(advertisementsAveragePrice.ToString("0.##"));

            priceModel.Advertisements = await BuildSearchResultModelsAsync(advertisements.ToList(), userId);

            priceModel.Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync());
            priceModel.Engines = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync());
            priceModel.Transmissions = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync());
            priceModel.Models = mapper.Map<ICollection<Model>, ICollection<ModelFormModel>>(await modelService.GetAllModelsAsync(priceModel.Brand));

            return priceModel;
        }

        public async Task<DetailedSearchAdvertisementModel> InitDetailedSearchAdvertisementModelAsync()
        {
            DetailedSearchAdvertisementModel model = new DetailedSearchAdvertisementModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(await regionService.GetAllRegionsAsync()),
                TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
                EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                VehicleCategories = mapper.Map<ICollection<VehicleCategory>, ICollection<CategoryFormModel>>(await categoryService.GetVehicleCategoriesAsync()),
                Colors = mapper.Map<ICollection<Color>, ICollection<ColorFormModel>>(await colorService.GetColorsAsync()),
                EuroStandards = mapper.Map<ICollection<VehicleEuroStandard>, ICollection<EuroStandardFormModel>>(await technicalService.GetEuroStandardsAsync()),
                Extras = mapper.Map<ICollection<ExtrasCategory>, ICollection<ExtrasCategoryFormModel>>(await technicalService.GetExtrasCategoriesAsync()),
            };

            return model;
        }

        public async Task<AveragePriceModel> InitAveragePriceModelAsync()
        {
            AveragePriceModel model = new AveragePriceModel()
            {
                Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                Engines = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                Transmissions = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
            };

            return model;
        }

        public async Task<HomeViewModel> InitHomeViewModelAsync(string userId)
        {
            var model = new HomeViewModel()
            {
                SearchAdvertisementModel = new SearchAdvertisementModel()
                {
                    Brands = mapper.Map<ICollection<Brand>, ICollection<BrandFormModel>>(await brandService.GetAllBrandsAsync()),
                    Regions = mapper.Map<ICollection<Region>, ICollection<RegionFormModel>>(await regionService.GetAllRegionsAsync()),
                    EngineTypes = mapper.Map<ICollection<VehicleEngineType>, ICollection<EngineFormModel>>(await technicalService.GetEngineTypesAsync()),
                    TransmissionTypes = mapper.Map<ICollection<TransmissionType>, ICollection<TransmissionFormModel>>(await technicalService.GetTransmissionTypesAsync()),
                },
                LatestPublishedAdvertisements = await GetLastPublishedAdvertisementsAsync(userId)
            };

            return model;
        }

        private async Task<List<Data.Models.Advertisement>> FindAdvertisementsAsync(SearchAdvertisementModel searchModel, bool isDetailedSearch)
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
            return await query.ToListAsync();
        }
    }
}