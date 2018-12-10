﻿using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.Elastic;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace Carwale.BL.PriceQuote
{
    public class CarPrices : IPrices
    {
        private readonly IPQCacheRepository _pqCacheRepo;
        private readonly ICacheManager _cache;
        private readonly ICarVersionCacheRepository _carVersionCacheRepository;
        private readonly INearbyCitiesSearch _nearbyCitiesBl;
        private readonly IPQGeoLocationBL _geoLocationBL;
        private readonly IChargesCacheRepository _charges;
        private readonly IChargeGroupsCacheRepository _chargeGroups;
        private readonly IPQRepository _pqRepo;

        public CarPrices(IPQRepository pqRepo, IPQCacheRepository pqCacheRepo, ICacheManager cache, ICarVersionCacheRepository carVersionCacheRepository,
            INearbyCitiesSearch nearbyCitiesBl, IPQGeoLocationBL geoLocationBL, IChargesCacheRepository charges, IChargeGroupsCacheRepository chargeGroups)
        {
            _pqCacheRepo = pqCacheRepo;
            _cache = cache;
            _carVersionCacheRepository = carVersionCacheRepository;
            _nearbyCitiesBl = nearbyCitiesBl;
            _geoLocationBL = geoLocationBL;
            _charges = charges;
            _pqRepo = pqRepo;
            _chargeGroups = chargeGroups;
        }

        public ModelPriceDTO GetOnRoadPrice(int modelId, int cityId)
        {
            ModelPriceDTO model = null;
            try
            {
                var modelPrices = GetModelCompulsoryPrices(modelId, cityId, true, true).VersionPricesList;

                if (modelPrices != null && modelPrices.Count() > 0)
                {
                    int solidPrice;
                    IEnumerable<PriceDTO> _prices;
                    var versions = modelPrices.Select(x => new { x.VersionId, x.VersionName }).Distinct();
                    IList<VersionPricesDTO> versionPricesDTO = new List<VersionPricesDTO>();

                    foreach (var version in versions)
                    {
                        versionPricesDTO.Add(new VersionPricesDTO
                        {
                            VersionId = version.VersionId,
                            VersionName = version.VersionName,
                            Prices = _prices = (from p in modelPrices
                                                where p.VersionId == version.VersionId
                                                && p.PricesList != null && p.PricesList.Count > 0
                                                from x in p.PricesList
                                                select new PriceDTO
                                                {
                                                    CategoryId = x.ChargeGroupPrice.Id,
                                                    CategoryItemId = x.ChargePrice.Charge.Id,
                                                    CategoryItemName = x.ChargePrice.Charge.Name,
                                                    CategoryItemValue = x.ChargePrice.Price,
                                                    IsMetallic = p.IsMetallic,
                                                    CategoryType = x.ChargeGroupPrice.Type,
                                                }),
                            OnRoadPrice = ((solidPrice = _prices.Where(c => !c.IsMetallic && c.CategoryType == (int)ChargeGroupType.Compulsory).Sum(x => x.CategoryItemValue)) > 0 ?
                                            solidPrice : _prices.Where(c => c.IsMetallic && c.CategoryType == (int)ChargeGroupType.Compulsory).Sum(x => x.CategoryItemValue)),
                            BasePrice = _prices.Where(d => (d.CategoryItemId == (int)PricesCategoryItem.Exshowroom)).Min(e => (int?)e.CategoryItemValue) ?? 0
                        });
                    }

                    model = new ModelPriceDTO();
                    model.MinPrice = versionPricesDTO.Min(m => m.BasePrice);
                    model.MaxPrice = versionPricesDTO.Max(m => m.BasePrice);
                    model.Versions = versionPricesDTO;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarPrices.GetOnRoadPrice for inputs of cityId :" + cityId + " and modelId : " + modelId);
                objErr.LogException();
            }
            return model;
        }

        public int GetMinOnRoadPrice(IEnumerable<VersionPricesDTO> Versions)
        {
            int onRoadPrice = 0;
            if (Versions != null && Versions.Count() > 0)
            {
                var updationList = Versions;
                onRoadPrice = updationList.OrderByDescending(i => i.OnRoadPrice).LastOrDefault().OnRoadPrice;
            }
            return onRoadPrice;
        }

        public Carwale.DTOs.PriceQuote.Prices GetVersionPQ(int cityId, int versionId)
        {
            PQ pq = _pqCacheRepo.GetPQ(cityId, versionId);

            PQOnRoadPriceDTO metaliList = new PQOnRoadPriceDTO();
            PQOnRoadPriceDTO nonmetaliList = new PQOnRoadPriceDTO();

            foreach (var item in pq.PriceQuoteList)
            {
                PQItemDTO pqitem = new PQItemDTO() { Key = item.Key, Value = item.Value };
                if (item.IsMetallic)
                {
                    metaliList.PriceQuoteList.Add(pqitem);
                    metaliList.OnRoadPrice += item.Value;
                }
                else
                {
                    nonmetaliList.PriceQuoteList.Add(pqitem);
                    nonmetaliList.OnRoadPrice += item.Value;
                }
            }

            Carwale.DTOs.PriceQuote.Prices prices = new Carwale.DTOs.PriceQuote.Prices();
            if (metaliList.OnRoadPrice > 0)
                prices.Metalic = metaliList;

            if (nonmetaliList.OnRoadPrice > 0)
                prices.Solid = nonmetaliList;

            return prices;
        }

        public PQ FilterPrices(int cityId, int versionId)
        {
            try
            {
                var pq = _pqCacheRepo.GetPQ(cityId, versionId);

                var filteredPrice = (from price in pq.PriceQuoteList
                                     where !price.IsMetallic
                                     select price).ToList();


                if (filteredPrice.Count > 0)
                {
                    pq.PriceQuoteList = filteredPrice;
                }

                pq.OnRoadPrice = pq.PriceQuoteList.Sum(x => x.Value);

                return pq;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.FilterPrices()");
                objErr.SendMail();
                return new PQ();
            }
        }

        public PriceOverview GetAveragePrice(VersionPrice versionPrice, int count, int priceStatus) // if count more than 1 then show Onwards after price value
        {
            if (versionPrice != null)
            {
                versionPrice.PriceVersionCount = count;
                versionPrice.PriceLabel = ConfigurationManager.AppSettings["AlternatePriceText"] ?? "Avg. Ex-Showroom";
                versionPrice.PriceStatus = priceStatus;

                var VersionWithExShowroom = Mapper.Map<VersionPrice, PriceOverview>(versionPrice);
                VersionWithExShowroom.Price = versionPrice.VersionBase.AveragePrice;
                if (VersionWithExShowroom.PriceStatus == (int)PriceBucket.PriceNotAvailable)
                {
                    VersionWithExShowroom.ReasonText = string.Format(ConfigurationManager.AppSettings["PriceReasonText"] ?? "", VersionWithExShowroom.City.CityName);
                }
                return VersionWithExShowroom;
            }
            return null;
        }

        public PriceOverview GetExShowRoomPrice(VersionPrice versionPrice, int count, int priceStatus) // if count more than 1 then show Onwards after price value
        {
            if (versionPrice != null)
            {
                versionPrice.PriceVersionCount = count;
                if (CustomParser.parseBoolObject(ConfigurationManager.AppSettings["IsGSTShow"]))
                {
                    versionPrice.PriceLabel = versionPrice.IsGSTPrice ? ("Ex-Showroom" + (ConfigurationManager.AppSettings["PriceWithGSTText"] ?? "")) :
                                                ("Ex-Showroom" + (ConfigurationManager.AppSettings["PriceEstimatedGSTText"] ?? ""));
                }
                else
                {
                    versionPrice.PriceLabel = ConfigurationManager.AppSettings["CityPriceText"] ?? "Ex-Showroom";
                }

                versionPrice.PriceStatus = priceStatus;

                var VersionWithAvgPrice = Mapper.Map<VersionPrice, PriceOverview>(versionPrice);
                VersionWithAvgPrice.Price = versionPrice.VersionBase.ExShowroomPrice;
                return VersionWithAvgPrice;
            }
            return null;
        }

        public PriceOverview GetOnRoadPrice(VersionPrice versionPrice, int count, int priceStatus) // if count more than 1 then show Onwards after price value
        {
            if (versionPrice != null)
            {
                versionPrice.PriceVersionCount = count;
                if (CustomParser.parseBoolObject(ConfigurationManager.AppSettings["IsGSTShow"]))
                {
                    versionPrice.PriceLabel = versionPrice.IsGSTPrice ? ("On Road Price" + (ConfigurationManager.AppSettings["PriceWithGSTText"] ?? "")) :
                                                ("On Road Price" + (ConfigurationManager.AppSettings["PriceEstimatedGSTText"] ?? ""));
                }
                else
                {
                    versionPrice.PriceLabel = ConfigurationManager.AppSettings["CityHavingPriceText"] ?? "On Road Price";
                }
                versionPrice.PriceStatus = priceStatus;

                var VersionWithAvgPrice = Mapper.Map<VersionPrice, PriceOverview>(versionPrice);
                VersionWithAvgPrice.Price = versionPrice.VersionBase.ExShowroomPrice;
                return VersionWithAvgPrice;
            }
            return null;
        }

        public EMIInformation GetEmiInformation(int price)
        {
            EMIInformation emiInformation = new EMIInformation();
            emiInformation.Text = "EMI starts at";
            emiInformation.Amount = Calculation.Calculation.CalculateEmi(price);
            emiInformation.TooltipMessage = "Assuming " + CustomParser.parseDoubleObject((ConfigurationManager.AppSettings["interestRate"])) +
                                            "% rate of interest and a tenure of " + CustomParser.parseDoubleObject((ConfigurationManager.AppSettings["tenure"])) + " months";
            emiInformation.LinkText = "Get EMI assistance";
            emiInformation.RupeeSymbol = ConfigurationManager.AppSettings["rupeeSymbol"];
            emiInformation.Suffix = ConfigurationManager.AppSettings["RupeeOnlySymbol"];
            return emiInformation;
        }

        public void UpdateCache(List<string> keys)
        {
            try
            {
                foreach (var key in keys)
                {
                    try
                    {
                        if (key.Contains("model-price-details") || key.Contains("version-price-details"))
                        {
                            _cache.ExpireCache(key);
                        }
                        else if (key.Contains("model-versions-price-details"))
                        {
                            string[] ids = key.Split('-');
                            _pqCacheRepo.ReplaceVersionsPriceByModelCity(int.Parse(ids[4]), int.Parse(ids[5]));
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorClass objErr = new ErrorClass(ex, "CarPrices.UpdateCache() - Inner");
                        objErr.SendMail();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.UpdateCache() - Outer");
                objErr.SendMail();
            }
        }

        public static bool CheckIsGstPrice(DateTime date)
        {
            return date >= CustomParser.parseDateObject(ConfigurationManager.AppSettings["GSTPriceUpdationTime"]);
        }

        public List<PQItem> MapFromVersionPriceQuoteDTO(IEnumerable<VersionPriceQuoteDTO> versionPricesList, int platformId)
        {
            List<PQItem> pqItems = new List<PQItem>();

            try
            {
                foreach (var versionPrice in versionPricesList)
                {
                    foreach (var price in versionPrice.PricesList.Where(x => x.OnRoadPriceInd))
                    {
                        pqItems.Add(new PQItem
                        {
                            CategoryItemId = price.Id,
                            Key = GetLabelForGst(price.Id, price.Name, versionPrice.LastUpdated, platformId),
                            Value = price.Value,
                            IsMetallic = versionPrice.IsMetallic
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = new ExceptionHandler(ex, "MapFromVersionPriceQuoteDTO");
                exception.LogException();
            }

            return pqItems;
        }

        private string GetLabelForGst(int categoryItemId, string name, DateTime lastUpdated, int platformId)
        {
            if (categoryItemId == (int)PricesCategoryItem.Exshowroom && CustomParser.parseBoolObject(ConfigurationManager.AppSettings["IsGSTShow"]))
            {
                string gstText = "";

                if (platformId == (int)Platform.CarwaleDesktop || platformId == (int)Platform.CarwaleMobile)
                {
                    gstText = CheckIsGstPrice(lastUpdated) ?
                            " (<a href = 'javascript:void(0)' id = 'gst-tooltip' class='class-ad-tooltip text-blue text-bold-imp font13' title=''>" +
                            ConfigurationManager.AppSettings["PQPagePriceWithGSTText"] + "</a>)" :
                            " (<a href = 'javascript:void(0)' id = 'gst-est-tooltip' class='class-ad-tooltip text-blue text-bold-imp font13' title=''>" +
                            ConfigurationManager.AppSettings["PQPagePriceEstimatedGSTText"] + "</a>)";
                }
                else
                {
                    gstText = ConfigurationManager.AppSettings["AppsGSTText"];
                }
                return name + gstText;
            }
            return name;
        }

        public NearByCityDetailsDto GetNearbyCitiesDto(int versionId, int cityId, int count)
        {
            NearByCityDetailsDto nearByCityDetailsDto = new NearByCityDetailsDto();
            try
            {
                nearByCityDetailsDto = Mapper.Map<NearByCityDetailsDto>(GetNearbyCities(versionId, cityId, count));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return nearByCityDetailsDto;
        }

        public NearByCityDetails GetNearbyCities(int versionId, int cityId, int count)
        {
            NearByCityDetails nearByCityDetails = new NearByCityDetails();

            try
            {
                List<VersionCityPricesObj> nearByCities = _nearbyCitiesBl.GetNearByCities(versionId, cityId, count);
                Dictionary<string, CustLocation> citiesDetails = _geoLocationBL.MultiGetCityNameFromCache(nearByCities.Select(c => c.CityId).ToList());

                foreach (var city in citiesDetails)
                {
                    var onRoadPrice = GetVersionOnRoadPrice(versionId, city.Value.CityId);

                    if (onRoadPrice > 0)
                    {
                        nearByCityDetails.Cities.Add(new NearByCity
                        {
                            Id = city.Value.CityId,
                            Name = city.Value.CityName,
                            OnRoadPrice = ConfigurationManager.AppSettings["rupeeSymbol"] + Format.PriceLacCr(onRoadPrice.ToString(CultureInfo.InvariantCulture))
                        });
                    }
                }

                nearByCityDetails.WidgetHeading = GetNearbyCitiesWidgetHeading(versionId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return nearByCityDetails;
        }

        /// <summary>
        /// This function returns OnRoadPrice of a version 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private double GetVersionOnRoadPrice(int versionId, int cityId)
        {
            double onRoadPrice = 0;
            try
            {
                int modelId = _carVersionCacheRepository.GetVersionDetailsById(versionId).ModelId;

                onRoadPrice = GetVersionORP(modelId, versionId, cityId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return onRoadPrice;
        }

        /// <summary>
        /// This function returns OnRoadPrice of a version 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public int GetVersionOnRoadPrice(int modelId, int versionId, int cityId)
        {
            return (int)GetVersionORP(modelId, versionId, cityId);
        }

        private double GetVersionORP(int modelId, int versionId, int cityId)
        {
            double onRoadPrice = 0;
            try
            {
                IEnumerable<ModelPrices> modelPrices = _pqCacheRepo.GetModelPrices(modelId, cityId);
                var versionPrices = (from p in modelPrices
                                     where p.VersionId == versionId
                                     select new PriceDTO
                                     {
                                         CategoryId = p.CategoryId,
                                         CategoryItemId = p.CategoryItemId,
                                         CategoryItemName = p.CategoryItemName,
                                         CategoryItemValue = p.CategoryItemValue,
                                         IsMetallic = p.IsMetallic
                                     });
                if (versionPrices.IsNotNullOrEmpty())
                {
                    double solidPrice = versionPrices.Where(c => !c.IsMetallic).Sum(x => x.CategoryItemValue);
                    onRoadPrice = solidPrice > 0 ? solidPrice : versionPrices.Where(c => c.IsMetallic).Sum(x => x.CategoryItemValue);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return onRoadPrice;
        }

        private string GetNearbyCitiesWidgetHeading(int versionId)
        {
            string modelName = "";
            var versionDetails = _carVersionCacheRepository.GetVersionDetailsById(versionId);
            if (versionDetails != null)
            {
                modelName = versionDetails.ModelName;
            }

            string heading = ConfigurationManager.AppSettings["NearByCitiesWidgetHeading"] ?? "";
            heading = heading.Replace("@ModelName", modelName);
            return heading;
        }

        public List<CityDTO> GetNearbyCitieswithPrices(int versionId, int cityId, int count)
        {
            List<CityDTO> cityDetails = new List<CityDTO>();
            try
            {
                List<VersionCityPricesObj> nearByCities = _nearbyCitiesBl.GetNearByCities(versionId, cityId, count);
                var cityDetail = _geoLocationBL.MultiGetCityNameFromCache(nearByCities.Select(c => c.CityId).ToList());
                foreach (var item in cityDetail)
                {
                    cityDetails.Add(Mapper.Map<CustLocation, CityDTO>(item.Value));
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "GetNearbyCitieswithPrices");
                exception.LogException();
            }
            return cityDetails;
        }

        public CarPriceQuote GetModelAllPrices(int modelId, int cityId, bool isNew, bool isCachedData)
        {
            CarPriceQuote carPriceQuote = null;
            List<VersionPrices> versionPrice = null;
            try
            {
                versionPrice = _pqRepo.GetVersionsPriceList(modelId, cityId, isNew);
                carPriceQuote = new CarPriceQuote();

                carPriceQuote.VersionPricesList = MapResultIntoVersionPrice(versionPrice, isCachedData);
                carPriceQuote.ModelId = modelId;
                carPriceQuote.CityId = cityId;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return carPriceQuote;
        }

        private List<VersionPriceQuote> MapResultIntoVersionPrice(List<VersionPrices> versionPrice, bool isCachedData)
        {
            List<VersionPriceQuote> priceQuote = new List<VersionPriceQuote>();
            if (versionPrice != null)
            {
                int count = versionPrice.Count;
                if (count > 0)
                {
                    priceQuote.Add(Mapper.Map<VersionPrices, VersionPriceQuote>(versionPrice[0]));
                    priceQuote[0].PricesList = new List<PQItemList>();
                }
                GetMappedVersionPrice(ref priceQuote, versionPrice, count, isCachedData);
            }

            return priceQuote;
        }

        private void GetMappedVersionPrice(ref List<VersionPriceQuote> priceQuote, List<VersionPrices> versionPrice, int versionPriceCount, bool isCachedData)
        {
            var allCharges = _charges.GetCharges();
            var allChargeGroup = _chargeGroups.GetChargeGroups();
            int versionPriceIndex = 0;
            int priceQuoteIndex = 0;
            try
            {
                while (versionPriceIndex < versionPriceCount)
                {
                    Carwale.Entity.Price.Charge charge = null;
                    Carwale.Entity.Price.ChargeGroup chargeGroup = null;
                    var currentVersionPrice = versionPrice[versionPriceIndex];
                    var currentPriceQuote = priceQuote[priceQuoteIndex];

                    if (currentVersionPrice.VersionId == currentPriceQuote.VersionId && currentVersionPrice.IsMetallic == currentPriceQuote.IsMetallic)
                    {
                        if (allCharges.TryGetValue(currentVersionPrice.PQItemId, out charge) && allChargeGroup.TryGetValue(charge.ChargeGroupId, out chargeGroup))
                        {
                            var chargePrice = new ChargePrice();
                            chargePrice.Charge = charge;
                            chargePrice.Price = currentVersionPrice.PQItemValue;

                            currentPriceQuote.PricesList.Add(new PQItemList
                            {
                                PQItemId = currentVersionPrice.PQItemId,
                                PQItemValue = currentVersionPrice.PQItemValue,
                                OnRoadPriceInd = chargeGroup.Type == (int)ChargeGroupType.Compulsory,
                                PQItemName = charge.Name,
                                ChargePrice = chargePrice,
                                ChargeGroupPrice = Mapper.Map<Carwale.Entity.Price.ChargeGroup, ChargeGroupPrice>(chargeGroup)
                            });

                            if (currentPriceQuote.LastUpdated > currentVersionPrice.LastUpdated)
                            {
                                currentPriceQuote.LastUpdated = currentVersionPrice.LastUpdated;
                            }
                        }
                        else
                        {
                            if (!isCachedData) // To handle OPR NewShowoomPrices Page
                            {
                                currentPriceQuote.PricesList.Add(new PQItemList());
                            }
                        }
                        versionPriceIndex++;
                    }
                    else
                    {
                        priceQuote.Add(new VersionPriceQuote
                        {
                            VersionId = currentVersionPrice.VersionId,
                            VersionName = currentVersionPrice.VersionName,
                            IsMetallic = currentVersionPrice.IsMetallic,
                            IsNew = currentVersionPrice.IsNew,
                            LastUpdated = currentVersionPrice.LastUpdated,
                            PricesList = new List<PQItemList>()
                        });
                        priceQuoteIndex++;
                    }

                }
                CalculateOnRoadPrice(ref priceQuote);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        private void CalculateOnRoadPrice(ref List<VersionPriceQuote> priceQuote)
        {
            for (int version = 0; version < priceQuote.Count; version++)
            {
                if (priceQuote[version].PricesList != null && priceQuote[version].PricesList.Count > 0 && priceQuote[version].PricesList[0].ChargePrice != null)
                {
                    priceQuote[version].PricesList = priceQuote[version].PricesList.Where(x => x.ChargeGroupPrice != null).OrderBy(x => x.ChargeGroupPrice.SortOrder).ThenBy(x => x.ChargePrice.Charge.SortOrder).ToList(); // Sorting
                    priceQuote[version].OnRoadPrice = priceQuote[version].PricesList.Where(x => x.OnRoadPriceInd).Sum(y => y.PQItemValue);
                }
            }
        }

        public CarPriceQuote GetModelPrices(int modelId, int cityId, bool isNew, bool isCachedData)
        {
            CarPriceQuote modelPrice = null;
            try
            {
                if (isCachedData)
                {
                    modelPrice = _pqCacheRepo.GetModelPricesCache(modelId, cityId, isNew);
                    if (modelPrice == null)
                    {
                        modelPrice = GetModelAllPrices(modelId, cityId, isNew, isCachedData);
                        _pqCacheRepo.StoreModelPrices(modelId, cityId, isNew, modelPrice);
                        return modelPrice;
                    }
                    return modelPrice;
                }
                else
                {
                    return GetModelAllPrices(modelId, cityId, isNew, isCachedData);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CarPrices.GetModelPrices() : modelId: " + modelId + " city: " + cityId + " isNew: " + isNew + " isCachedData: " + isCachedData);
            }
            return null;
        }

        public CarPriceQuote GetModelCompulsoryPrices(int modelId, int cityId, bool isNew, bool isCachedData)
        {
            var carPriceQuote = GetModelPrices(modelId, cityId, isNew, isCachedData);
            var versionPricesList = carPriceQuote.VersionPricesList;
            try
            {
                GetCompulsoryItems(ref versionPricesList);
                carPriceQuote.VersionPricesList = versionPricesList;
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CarPrices.GetModelCompulsoryPrices() : modelId: " + modelId + " city: " + cityId + " isNew: " + isNew + " isCachedData: " + isCachedData);
            }
            return carPriceQuote;
        }

        public List<VersionPriceQuote> GetVersionCompulsoryPrices(int versionId, int cityId, bool isCachedData)
        {
            List<VersionPriceQuote> versionPricesList = null;
            try
            {
                int modelId = _carVersionCacheRepository.GetVersionDetailsById(versionId).ModelId;
                versionPricesList = GetVersionCompulsoryPrices(modelId, versionId, cityId, isCachedData);
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CarPrices.GetVersionCompulsoryPrices1() : versionId: " + versionId + " city: " + cityId + " isCachedData: " + isCachedData);
            }

            return versionPricesList;
        }

        public List<VersionPriceQuote> GetVersionCompulsoryPrices(int modelId, int versionId, int cityId, bool isCachedData)
        {
            List<VersionPriceQuote> versionPricesList = null;
            try
            {
                var carPriceQuote = GetModelPrices(modelId, cityId, true, isCachedData);
                versionPricesList = carPriceQuote.VersionPricesList.Where(x => x.VersionId == versionId).ToList();

                GetCompulsoryItems(ref versionPricesList);
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CarPrices.GetVersionCompulsoryPrices2() : modelId: " + modelId + " versionId: " + versionId + " city: " + cityId + " isCachedData: " + isCachedData);
            }

            return versionPricesList;
        }

        private static void GetCompulsoryItems(ref List<VersionPriceQuote> versionPricesList)
        {
            for (int i = 0; i < versionPricesList.Count; i++)
            {
                versionPricesList[i].PricesList = versionPricesList[i].PricesList.Where(x => (x.ChargeGroupPrice.Type != (int)ChargeGroupType.Optional)).ToList();
            }
        }
    }
}
