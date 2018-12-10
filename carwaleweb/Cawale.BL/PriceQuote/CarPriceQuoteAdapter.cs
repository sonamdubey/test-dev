using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.BL.Elastic.NewCarSearch;
using Carwale.BL.Experiments;
using Carwale.DAL.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.BL.PriceQuote
{
    public class CarPriceQuoteAdapter : ICarPriceQuoteAdapter
    {
        private readonly IPricesRepository<CarPriceQuote, VersionPriceQuote> _iPricesRepo;
        private readonly IPricesCacheRepository<CarPriceQuote, VersionPriceQuote> _iPricesCacheRepo;
        private readonly ICacheManager _ICacheManager;
        private readonly CarDataRepository _iDataRepo;
        private readonly ICarVersionCacheRepository _carVersionCacheRepository;
        private readonly IPQCacheRepository _pqCacheRepo;
        private readonly IPQGeoLocationBL _pqGeoLocation;
        private readonly IPrices _prices;
        private readonly INearbyCitiesSearch _nearbyCitiesSearch;
        private readonly IOperationsTrackingRepository _operationsTrackingRepo;
        private readonly IModelSimilarPriceDetailsBL _modelSimilarPriceDetailBL;

        public CarPriceQuoteAdapter(IPricesRepository<CarPriceQuote, VersionPriceQuote> iPricesRepo, IPricesCacheRepository<CarPriceQuote, VersionPriceQuote> iPricesCacheRepo, ICacheManager ICacheManager, CarDataRepository iDataRepo,
            ICarVersionCacheRepository carVersionCacheRepository, IPQCacheRepository pqCacheRepo, IPQGeoLocationBL pqGeoLocation, IPrices prices,
            INearbyCitiesSearch nearbyCitiesSearch, IOperationsTrackingRepository operationsTrackingRepo, IModelSimilarPriceDetailsBL modelSimilarPriceDetailBL)
        {
            _iPricesRepo = iPricesRepo;
            _iPricesCacheRepo = iPricesCacheRepo;
            _ICacheManager = ICacheManager;
            _iDataRepo = iDataRepo;
            _carVersionCacheRepository = carVersionCacheRepository;
            _pqCacheRepo = pqCacheRepo;
            _pqGeoLocation = pqGeoLocation;
            _prices = prices;
            _nearbyCitiesSearch = nearbyCitiesSearch;
            _operationsTrackingRepo = operationsTrackingRepo;
            _modelSimilarPriceDetailBL = modelSimilarPriceDetailBL;
        }

        private bool ValidateInput(CarPriceQuote pricesInput)
        {
            return (pricesInput != null && pricesInput.VersionPricesList != null
                    && pricesInput.ModelId > 0 && pricesInput.CityId > 0 && pricesInput.UpdatedBy > 0
                    && !pricesInput.VersionPricesList.Any((item) => item.VersionId <= 0 || item.PricesList == null
                                                                  || item.PricesList.Any((property) => property.PQItemId <= 0)));
        }

        private int SetStatus(ResponseDTO response)
        {
            if (response.Result)
                return (int)StatusCodesEnum.StatusCodes.Ok;

            return (int)StatusCodesEnum.StatusCodes.InternalServerError;
        }

        public CarPriceQuoteDTO GetModelPriceQuote(int modelId, int cityId, bool isNew, bool isSpecsMandatory, bool isCachedData)
        {
            CarPriceQuote priceQuote = null;
            CarPriceQuoteDTO modelPrices = null;

            priceQuote = _prices.GetModelPrices(modelId, cityId, isNew, isCachedData);
            if (priceQuote != null)
            {
                if (isSpecsMandatory)
                {
                    var versionsSummary = _carVersionCacheRepository.GetVersionSummaryByModel(modelId, Status.All);
                    if (versionsSummary != null)
                    {
                        var versionIds = versionsSummary.FindAll(x => x.New).Select(y => y.Id);
                        priceQuote.VersionPricesList = priceQuote.VersionPricesList.Where(x => versionIds.Contains(x.VersionId)).ToList();
                    }
                }

                modelPrices = Mapper.Map<CarPriceQuote, CarPriceQuoteDTO>(priceQuote);
            }
            return modelPrices;
        }

        public ResponseDTO InsertPriceQuote(CarPriceQuote pricesInput)
        {
            ResponseDTO response = null;
            try
            {
                int insertSuccess;
                response = new ResponseDTO();

                if (ValidateInput(pricesInput) && !(pricesInput.VersionPricesList.Any((item) => item.PricesList.Any((property) => property.PQItemValue <= 0))))
                {
                    List<int> priceAddedVersions = new List<int>();
                    response.Result = _iPricesRepo.InsertPriceQuote(pricesInput.VersionPricesList, pricesInput.CityId, pricesInput.UpdatedBy, ref priceAddedVersions);

                    if (pricesInput.SourceCityId > 0 && response.Result)
                    {
                        TrackCopyOperation(pricesInput, priceAddedVersions);
                    }

                    response.CityId = pricesInput.CityId;

                    AddPriceIndex(priceAddedVersions, pricesInput.CityId);
                    insertSuccess = SetStatus(response);

                    if (insertSuccess == (int)StatusCodesEnum.StatusCodes.Ok)
                    {
                        insertSuccess = (int)StatusCodesEnum.StatusCodes.Created;
                        NewCarElasticSearch.PushInCarDataQueue(pricesInput.ModelId, GetVersionIds(pricesInput.VersionPricesList), pricesInput.CityId, CarDocumentFields.PriceAdd);
                        SaveModelSimilarPriceDetails(pricesInput);
                    }

                    if (!(_iPricesCacheRepo.InvalidateCache(pricesInput)))
                    {
                        insertSuccess = (int)StatusCodesEnum.StatusCodes.ExpectationFailed;
                    }
                }
                else
                {
                    insertSuccess = (int)StatusCodesEnum.StatusCodes.BadRequest;
                }

                response.StatusCode = insertSuccess;

                return response;
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                response.StatusCode = (int)StatusCodesEnum.StatusCodes.InternalServerError;
                return response;
            }
        }

        private void TrackCopyOperation(CarPriceQuote pricesInput, List<int> priceAddedVersions)
        {
            try
            {
                foreach (var version in priceAddedVersions)
                {
                    List<VersionPriceQuote> prices = pricesInput.VersionPricesList.Where(x => x.VersionId == version).ToList();

                    foreach (var price in prices)
                    {
                        _operationsTrackingRepo.TrackOperations(version, pricesInput.CityId, price.IsMetallic, pricesInput.SourceCityId, pricesInput.UpdatedBy);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private static IEnumerable<int> GetVersionIds(IEnumerable<VersionPriceQuote> versionList)
        {
            try
            {
                return versionList.Select(x => x.VersionId).Distinct();
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex, "CarPriceQuoteAdapter.GetVersionIds");
            }
            return null;
        }
        /// <summary>
        /// To add "version-city" document when exshowroom price is added for version
        /// </summary>
        /// <param name="priceAddedVersions">List of VersionIds whose price is added</param>
        /// <param name="cityId">CityId for which price is added</param>
        private void AddPriceIndex(List<int> priceAddedVersions, int cityId)
        {
            try
            {
                foreach (var version in priceAddedVersions)
                {
                    _nearbyCitiesSearch.AddToIndex(version, cityId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarPriceQuoteAdapter.AddPriceIndex()");
                throw;
            }
        }

        public ResponseDTO DeletePriceQuote(CarPriceQuote pricesInput)
        {
            ResponseDTO response = null;
            try
            {
                int deleteSuccess;
                response = new ResponseDTO();

                if (ValidateInput(pricesInput))
                {
                    List<int> PriceDeletedVersions = new List<int>();
                    response.Result = _iPricesRepo.DeletePriceQuote(pricesInput.VersionPricesList, pricesInput.CityId, pricesInput.UpdatedBy, ref PriceDeletedVersions);
                    response.CityId = pricesInput.CityId;

                    DeletePriceIndex(PriceDeletedVersions, pricesInput.CityId);
                    deleteSuccess = SetStatus(response);
                    if (deleteSuccess == (int)StatusCodesEnum.StatusCodes.Ok)
                    {
                        NewCarElasticSearch.PushInCarDataQueue(pricesInput.ModelId, GetVersionIds(pricesInput.VersionPricesList), pricesInput.CityId, CarDocumentFields.PriceDelete);
                    }
                    if (!(_iPricesCacheRepo.InvalidateCache(pricesInput)))
                    {
                        deleteSuccess = (int)StatusCodesEnum.StatusCodes.ExpectationFailed;
                    }
                }
                else
                {
                    deleteSuccess = (int)StatusCodesEnum.StatusCodes.BadRequest;
                }

                response.StatusCode = deleteSuccess;

                return response;
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                response.StatusCode = (int)StatusCodesEnum.StatusCodes.InternalServerError;
                return response;
            }
        }

        /// <summary>
        /// To delete "version-city" document when ex-showroom price is deleted
        /// </summary>
        /// <param name="priceDeletedVersions">List of VersionIds whose price is deleted</param>
        /// <param name="cityId">City Id for which price is deleted</param>
        private void DeletePriceIndex(List<int> priceDeletedVersions, int cityId)
        {
            try
            {
                foreach (var version in priceDeletedVersions)
                {
                    _nearbyCitiesSearch.DeleteFromIndex(version, cityId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarPriceQuoteAdapter.DeletePriceIndex()");
                throw;
            }
        }

        /// <summary>
        /// Get average or Ex showroom price of All version for a model
        /// Written By : Sanjay Soni on <02/01/2017>
        /// </summary>
        public IEnumerable<VersionPrice> GetAllVersionPriceByModelCity(int modelId, int cityId, bool ORP = false, bool isNew = true)
        {
            try
            {
                if (cityId <= 0)
                    return GetAllVersionPriceByModelWithoutCity(modelId, isNew);
                else
                    return GetAllVersionPriceByModelWithCity(modelId, cityId, ORP, isNew);

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarPrices.GetAllVersionPriceByModelCity()");
                objErr.LogException();
                return null;
            }
        }

        private IEnumerable<VersionPrice> GetAllVersionPriceByModelWithoutCity(int modelId, bool isNew)
        {
            List<VersionPrice> modelVersionPriceList = new List<VersionPrice>();
            try
            {
                var modelVersionAvgPrice = _pqCacheRepo.GetModelsVersionAveragePrices(modelId, isNew);
                VersionAveragePrice versionAveragePrice = null;

                foreach (var version in modelVersionAvgPrice.Keys)
                {
                    modelVersionAvgPrice.TryGetValue(version, out versionAveragePrice);
                    if (versionAveragePrice != null)
                    {
                        var versionPrice = new VersionPrice()
                        {
                            VersionBase = new VersionBase()
                            {
                                AveragePrice = versionAveragePrice != null ? versionAveragePrice.CarAveragePrice : 0,
                                VersionId = versionAveragePrice.VersionId,
                                VersionName = versionAveragePrice.VersionName
                            }
                        };
                        modelVersionPriceList.Add(versionPrice);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarPrices.GetAllVersionPriceByModelWithoutCity()");
                objErr.LogException();
                return null;
            }

            return modelVersionPriceList;
        }

        private IEnumerable<VersionPrice> GetAllVersionPriceByModelWithCity(int modelId, int cityId, bool ORP, bool isNew)
        {
            List<VersionPrice> modelVersionPriceList = new List<VersionPrice>();
            try
            {
                var versionPriceQuoteList = new List<VersionPriceQuoteDTO>();
                var modelVersionsPriceQuote = GetModelPriceQuote(modelId, cityId, isNew, false, true);
                if (modelVersionsPriceQuote == null)
                    return modelVersionPriceList;
                else
                    versionPriceQuoteList = modelVersionsPriceQuote.VersionPricesList;

                var modelVersionAvgPrice = _pqCacheRepo.GetModelsVersionAveragePrices(modelId, isNew);

                bool isEligibleForOrp = ProductExperiments.IsEligibleForORP();
                bool showOnRoadPrice = (isEligibleForOrp || ORP) ? true : false;

                IDictionary<string, VersionPriceQuoteDTO> modelVersionsPriceQuoteHash = new Dictionary<string, VersionPriceQuoteDTO>();
                foreach (var versionPriceQuote in versionPriceQuoteList)
                {
                    var index = versionPriceQuote.VersionId.ToString() + '-' + versionPriceQuote.IsMetallic.ToString();
                    modelVersionsPriceQuoteHash[index] = versionPriceQuote;
                }

                string type = isNew ? "New" : "Discontinued";
                List<CarVersionEntity> versionList = _carVersionCacheRepository.GetCarVersionsByType(type, modelId);
                var usercityObj = _pqGeoLocation.GetCityById(cityId);
                foreach (var version in versionList)
                {
                    VersionAveragePrice versionAveragePrice = null;
                    modelVersionAvgPrice.TryGetValue(version.ID, out versionAveragePrice);

                    VersionPriceQuoteDTO versionBreakupMetallic = null;
                    VersionPriceQuoteDTO versionBreakupSolid = null;
                    modelVersionsPriceQuoteHash.TryGetValue(version.ID.ToString() + "-True", out versionBreakupMetallic);
                    modelVersionsPriceQuoteHash.TryGetValue(version.ID.ToString() + "-False", out versionBreakupSolid);

                    int price = 0;

                    if ((versionBreakupSolid != null && versionBreakupSolid.PricesList.Count > 0 && versionBreakupSolid.PricesList[0].Value != 0))
                    {
                        if (showOnRoadPrice)
                        {
                            price = versionBreakupSolid.PricesList.Where(x => x.OnRoadPriceInd).Sum(t => t.Value);
                        }
                        else
                        {
                            var priceQuote = versionBreakupSolid.PricesList.Where(y => y.Id == 2).FirstOrDefault();
                            price = priceQuote == null ? 0 : priceQuote.Value;
                        }
                    }
                    else if (versionBreakupMetallic != null && versionBreakupMetallic.PricesList.Count > 0 && versionBreakupMetallic.PricesList[0].Value != 0)
                    {
                        if (showOnRoadPrice)
                        {
                            price = versionBreakupMetallic.PricesList.Where(x => x.OnRoadPriceInd).Sum(t => t.Value);
                        }
                        else
                        {
                            var priceQuote = versionBreakupMetallic.PricesList.Where(y => y.Id == 2).FirstOrDefault();
                            price = priceQuote == null ? 0 : priceQuote.Value;
                        }
                    }

                    var versionPrice = new VersionPrice()
                    {
                        City = usercityObj,
                        PriceStatus = (int)PriceBucket.HaveUserCity,
                        VersionBase = new VersionBase()
                        {
                            AveragePrice = versionAveragePrice != null ? versionAveragePrice.CarAveragePrice : 0,
                            VersionId = version.ID,
                            VersionName = version.Name,
                            ExShowroomPrice = price
                        },
                        IsGSTPrice = (versionBreakupSolid != null && versionBreakupSolid.PricesList.Count > 0 && versionBreakupSolid.PricesList[0].Value != 0)
                                                ? CarPrices.CheckIsGstPrice(versionBreakupSolid.LastUpdated)
                                                : ((versionBreakupMetallic != null && versionBreakupMetallic.PricesList.Count > 0 && versionBreakupMetallic.PricesList[0].Value != 0) ? CarPrices.CheckIsGstPrice(versionBreakupMetallic.LastUpdated) : false)
                    };
                    modelVersionPriceList.Add(versionPrice);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarPrices.GetAllVersionPriceByModelWithCity()");
                objErr.LogException();
                return null;
            }
            return modelVersionPriceList;
        }

        private PriceOverview GetAvailableVersionPriceForSameModel(IDictionary<int, VersionPrice> versionPriceList, int versionId, int cityId, bool ORP = false)
        {
            VersionPrice versionPrice = null;
            PriceOverview carPriceAvailability = null;
            try
            {
                versionPriceList.TryGetValue(versionId, out versionPrice);

                if (versionPrice != null)
                {
                    bool isVersionBlocked = versionPrice.IsVersionBlocked;
                    var versionDetail = versionPrice.VersionBase;

                    if (cityId < 1)
                    {
                        carPriceAvailability = _prices.GetAveragePrice(versionPrice, 1, (int)PriceBucket.NoUserCity);
                    }
                    else
                    {
                        if (isVersionBlocked)
                        {
                            carPriceAvailability = _prices.GetAveragePrice(versionPrice, 1, (int)PriceBucket.CarNotSold);
                        }
                        else
                        {
                            if (versionDetail.ExShowroomPrice > 0)
                            {
                                if (ProductExperiments.IsEligibleForORP() || ORP)
                                    carPriceAvailability = _prices.GetOnRoadPrice(versionPrice, 1, (int)PriceBucket.HaveUserCity);
                                else
                                    carPriceAvailability = _prices.GetExShowRoomPrice(versionPrice, 1, (int)PriceBucket.HaveUserCity);
                            }
                            else if (versionDetail.AveragePrice > 0)
                            {
                                carPriceAvailability = _prices.GetAveragePrice(versionPrice, 1, (int)PriceBucket.PriceNotAvailable);
                            }
                        }
                    }
                    return carPriceAvailability;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.GetAvailableVersionPriceForSameModel()");
                objErr.SendMail();
            }
            return null;
        }

        public PriceOverview GetAvailablePriceForModel(int modelId, int cityId, IEnumerable<VersionPrice> versionPriceList = null, bool ORP = false)
        {
            PriceOverview carPriceAvailability = null;
            IEnumerable<VersionPrice> unblockedVersionList = null;
            try
            {
                versionPriceList = versionPriceList ?? GetAllVersionPriceByModelCity(modelId, cityId, ORP); // Filter response by IsVersionBlocked attr

                if (versionPriceList != null && versionPriceList.Count() > 0)
                {
                    unblockedVersionList = versionPriceList.Where(x => !x.IsVersionBlocked).ToList();
                }
                if (unblockedVersionList != null && unblockedVersionList.Count() > 0) // Checking after filteration
                {
                    // Status 0
                    if (cityId < 1)
                    {
                        var _versionPriceList = unblockedVersionList.OrderBy(x => x.VersionBase.AveragePrice);
                        carPriceAvailability = _prices.GetAveragePrice(_versionPriceList.FirstOrDefault(), _versionPriceList.Count(), (int)PriceBucket.NoUserCity);
                    }
                    else
                    {
                        // Status 1
                        var versionExShowRoomPriceList = unblockedVersionList.Where(x => x.VersionBase.ExShowroomPrice > 0).ToList();
                        if (versionExShowRoomPriceList.Count() > 0)
                        {
                            var _versionPriceList = versionExShowRoomPriceList.OrderBy(x => x.VersionBase.ExShowroomPrice);

                            if (ProductExperiments.IsEligibleForORP() || ORP)
                            {
                                carPriceAvailability = _prices.GetOnRoadPrice(_versionPriceList.FirstOrDefault(), _versionPriceList.Count(), (int)PriceBucket.HaveUserCity);
                            }
                            else
                            {
                                carPriceAvailability = _prices.GetExShowRoomPrice(_versionPriceList.FirstOrDefault(), _versionPriceList.Count(), (int)PriceBucket.HaveUserCity);
                            }
                        }
                        else
                        {
                            // Status 2
                            var VersionAveragePriceList = unblockedVersionList.Where(x => x.VersionBase.AveragePrice > 0).ToList();
                            var _versionPriceList = VersionAveragePriceList.OrderBy(x => x.VersionBase.AveragePrice);
                            if (VersionAveragePriceList.Count() > 0)
                            {
                                carPriceAvailability = _prices.GetAveragePrice(_versionPriceList.FirstOrDefault(), _versionPriceList.Count(), (int)PriceBucket.PriceNotAvailable);
                            }
                        }
                    }
                }
                else
                {
                    // Status 3
                    if (versionPriceList != null && versionPriceList.Count() > 0)
                    {
                        var VersionAveragePriceList = versionPriceList.Where(x => x.VersionBase.AveragePrice > 0).ToList();
                        var _versionPriceList = VersionAveragePriceList.OrderBy(x => x.VersionBase.AveragePrice);
                        carPriceAvailability = _prices.GetAveragePrice(_versionPriceList.FirstOrDefault(), VersionAveragePriceList.Count(), (int)PriceBucket.CarNotSold);
                    }
                }
                return carPriceAvailability;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.GetAvailabledPriceForModelCity()");
                objErr.SendMail();
            }
            return null;
        }

        public IDictionary<int, PriceOverview> GetVersionsPriceForSameModel(int modelId, List<int> versionList, int cityId, bool ORP = false)
        {
            var Versions = new Dictionary<int, PriceOverview>();
            try
            {
                IEnumerable<VersionPrice> modelPrices = GetAllVersionPriceByModelCity(modelId, cityId, ORP);
                IDictionary<int, VersionPrice> modelPriceOverviewHash = new Dictionary<int, VersionPrice>();
                if (modelPrices != null)
                {
                    foreach (var versionPriceOverview in modelPrices)
                    {
                        if (!modelPriceOverviewHash.ContainsKey(versionPriceOverview.VersionBase.VersionId))
                        {
                            modelPriceOverviewHash[versionPriceOverview.VersionBase.VersionId] = versionPriceOverview;
                        }
                    }
                }

                foreach (var versionId in versionList)
                {
                    Versions.Add(versionId, GetAvailableVersionPriceForSameModel(modelPriceOverviewHash, versionId, cityId, ORP));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.GetCarPriceOverview()");
                objErr.SendMail();
            }
            return Versions;
        }

        public IDictionary<int, PriceOverview> GetVersionsPriceForDifferentModel(List<int> versionList, int cityId, bool ORP = false)
        {
            var Versions = new Dictionary<int, PriceOverview>();
            try
            {
                foreach (var versionId in versionList)
                {
                    if (!Versions.ContainsKey(versionId))
                    {
                        Versions.Add(versionId, GetAvailablePriceForVersion(versionId, cityId, ORP));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.GetVersionsPriceForDifferentModel()");
                objErr.SendMail();
            }
            return Versions;
        }

        public PriceOverview GetAvailablePriceForVersion(int versionId, int cityId, bool ORP, bool isNew = true)
        {
            VersionPrice versionPrice = null;
            PriceOverview carPriceAvailability = null;

            var versionDetails = _carVersionCacheRepository.GetVersionDetailsById(versionId);
            try
            {
                if (versionDetails != null)
                {
                    var modelPrices = GetAllVersionPriceByModelCity(versionDetails.ModelId, cityId, ORP, isNew);
                    IDictionary<int, VersionPrice> modelPriceOverviewHash = new Dictionary<int, VersionPrice>();
                    foreach (var versionPriceOverview in modelPrices)
                    {
                        modelPriceOverviewHash[versionPriceOverview.VersionBase.VersionId] = versionPriceOverview;
                    }

                    modelPriceOverviewHash.TryGetValue(versionId, out versionPrice);

                    if (versionPrice != null)
                    {
                        bool isVersionBlocked = versionPrice.IsVersionBlocked;
                        var versionDetail = versionPrice.VersionBase;

                        if (cityId < 1)
                        {
                            carPriceAvailability = _prices.GetAveragePrice(versionPrice, 1, (int)PriceBucket.NoUserCity);
                        }
                        else
                        {
                            if (isVersionBlocked)
                            {
                                carPriceAvailability = _prices.GetAveragePrice(versionPrice, 1, (int)PriceBucket.CarNotSold);
                            }
                            else
                            {
                                if (versionDetail.ExShowroomPrice > 0)
                                {
                                    if (ProductExperiments.IsEligibleForORP() || ORP)
                                        carPriceAvailability = _prices.GetOnRoadPrice(versionPrice, 1, (int)PriceBucket.HaveUserCity);
                                    else
                                        carPriceAvailability = _prices.GetExShowRoomPrice(versionPrice, 1, (int)PriceBucket.HaveUserCity);
                                }
                                else if (versionDetail.AveragePrice > 0)
                                {
                                    carPriceAvailability = _prices.GetAveragePrice(versionPrice, 1, (int)PriceBucket.PriceNotAvailable);
                                }
                            }
                        }
                        return carPriceAvailability;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.GetAvailabledPriceForVersionCity()");
                objErr.SendMail();
            }
            return null;
        }


        public IDictionary<int, PriceOverview> GetModelsCarPriceOverview(List<int> modelList, int cityId, bool ORP = false)
        {
            var Models = new Dictionary<int, PriceOverview>();
            try
            {
                bool takeModels = modelList != null;

                if (takeModels) foreach (var modelId in modelList)
                    {
                        if (!Models.ContainsKey(modelId))
                        {
                            Models.Add(modelId, GetAvailablePriceForModel(modelId, cityId, null, ORP));
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarPrices.GetModelsCarPriceOverview()");
                objErr.SendMail();
            }
            return Models;
        }

        private void SaveModelSimilarPriceDetails(CarPriceQuote pricesInput)
        {
            try
            {
                if (pricesInput.SourceCityId < 1)
                {
                    List<int> changedPriceVersionIds = pricesInput.VersionPricesList.Where(y => y.IsMetallic == false).Select(x => x.VersionId).Distinct().ToList();
                    if (changedPriceVersionIds != null && changedPriceVersionIds.Count > 0)
                    {
                        bool isResetable = true;
                        ModelSimilarPriceDetail modelSimilarPriceDetail = new ModelSimilarPriceDetail();
                        var allVersions = _carVersionCacheRepository.GetCarVersionsByType("new", pricesInput.ModelId);
                        List<int> allVersionIds = allVersions.Select(x => x.ID).Distinct().ToList();

                        bool allPricesChanged = changedPriceVersionIds.Count == allVersionIds.Count && changedPriceVersionIds.All(allVersionIds.Contains);

                        if (allPricesChanged)
                        {
                            isResetable = pricesInput.VersionPricesList.Any(x => x.PricesList.Any(y => y.PQItemId == (int)PricesCategoryItem.Exshowroom)); // Ex-showroom price is changed
                        }

                        modelSimilarPriceDetail.ModelId = pricesInput.ModelId;
                        modelSimilarPriceDetail.CanResetRefreshCount = isResetable;
                        _modelSimilarPriceDetailBL.CreateOrUpdate(modelSimilarPriceDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarPriceQuoteAdapter.SaveModelSimilarPriceDetails() modelId = " + pricesInput.ModelId);
                objErr.LogException();
            }
        }


        public int GetVersionExShowroomPrice(int modelId, int versionId, int cityId)
        {
            try
            {
                var modelPrice = GetModelPriceQuote(modelId, cityId, true, false, true);

                if (modelPrice == null || modelPrice.VersionPricesList == null || modelPrice.VersionPricesList.Count == 0)
                {
                    return 0;
                }

                var versionPriceList = modelPrice.VersionPricesList.Where(x => x.VersionId == versionId).FirstOrDefault();

                if (versionPriceList == null || versionPriceList.PricesList == null || versionPriceList.PricesList.Count == 0)
                {
                    return 0;
                }

                var versionExShowroom = versionPriceList.PricesList.FirstOrDefault(y => y.Id == (int)PricesCategoryItem.Exshowroom).Value;

                return versionExShowroom;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarPriceQuoteAdapter.GetVersionExShowroomPrice() modelId = " + modelId);
                objErr.LogException();
                return 0;
            }
        }
    }
}

