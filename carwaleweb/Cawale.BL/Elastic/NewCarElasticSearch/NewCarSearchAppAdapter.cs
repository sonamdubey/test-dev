using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.Cache.Classification;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Search.Model;
using Carwale.Entity.CarData;
using Carwale.Entity.Classification;
using Carwale.Entity.Deals;
using Carwale.Entity.ElasticEntities;
using Carwale.Entity.NewCarFinder;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.Classification;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCarFinder;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Nest;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Carwale.BL.Elastic.NewCarSearch
{
    public class NewCarSearchAppAdapter : INewCarSearchAppAdapter
    {
        protected const string str_comma = ",";
        protected const char c_comma = ',';
        protected static readonly int versionThresholdCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["newsearchversionthreshold"] ?? "3");
        protected static readonly string moreVersionTxt = System.Configuration.ConfigurationManager.AppSettings["newsearchversiontxt"] ?? "View All";
        protected const string re_sys = "₹";
        private readonly INewCarElasticSearch _elasticsearch;
        private readonly IUnityContainer _container;
        private readonly INewCarFinderCacheRepository _newCarFinderCacheRepo;
        private readonly IBodyTypeCache _bodyTypeCache;
        private readonly ICarMakesCacheRepository _carMakeCacheRepository;
        public NewCarSearchAppAdapter(INewCarElasticSearch elasticsearch, IUnityContainer container,
            IBodyTypeCache bodyTypeCache, ICarMakesCacheRepository carMakeCacheRepository, INewCarFinderCacheRepository newCarFinderCacheRepo)
        {
            _elasticsearch = elasticsearch;
            _container = container;
            _newCarFinderCacheRepo = newCarFinderCacheRepo;
            _bodyTypeCache = bodyTypeCache;
            _carMakeCacheRepository = carMakeCacheRepository;
        }

        private static int SortByPrice(VersionPriceOverview a, VersionPriceOverview b)
        {
            if (a == null)
            {
                return 1;
            }
            else if (b == null)
            {
                return -1;
            }
            else
            {
                return a.PriceForSorting.CompareTo(b.PriceForSorting);
            }
        }
        public Carwale.DTOs.Search.Version.NewCarSearchDTO GetVersions(NameValueCollection queryString)
        {
            NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(queryString);

            Carwale.DTOs.Search.Version.NewCarSearchDTO DTO = new DTOs.Search.Version.NewCarSearchDTO() { MatchingVersions = new List<DTOs.Search.Version.NewCarSearchVersionDTO>() };
            if (inputs.ModelIds.Count < 1)
            {
                return DTO;
            }

            var data = _elasticsearch.GetVersions(inputs);

            if (data.ModelVersionDict.Any())
            {
                var first = data.ModelVersionDict.First().Value.First().Value;
                bool doAllVersionsMatch = first.VersionsCount == first.MatchingVersionsCount && first.MatchingVersionsCount > 3;
                if (doAllVersionsMatch)
                {
                    DTO.MoreVersionText = moreVersionTxt;
                }

                int counter = 0;

                foreach (var version in data.ModelVersionDict.First().Value.Values)
                {
                    DTO.MatchingVersions.Add(new DTOs.Search.Version.NewCarSearchVersionDTO()
                    {
                        VersionId = version.VersionId,
                        Name = version.VersionName,
                        CarRating = version.ReviewRate.ToString(),
                        SpecsSummary = version.SpecSummary.Replace("|", ""),
                        VersionOffer = Mapper.Map<DiscountSummary, DiscountSummaryDTO_Android>(version.DiscountSummary),
                        PriceOverview = Mapper.Map<PriceOverview, PriceOverviewDTO>(version.PriceOverview)
                    });
                    counter++;
                    if (doAllVersionsMatch && counter > 2)
                    {
                        break;
                    }
                }
            }

            return DTO;
        }

        public Carwale.DTOs.Search.Model.NewCarSearchDTO GetModels(NameValueCollection queryString)
        {
            NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(queryString);
            ElasticCarData data = _elasticsearch.GetModels(inputs);

            Carwale.DTOs.Search.Model.NewCarSearchDTO DTO = new DTOs.Search.Model.NewCarSearchDTO() { CarModels = new List<DTOs.Search.Model.NewCarSearchModelDTO>() };
            DTO.TotalModels = data.TotalModels;
            DTO.TotalVersions = data.TotalVersions;
            if (data.ModelVersionDict.Any())
            {
                if (!inputs.CountsOnly)
                {
                    foreach (var model in data.ModelVersionDict)
                    {
                        var first = model.Value.First().Value;
                        bool areAllVersionsMatching = first.VersionsCount == first.MatchingVersionsCount;
                        bool justOneVersion = first.MatchingVersionsCount == 1;
                        //MAP DTO 
                        var modelDTO = new Carwale.DTOs.Search.Model.NewCarSearchModelDTO()
                        {
                            MakeId = first.MakeId,
                            ModelId = first.ModelId,
                            MakeName = first.MakeName,
                            ModelName = first.ModelName,
                            MaskingName = first.ModelMaskingName,
                            CarRating = first.ReviewRate.ToString(),
                            OriginalImgPath = first.ImagePath,
                            HostUrl = first.HostUrl.Last() != '/' ? first.HostUrl + "/" : first.HostUrl
                        };

                        if (first.PriceOverview != null)
                        {
                            modelDTO.MinPrice = string.Format("₹ {0}", Format.PriceLacCr(first.PriceOverview.Price.ToString()));
                            modelDTO.MatchingVersionText = string.Format(justOneVersion ? "{0} {1} at ₹ <b>{2}</b>" : "{0} {1} starting at ₹ <b>{2}</b>"
                                , first.MatchingVersionsCount
                                , areAllVersionsMatching ? (justOneVersion ? "version available" : "versions available") : (justOneVersion ? "matching version" : "matching versions")
                                , Format.PriceLacCr(first.PriceOverview.Price.ToNullSafeString())
                                );
                        }
                        else if (first.ModelMinPrice > 0)
                        {
                            modelDTO.MinPrice = string.Format("₹ {0}", Format.PriceLacCr(first.ModelMinPrice.ToString()));
                            modelDTO.MatchingVersionText = string.Format("{0} {1}"
                                , first.MatchingVersionsCount
                                , areAllVersionsMatching ? (justOneVersion ? "version available" : "versions available") : (justOneVersion ? "matching version" : "matching versions")
                                );
                        }
                        else
                        {
                            modelDTO.MinPrice = "N/A";
                        }

                        if (modelDTO.MinPrice != "N/A")
                        {
                            string min = Format.PriceLacCr(first.ModelMinPrice.ToString());
                            string max = Format.PriceLacCr(first.ModelMaxPrice.ToString());
                            if (min.Equals(max))
                                modelDTO.PriceText = string.Format("₹ {0}", min);
                            else
                                modelDTO.PriceText = string.Format("₹ {0} - ₹ {1}", Format.PriceLacCr(first.ModelMinPrice.ToString()), Format.PriceLacCr(first.ModelMaxPrice.ToString()));
                        }
                        else
                        {
                            modelDTO.PriceText = "N/A";
                            modelDTO.MatchingVersionText = string.Format("{0} matching versions", first.MatchingVersionsCount);
                        }
                        DTO.CarModels.Add(modelDTO);
                    }
                }
            }
            return DTO;
        }
        public NewCarFinderDto GetNCFModels(NameValueCollection queryString)
        {
            NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(queryString);
            NCFElasticCarData data = _elasticsearch.GetNCFModels(inputs);
            NewCarFinderDto DTO = new NewCarFinderDto() { Models = new List<NewCarFinderModelDto>() };
            if (!string.IsNullOrWhiteSpace(queryString["cityId"]))
            {

                var cityName = _container.Resolve<IGeoCitiesCacheRepository>().GetCityNameById(queryString["cityId"]);
                DTO.CityDto = new DTOs.Geolocation.CityDTO()
                {
                    Id = Convert.ToInt32(queryString["cityId"]),
                    Name = cityName
                };
            }

            if (data != null)
            {
                DTO.TotalModels = data.TotalModels;
                DTO.TotalVersions = data.TotalVersions;
                try
                {
                    if (data.ModelVersionDict.Any() && !inputs.CountsOnly)
                    {
                        foreach (var model in data.ModelVersionDict)
                        {
                            var first = model.Value.FirstOrDefault().Value;
                            string label = string.Empty;
                            int minPrice = int.MaxValue;
                            foreach (var version in model.Value)
                            {
                                var priceOverView = version.Value.PriceOverview;
                                if (priceOverView != null && priceOverView.Price > 0 && priceOverView.Price < minPrice)
                                {
                                    minPrice = priceOverView.Price;
                                    label = priceOverView.PriceLabel;
                                }
                            }
                            //MAP DTO 
                            var modelDTO = new NewCarFinderModelDto()
                            {
                                MakeId = first.MakeId,
                                ModelId = first.ModelId,
                                MakeName = first.MakeName,
                                ModelName = first.ModelName,
                                MakeMaskingName = Format.FormatSpecial(first.MakeName),
                                ModelMaskingName = first.ModelMaskingName,
                                OriginalImagePath = first.ImagePath,
                                HostUrl = first.HostUrl.Last() != '/' ? first.HostUrl + "/" : first.HostUrl,
                                CarRating = first.ReviewRate,
                                PriceOverview = new VersionPriceOverview
                                {
                                    Price = ConfigurationManager.AppSettings["RupeeSymbol"] + Format.FormatNumericCommaSep(Convert.ToString(minPrice != int.MaxValue ? minPrice : 0)),
                                    Label = label,
                                    PriceForSorting = (minPrice != int.MaxValue ? minPrice : 0)
                                }
                            };

                            var carVersions = new List<NewCarFinderVersionDto>();
                            foreach (var version in model.Value)
                            {
                                var versionInfo = version.Value;
                                var specs = versionInfo.SpecSummary.Replace(",", "").Split('|');
                                var newCarFinderVersionDto = new NewCarFinderVersionDto()
                                {
                                    Id = version.Key,
                                    Name = versionInfo.VersionName,
                                    MaskingName = versionInfo.VersionMaskingName,
                                    FuelType = Convert.ToString(specs.Count() > 1 ? specs[1] : ""),
                                    MaxPower = string.Format("{0} bhp", versionInfo.PowerBHP),
                                    Transmission = Convert.ToString(specs.Count() > 2 ? specs[2].Trim() : ""),
                                    Displacement = versionInfo.Displacement > 0 ? string.Format("{0} cc", versionInfo.Displacement) : null,
                                    Emi = ConfigurationManager.AppSettings["RupeeSymbol"] + Calculation.Calculation.CalculateEmi(versionInfo.PriceOverview.Price),
                                    PriceOverview = new VersionPriceOverview()
                                    {
                                        Price = ConfigurationManager.AppSettings["RupeeSymbol"] + Format.FormatNumericCommaSep(versionInfo.PriceOverview.Price.ToString()),
                                        Label = versionInfo.PriceOverview.PriceLabel,
                                        PriceForSorting = versionInfo.PriceOverview.Price
                                    }
                                };
                                carVersions.Add(newCarFinderVersionDto);
                            }
                            carVersions.Sort((a, b) => SortByPrice(a.PriceOverview, b.PriceOverview));
                            modelDTO.Versions = carVersions;

                            DTO.Models.Add(modelDTO);
                        }
                        DTO.Models.Sort((a, b) => SortByPrice(a.PriceOverview, b.PriceOverview));
                    }
                }
                catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException)
                {
                    Logger.LogException(ex, "Error in fetching NCFModels");
                }
            }
            return DTO;
        }
        public List<BodyTypesDto> GetBodyTypes(NameValueCollection queryString)
        {
            List<BodyTypesDto> CarBodyTypesDTO = new List<BodyTypesDto>();

            try
            {
                var bodyTypeDetailEntity = _bodyTypeCache.GetBodyType();
                if (bodyTypeDetailEntity != null)
                {
                    List<BodyTypesDto> bodyTypesDetail = Mapper.Map<List<Entity.Classification.BodyType>, List<BodyTypesDto>>(bodyTypeDetailEntity);
                    KeyedBucket<object> bucketObj = null;
                    int budget = 0;
                    int.TryParse(queryString["budget"], out budget);
                    NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(queryString);
                    ISearchResponse<CarBaseEntity> elasticResponse = null;
                    if (inputs.cityId > 0 || inputs.budgets.Count > 0)
                    {
                        elasticResponse = _elasticsearch.GetBodyTypes(inputs);
                    }
                    if (elasticResponse == null || !elasticResponse.Hits.Any())
                    {
                        return bodyTypesDetail;
                    }
                    IDictionary<int, BodyTypesDto> bodyTypeDict = new Dictionary<int, BodyTypesDto>();

                    foreach (var bodytype in bodyTypesDetail)
                    {
                        if (!bodyTypeDict.ContainsKey(bodytype.Id))
                        {
                            bodyTypeDict.Add(bodytype.Id, bodytype);
                        }
                    }
                    if (elasticResponse.Aggregations != null && elasticResponse.Aggregations.Count > 0)
                    {
                        int bodyStyleId, modelCount;
                        foreach (IBucket bucket in (elasticResponse.Aggregations.FirstOrDefault().Value as BucketAggregate).Items)
                        {
                            bucketObj = ((KeyedBucket<object>)bucket);
                            bodyStyleId = Convert.ToInt32(bucketObj.Key);
                            modelCount = Convert.ToInt32((bucketObj.Aggregations.First().Value as ValueAggregate).Value);
                            bodyTypeDict[bodyStyleId].CarCount = modelCount;
                        }
                    }
                    List<int> bodyTypesOrder = null;
                    if (inputs.cityId > 0 && inputs.budgets.Count > 0)
                    {
                        bodyTypesOrder = _newCarFinderCacheRepo.GetBodyTypesOrder(inputs.budgets[0].LowerLimit, inputs.budgets[0].UpperLimit, inputs.cityId, budget);
                    }
                    if (bodyTypeDict.Any() && bodyTypesOrder != null && bodyTypesOrder.Any())
                    {
                        foreach (var bodyType in bodyTypesOrder)
                        {
                            if (bodyTypeDict.ContainsKey(bodyType) && bodyTypeDict[bodyType].CarCount > 0)
                            {
                                CarBodyTypesDTO.Add(bodyTypeDict[bodyType]);
                                bodyTypeDict.Remove(bodyType);
                            }
                        }
                    }
                    var tempList = new List<BodyTypesDto>();
                    foreach (var bodyType in bodyTypeDict)
                    {
                        tempList.Add(bodyType.Value);
                    }
                    tempList.Sort(delegate (BodyTypesDto a, BodyTypesDto b)
                    {
                        if (a.CarCount < b.CarCount)
                        {
                            return 1;
                        }
                        else if (a.CarCount > b.CarCount)
                        {
                            return -1;
                        }
                        else
                        {
                            return a.Name.CompareTo(b.Name);
                        }
                    });
                    CarBodyTypesDTO.AddRange(tempList);
                    for (int index = 0; index < Math.Min(CarBodyTypesDTO.Count, 2); index++)
                    {
                        if (CarBodyTypesDTO[index].CarCount > 0)
                        {
                            CarBodyTypesDTO[index].IsRecommended = true;
                            CarBodyTypesDTO[index].IsSelected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error in fetching bodytype");
            }
            return CarBodyTypesDTO;
        }
        public List<FuelTypesDto> GetFuelTypes(NameValueCollection queryString)
        {
            List<FuelTypesDto> CaFuelTypesDTO = new List<FuelTypesDto>();

            try
            {
                List<FuelTypes> fuelTypesEntity = _newCarFinderCacheRepo.GetFuelTypes();
                if (fuelTypesEntity != null)
                {
                    fuelTypesEntity = fuelTypesEntity.Where(x => x.Id != 4).ToList();
                    ISearchResponse<CarBaseEntity> elasticResponse = null;
                    var modelCountDict = new Dictionary<int, int>();
                    KeyedBucket<object> bucketObj = null;
                    List<FuelTypesDto> fuelTypesDetail = Mapper.Map<List<FuelTypes>, List<FuelTypesDto>>(fuelTypesEntity);
                    NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(queryString);
                    if (inputs.cityId > 0 || inputs.budgets.Count > 0 || inputs.bodytype.Any())
                    {
                        elasticResponse = _elasticsearch.GetFuelTypes(inputs);
                    }
                    if (elasticResponse == null || !elasticResponse.Hits.Any())
                    {
                        return fuelTypesDetail;
                    }
                    if (elasticResponse.Aggregations != null && elasticResponse.Aggregations.Count > 0)
                    {
                        int fuelId, modelCount;
                        foreach (IBucket bucket in (elasticResponse.Aggregations.FirstOrDefault().Value as BucketAggregate).Items)
                        {
                            bucketObj = ((KeyedBucket<object>)bucket);
                            fuelId = Convert.ToInt32(bucketObj.Key);
                            modelCount = Convert.ToInt32((bucketObj.Aggregations.First().Value as ValueAggregate).Value);
                            modelCountDict.Add(fuelId, modelCount);
                        }
                    }

                    foreach (var fueltype in fuelTypesDetail)
                    {
                        fueltype.CarCount = modelCountDict.ContainsKey(fueltype.Id) ? modelCountDict[fueltype.Id] : 0;
                        fueltype.IsSelected = false;
                        CaFuelTypesDTO.Add(fueltype);
                    }
                    CaFuelTypesDTO.Sort(delegate (FuelTypesDto a, FuelTypesDto b)
                    {
                        if (a.CarCount < b.CarCount)
                        {
                            return 1;
                        }
                        return b.CarCount.CompareTo(a.CarCount);
                    });
                }
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is KeyNotFoundException || ex is AutoMapperMappingException || ex is ArgumentNullException)
            {
                Logger.LogException(ex, "Error in fetching bodytype");
            }
            return CaFuelTypesDTO;
        }

        public AllFiltersDTO GetAllFilters(int sourceId)
        {
            AllFiltersDTO allFiltersDTO = new AllFiltersDTO
            {
                Makes = new MakesFilterDto { DisplayName = "Make" },
                BodyType = new BodyTypeFilterDto { DisplayName = "Body Type" },
                FuelType = new FuelTypeFilterDto { DisplayName = "Fuel Type" },
                TransmissionType = new TransmissionTypeFilterDto { DisplayName = "Tramsmission Type" },
                SeatingCapacity = new SeatingCapacityFilterDto { DisplayName = "Seating Capacity" },
            };
            try
            {
                NewCarFinderBudget newCarFinderBudget = null;
                EmiBase emiDetails = null;
                List<CarMakeEntityBase> carMakes = null;
                List<TransmissionTypeBase> transmissionTypes = null;
                ICarMakesCacheRepository _carMakesCacheRepository = _container.Resolve<ICarMakesCacheRepository>();
                IBodyTypeCache _bodyTypeCache = _container.Resolve<IBodyTypeCache>();

                newCarFinderBudget = _newCarFinderCacheRepo.GetBudgetDetails(-1);
                allFiltersDTO.Budget = Mapper.Map<NewCarFinderBudget, BudgetBaseDTO>(newCarFinderBudget);
                allFiltersDTO.Budget.DisplayName = "Budget";

                emiDetails = _newCarFinderCacheRepo.GetEmiDetails(-1);
                allFiltersDTO.Emi = Mapper.Map<EmiBase, EmiBaseDTO>(emiDetails);
                allFiltersDTO.Emi.DisplayName = "EMI";

                carMakes = _carMakesCacheRepository.GetCarMakesByType("new");
                allFiltersDTO.Makes.MakeList = Mapper.Map<List<CarMakeEntityBase>, List<CarMakesDtoV1>>(carMakes);

                List<BodyType> bodyTypesList = _bodyTypeCache.GetBodyType();
                allFiltersDTO.BodyType.BodyTypeList = Mapper.Map<List<BodyType>, List<BodyTypeBaseDTO>>(bodyTypesList);

                List<FuelTypes> fuelTypesList = _newCarFinderCacheRepo.GetFuelTypes();
                allFiltersDTO.FuelType.FuelTypeList = Mapper.Map<List<FuelTypes>, List<FuelTypeBaseDTO>>(fuelTypesList);

                transmissionTypes = _newCarFinderCacheRepo.GetTransmissionTypes();
                allFiltersDTO.TransmissionType.TransmissionTypeList = Mapper.Map<List<TransmissionTypeBase>, List<TransmissionBaseDTO>>(transmissionTypes);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return allFiltersDTO;
        }
        public Entity.NewCarFinder.MakeFilter GetMakeList(NameValueCollection queryString)
        {
            Entity.NewCarFinder.MakeFilter makeFilter = new Entity.NewCarFinder.MakeFilter();
            List<NcfMake> ncfPopularMake = null;
            List<NcfMake> ncfOtherMakes = null;
            try
            {
                NewCarSearchInputs inputs = _elasticsearch.GetElasticInputs(queryString);
                var modelList = _elasticsearch.GetModelList(inputs);
                if (modelList.IsNotNullOrEmpty())
                {
                    ncfOtherMakes = modelList.GroupBy(x => x.MakeId)
                                   .Select(y => new NcfMake { MakeId = y.Key, MakeName = y.ElementAt(0).MakeName, ModelPopularity = y.Max(z => z.ModelPopularity), ModelCount = y.Count() })
                                   .OrderByDescending(x => x.ModelPopularity).ToList();
                    ncfPopularMake = ncfOtherMakes.Take(5).ToList();
                    var allMakes = _carMakeCacheRepository.GetCarMakesByType("new");
                    var makeIds = ncfOtherMakes.Select(x => x.MakeId).ToList();
                    ncfOtherMakes.RemoveRange(0, ncfOtherMakes.Count < 5 ? ncfOtherMakes.Count : 5);

                    foreach (var make in allMakes)
                    {
                        if (makeIds.IndexOf(make.MakeId) < 0)
                        {
                            ncfOtherMakes.Add(new NcfMake
                            {
                                MakeId = make.MakeId,
                                MakeName = make.MakeName
                            });
                        }
                    }
                    ncfOtherMakes = ncfOtherMakes.OrderByDescending(x => x.ModelCount).ToList();
                    makeFilter.PopularMakes = ncfPopularMake;
                    makeFilter.OtherMakes = ncfOtherMakes;
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException || ex is InvalidCastException || ex is IndexOutOfRangeException)
            {
                Logger.LogException(ex);
            }
            return makeFilter;
        }
    }

}
