using AEPLCore.Cache;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Interfaces.Geolocation;
using System.Configuration;
using Microsoft.Practices.Unity;
using Carwale.DTOs.NewCars;
using AutoMapper;
using Carwale.Interfaces.Deals.Cache;
using System.Web;
using Carwale.Interfaces.Customer;
using Carwale.Notifications.Logs;
using Carwale.DAL.ApiGateway;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.DAL.ApiGateway.Extensions.CarData;
using VehicleData.Service.ProtoClass;
using Carwale.Entity.VehicleData;
using AEPLCore.Cache.Interfaces;

namespace Carwale.BL.CarData
{
    public class CarVersionsBL : ICarVersions
    {
        protected readonly ICarVersionCacheRepository _carVersionsCache;
        private readonly IPriceQuoteBL _pqBL;
        private readonly ICarModelCacheRepository _modelCacheRepository;
        private readonly ICacheManager _cache;
        private readonly ICarVersionRepository _carVersionsRepo;
        private readonly IPrices _iPrice;
        private readonly IDealsCache _dealsCache;
        private readonly ICarPriceQuoteAdapter _carPrices;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly IPQGeoLocationBL _geoBL;
        private readonly IUnityContainer _unityContainer;
        private readonly ICustomerTracking _trackingBL;
        private readonly ITyresBL _tyresBl;
        private readonly static string _defaultCityName = ConfigurationManager.AppSettings["DefaultCityName"] ?? string.Empty;

        public CarVersionsBL(ICarVersionCacheRepository carVersionsCache, IPriceQuoteBL pqBL, ICacheManager cache, IDealsCache dealsCache, ICarVersionRepository carVersionsRepo, IPrices iPrice, ICarPriceQuoteAdapter carPrices,
            ICarModelCacheRepository modelCacheRepository, IGeoCitiesCacheRepository geoCitiesCacheRepo, IPQGeoLocationBL geoBL, IUnityContainer unityContainer, ICustomerTracking trackingBL, ITyresBL tyresBl)
        {
            _carVersionsCache = carVersionsCache;
            _pqBL = pqBL;
            _cache = cache;
            _carVersionsRepo = carVersionsRepo;
            _iPrice = iPrice;
            _carPrices = carPrices;
            _modelCacheRepository = modelCacheRepository;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _geoBL = geoBL;
            _dealsCache = dealsCache;
            _unityContainer = unityContainer;
            _trackingBL = trackingBL;
            _tyresBl = tyresBl;
        }

        /// <summary>
        /// Returns New Car Versions List
        /// Written By : Shalini Nair on 19/09/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>List of New Car Versions</returns>
        public List<CarVersions> GetCarVersions(int modelId, Status status)
        {
            try
            {
                var versionsSummary = _carVersionsCache.GetVersionSummaryByModel(modelId, status);
                if (versionsSummary.IsNotNullOrEmpty())
                {
                    IEnumerable<int> itemIds = new List<int> { (int)Items.Mileage_ARAI,
                        (int)Items.Displacement,(int)Items.Fuel_Type,(int)Items.Transmission_Type,(int)Items.Seating_Capacity,(int)Items.Drivetrain,(int)Items.Max_Power};
                    List<VersionSpecsSummary> response = GetSpecsSummaryForVersions(versionsSummary.Select(x => x.Id), itemIds);
                    if (response.IsNotNullOrEmpty())
                    {
                        for (int versionCount = 0; versionCount < versionsSummary.Count; versionCount++)
                        {
                            VersionSpecsSummary versionData = versionCount < response.Count ? response[versionCount] : null;
                            var version = versionsSummary[versionCount];
                            MapCarVersionWithSpecs(version, versionData);
                        }
                    }
                    return versionsSummary;
                }
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
            return new List<CarVersions>();
        }

        /// <summary>
        /// Return SpecSummary for list of versions
        /// Written By: Jitendra Singh on 06/03/2018
        /// </summary>
        /// <param name="versions"></param>
        /// <returns></returns>
        private List<VersionSpecsSummary> GetSpecsSummaryForVersions(IEnumerable<int> versions, IEnumerable<int> itemIds)
        {
            try
            {
                if (versions.Any())
                {
                    IApiGatewayCaller _apiGatewayCaller = _unityContainer.Resolve<IApiGatewayCaller>();

                    _apiGatewayCaller.GetVersionSpecsSummary(versions, itemIds);
                    _apiGatewayCaller.Call();
                    return _apiGatewayCaller.GetResponse<VersionSpecsSummaryList>(0).Values.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarVersionsBL.GetSpecsSummaryForVersions()" + versions);
            }
            return new List<VersionSpecsSummary>();
        }

        /// <summary>
        /// Function is used for mapping version with specs summary
        /// </summary>
        /// <param name="carVersions"></param>
        /// <param name="specsSummary"></param>
        /// <returns>List<CarVersions> with specs summary maped</returns>
        private static void MapCarVersionWithSpecs(CarVersions carVersions, VersionSpecsSummary specsSummary)
        {
            try
            {
                string mileage = string.Empty;
                if (specsSummary != null && specsSummary.Specs.Any())
                {
                    var specList = specsSummary.Specs;
                    foreach (var item in specList)
                    {
                        if (item.ItemId == (int)Items.Displacement)
                        {
                            carVersions.Displacement = string.Format("{0} {1}", item.Value, string.IsNullOrEmpty(item.Value) ? string.Empty : item.UnitType);
                        }
                        else if (item.ItemId == (int)Items.Fuel_Type)
                        {
                            carVersions.CarFuelType = item.Value;
                        }
                        else if (item.ItemId == (int)Items.Transmission_Type)
                        {
                            carVersions.TransmissionType = item.Value;
                        }
                        else if (item.ItemId == (int)Items.Mileage_ARAI)
                        {
                            double value = 0;
                            double.TryParse(item.Value, out value);
                            carVersions.Arai = value;
                            carVersions.MileageUnit = item.UnitType;
                            mileage = string.Format("{0} {1}", item.Value, string.IsNullOrEmpty(item.Value) ? string.Empty : item.UnitType);
                        }
                        else if (item.ItemId == (int)Items.Drivetrain)
                        {
                            carVersions.Drivetrain = item.Value;
                        }
                        else if (item.ItemId == (int)Items.Seating_Capacity)
                        {
                            carVersions.SeatingCapacity = item.Value;
                        }
                        else if (item.ItemId == (int)Items.Max_Power)
                        {
                            carVersions.MaxPower = string.Format("{0} {1}", item.Value, string.IsNullOrEmpty(item.Value) ? string.Empty : item.UnitType);
                        }
                    }

                    carVersions.SpecsSummary = string.Format("{0}{1}{2}{3}",
                        StringwithCommaSeperator(carVersions.Displacement),
                        StringwithCommaSeperator(carVersions.CarFuelType),
                        StringwithCommaSeperator(carVersions.TransmissionType),
                        mileage);

                    carVersions.IsSpecsExist = specList.Any();
                }
                else
                {
                    carVersions.SpecsSummary = string.Empty;
                    carVersions.IsSpecsExist = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private static string StringwithCommaSeperator(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? string.Empty : string.Format("{0}, ", str);
        }

        /// <summary>
        ///  Gets the no. of Transmissions,Engines and Fuel
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public int[] ProcessVersionsData(int modelId, List<NewCarVersionsDTO> versionsList)
        {
            var noOfVersions = 0;
            var noOfTransmission = 0;
            var noOfEngines = 0;
            var noOfFuels = 0;
            int[] processedVersionData = new int[4];
            try
            {
                //var versionsList = GetCarVersions(modelId, Status.New);// Gets the list of running Car versions 
                noOfVersions = versionsList.Count; // Count of running car versions
                Dictionary<string, byte> transmission = new Dictionary<string, byte>();
                Dictionary<string, byte> engine = new Dictionary<string, byte>();
                Dictionary<string, byte> fuel = new Dictionary<string, byte>();

                for (int i = 0; i < noOfVersions; i++)
                {
                    string specSummary = versionsList[i].SpecsSummary;

                    // Check if Specs Summary is available 
                    if (!String.IsNullOrEmpty(specSummary))
                    {
                        string[] words = specSummary.Replace(",", string.Empty).Replace("/", string.Empty).Split('|'); //Formatting the data 

                        for (int k = 0; k < words.Length; k++)
                        {
                            if (words[k] == string.Empty)
                            {
                                words[k] = "@";
                            }
                        }
                        if ((words.Length >= 3 && words[2] != "@") && !transmission.ContainsKey(words[2]))
                        {
                            transmission[words[2]] = 1;
                        }
                        if ((words.Length >= 1 && words[0] != "@") && !engine.ContainsKey(words[0]))
                        {
                            engine[words[0]] = 1;
                        }
                        if ((words.Length >= 2 && words[1] != "@") && !fuel.ContainsKey(words[1]))
                        {
                            fuel[words[1]] = 1;
                        }
                    }
                }
                noOfEngines = engine.Keys.Count;  // No. of Engines 
                noOfTransmission = transmission.Keys.Count;// No. of Transmission types
                noOfFuels = fuel.Keys.Count; // No. of Fuel Types 
                processedVersionData = new int[] { noOfVersions, noOfEngines, noOfTransmission, noOfFuels };
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CarVersionsBL.ProcessVersionsData()");
                objErr.SendMail();
            }
            return processedVersionData;
        }

        public PqVersionCitiesEntity PqVersionsAndCities(int modelId)
        {
            var modelData = _modelCacheRepository.GetModelDetailsById(modelId);

            var response = new PqVersionCitiesEntity()
            {
                SmallPicUrl = modelData.ModelImageSmall,
                LargePicUrl = modelData.ModelImageLarge,
                MinPrice = modelData.MinPrice,
                MaxPrice = modelData.MaxPrice,
                ReviewCount = modelData.ReviewCount,
                ReviewRate = modelData.ModelRating,
                OfferExists = modelData.OfferExists,
                ExShowroomCity = _defaultCityName,
                CarName = string.Format("{0} {1}", modelData.MakeName, modelData.ModelName),
                OriginalImgPath = modelData.OriginalImage,
                HostUrl = modelData.HostUrl,
                Versions = _carVersionsCache.GetCarVersionsByType("new", modelId),
                Zones = _geoBL.GetPQCityZonesList(modelId),
                Cities = _geoCitiesCacheRepo.GetPQCitiesByModelId(modelId)
            };

            return response;
        }

        /// <summary>
        /// Author:Rakesh Yadav On 09 Oct 2015
        /// desc: get fuels and alternative fuels for version
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public CarFuel GetCarFuel(int versionId)
        {
            var versionDeatils = _carVersionsCache.GetVersionDetailsById(versionId);
            var versionFuels = new CarFuel()
            {
                Id = versionDeatils.FuelTypeId,
                Name = versionDeatils.FuelType
            };

            if (versionFuels.Id == Convert.ToInt32(CarFuelType.Petrol))
            {
                versionFuels.AlternativeFuels = new List<IdName>();
                versionFuels.AlternativeFuels.Add(new IdName { Id = Convert.ToInt32(CarFuelType.CNG), Name = "CNG" });
                versionFuels.AlternativeFuels.Add(new IdName { Id = Convert.ToInt32(CarFuelType.LPG), Name = "LPG" });
            }

            return versionFuels;

        }

        public List<CarVersionEntity> GetCarVersionsByType(string type, int modelId, int cityId, UInt16? year = null)
        {

            try
            {
                if (type.ToLower() == "new")
                {
                    if (cityId <= 0) cityId = 10;
                    return _pqBL.GetCarVersionDetails(modelId, cityId);
                }
                else
                {
                    return _carVersionsCache.GetCarVersionsByType(type, modelId, year);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return _carVersionsCache.GetCarVersionsByType(type, modelId, year);
            }
        }

        public Dictionary<int, List<CarVersionDetails>> GetVersionDetailsList(List<int> modelList, List<int> versionList, int cityid, bool orp = false, string type = "New")
        {
            var carResults = new Dictionary<int, List<CarVersionDetails>>();
            try
            {
                bool takeModels = modelList != null, takeVersions = versionList != null;
                if (takeModels)
                {
                    foreach (var model in modelList)
                    {
                        List<CarVersionEntity> versionsByModel = _carVersionsCache.GetCarVersionsByType(type, model);
                        var versionDetailsList = new List<CarVersionDetails>();
                        if (versionsByModel != null)
                        {
                            var versionids = versionsByModel.Select(src => src.ID).Distinct().ToList();
                            var versionsResult = _carVersionsCache.MultiGetVersionDetails(versionids);
                            var versionsPrices = _carPrices.GetVersionsPriceForSameModel(model, versionids, cityid, orp);

                            foreach (var version in versionids)
                            {
                                CarVersionDetails versionDetails;
                                versionsResult.TryGetValue(version, out versionDetails);
                                if (versionDetails != null)
                                {
                                    PriceOverview priceOverview;
                                    versionsPrices.TryGetValue(version, out priceOverview);
                                    versionDetails.PriceOverview = priceOverview ?? new PriceOverview();
                                }
                                versionDetailsList.Add(versionDetails);
                            }
                            versionDetailsList = versionDetailsList.AsEnumerable().OrderBy(d => d.PriceOverview.PriceStatus)
                                                    .GroupBy(d => d.PriceOverview.PriceStatus)
                                                    .SelectMany(g => g.OrderBy(b => b.PriceOverview.Price))
                                                    .ToList();

                        }
                        if (!carResults.ContainsKey(model))
                            carResults.Add(model, versionDetailsList);
                    }
                }
                else if (takeVersions)
                {
                    var versionsResult = _carVersionsCache.MultiGetVersionDetails(versionList);
                    var versionsPrices = _carPrices.GetVersionsPriceForDifferentModel(versionList, cityid, orp);
                    foreach (var version in versionList)
                    {
                        CarVersionDetails versionDetails;
                        versionsResult.TryGetValue(version, out versionDetails);
                        if (versionDetails != null)
                        {
                            PriceOverview priceOverview;
                            versionsPrices.TryGetValue(version, out priceOverview);
                            bool isPriceAvailable = priceOverview != null;
                            versionDetails.PriceOverview = isPriceAvailable ? priceOverview : new PriceOverview();
                            versionDetails.EmiInfo = (isPriceAvailable) ? _iPrice.GetEmiInformation(CustomParser.parseIntObject(priceOverview.Price)) : null;
                        }
                        if (!carResults.ContainsKey(version))
                            carResults.Add(version, new List<CarVersionDetails>() { versionDetails });
                        int platformId = CustomParser.parseIntObject(HttpContext.Current.Request.Headers["sourceid"]);
                        if (platformId == (int)Platform.CarwaleAndroid || platformId == (int)Platform.CarwaleiOS)
                        {
                            var carDataTrackingEntity = new CarDataTrackingEntity();
                            carDataTrackingEntity.ModelId = versionDetails.ModelId;
                            carDataTrackingEntity.Location.CityId = cityid;
                            carDataTrackingEntity.Platform = CustomParser.parseIntObject(HttpContext.Current.Request.Headers["sourceid"]);
                            carDataTrackingEntity.VersionId = version;
                            carDataTrackingEntity.Category = "VersionPage";
                            carDataTrackingEntity.Action = "VersionImpression";

                            _trackingBL.AppsTrackModelVersionImpression(carDataTrackingEntity, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarVersionsBL.GetVersionDetailsList()");
                objErr.SendMail();
            }
            return carResults;
        }

        public List<NewCarVersionsDTO> MapCarVersionDtoWithCarVersionEntity(int modelId, int cityId, bool isVersionsPassed = false, List<CarVersions> carVersionList = null)
        {

            List<CarVersions> versionData;
            List<NewCarVersionsDTO> versionDTOData = null;
            try
            {
                versionData = isVersionsPassed ? carVersionList : GetCarVersions(modelId, Status.New);
                versionDTOData = Mapper.Map<List<CarVersions>, List<NewCarVersionsDTO>>(versionData);
                Dictionary<int, Entity.Deals.DiscountSummary> versionDiscountByModel = new Dictionary<int, Entity.Deals.DiscountSummary>();
                if (cityId > 0)
                {
                    versionDiscountByModel = _dealsCache.BestVersionDealsByModel(modelId, cityId);
                }
                if (versionDiscountByModel != null && versionDiscountByModel.Count > 0)
                {
                    foreach (var vId in versionDTOData)
                    {
                        if (versionDiscountByModel.ContainsKey(vId.Id))
                        {
                            vId.DiscountSummary = Mapper.Map<Entity.Deals.DiscountSummary, DTOs.Deals.DiscountSummaryDTO>(versionDiscountByModel[vId.Id]);
                        }
                    }
                }
                //to get prices of all versions              
                List<int> versionList = versionData.Select(x => x.Id).ToList();
                var versionsPrice = _carPrices.GetVersionsPriceForSameModel(modelId, versionList, cityId, true);
                versionDTOData.ForEach(x => { x.CarPriceOverview = ((versionsPrice != null && versionsPrice.Count > 0 && versionsPrice[x.Id] != null) ? versionsPrice[x.Id] : new PriceOverview()); });
                versionDTOData.Sort(CompareVersionList);
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
            return versionDTOData;
        }

        public List<NewCarVersionsDTO> MapUpcomingVersionDTOWithEntity(int modelId, int cityId, bool isCityPage = false)
        {
            List<NewCarVersionsDTO> versionList;
            try
            {
                versionList = Mapper.Map<List<CarVersions>, List<NewCarVersionsDTO>>(
                                                      GetCarVersions(modelId, Status.Futuristic)
                                                      );
                var versionsPrice = _carPrices.GetVersionsPriceForSameModel(
                                                modelId,
                                                versionList.Select(x => x.Id).ToList(),
                                                cityId,
                                                isCityPage);
                if (versionsPrice != null && versionsPrice.Count > 0 && versionsPrice.Any(x => x.Value != null))
                {
                    versionList.ForEach(x =>
                    {
                        x.CarPriceOverview = (versionsPrice[x.Id] ?? new PriceOverview());
                    });

                    versionList.Sort(CompareVersionList);
                }
                else
                {
                    versionList = versionList.OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarVersionBL.MapUpcomingVersionDTOWithEntity");
                versionList = new List<NewCarVersionsDTO>();
            }
            return versionList;
        }

        /// <summary>
        /// Returns the Selected CarVersions' Details for Comparison
        /// </summary>
        /// <returns></returns>
        public List<CarVersionDetails> GetSelectedVersionDetails(string compareVersions)
        {
            try
            {
                List<CarVersionDetails> versionList = new List<CarVersionDetails>();
                List<string> arrVer = !string.IsNullOrEmpty(compareVersions) ? compareVersions.Split('|').ToList() : new List<string>();

                for (int i = 0; i < arrVer.Count; i++)
                {
                    int versionId;
                    int.TryParse(arrVer[i], out versionId);
                    CarVersionDetails versionDetails = versionId > 0 ? _carVersionsCache.GetVersionDetailsById(versionId) : null;

                    if (versionDetails != null)
                    {
                        versionList.Add(versionDetails);
                    }
                }

                return versionList;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "GetSelectedVersionDetails");
                return null;
            }
        }
        public static int CompareVersionList(NewCarVersionsDTO v1, NewCarVersionsDTO v2)
        {
            if (v1.CarPriceOverview.PriceStatus == v2.CarPriceOverview.PriceStatus)
            {
                if (v1.CarPriceOverview.Price != 0 && v2.CarPriceOverview.Price != 0)
                    return v1.CarPriceOverview.Price.CompareTo(v2.CarPriceOverview.Price);
                else if (v1.CarPriceOverview.Price == 0) return 1;
                return -1;
            }
            else if (v1.CarPriceOverview.PriceStatus == 0) return 1;
            else if (v2.CarPriceOverview.PriceStatus == 0) return -1;
            return v1.CarPriceOverview.PriceStatus.CompareTo(v2.CarPriceOverview.PriceStatus);
        }

        public VersionMaskingNameValidation FetchVersionInfoFromMaskingName(string modelMaskingName, string versionMaskingName, string modelId, string versionId, bool isMsite, string makeMaskingName = null)
        {
            int urlVersionId = CustomParser.parseIntObject(versionId);
            var versionValidation = new VersionMaskingNameValidation
            {
                VersionId = -1,
                ModelId = -1,
                IsRedirect = false,
                RedirectUrl = string.Empty,
                IsValid = true
            };
            ModelMaskingValidationEntity modelinfo = null;
            try
            {
                ICarModels _carModelBL = _unityContainer.Resolve<ICarModels>();
                modelinfo = _carModelBL.FetchModelIdFromMaskingName(modelMaskingName, modelId, makeMaskingName, isMsite);

                if (modelinfo.IsValid && modelinfo.ModelId > 0)
                {

                    VersionMaskingResponse vmr = new VersionMaskingResponse();
                    if (!string.IsNullOrEmpty(versionId) && RegExValidations.IsPositiveNumber(versionId))
                    {
                        versionValidation.VersionId = urlVersionId;
                    }
                    if (!string.IsNullOrEmpty(versionMaskingName))
                    {
                        vmr = _carVersionsCache.GetVersionInfoFromMaskingName(versionMaskingName, modelinfo.ModelId, versionValidation.VersionId);
                    }

                    if (vmr == null || vmr.ModelId <= 0 || vmr.VersionId <= 0 || !vmr.Valid)
                    {

                        if (!string.IsNullOrEmpty(makeMaskingName) && !string.IsNullOrEmpty(modelMaskingName))
                        {
                            versionValidation.IsRedirect = true;
                            versionValidation.RedirectUrl = ManageCarUrl.CreateModelUrl(makeMaskingName, modelMaskingName);
                        }
                        else
                        {
                            versionValidation.IsValid = false;
                        }
                    }
                    else
                    {
                        versionValidation.IsRedirect = urlVersionId > 0 || vmr.Redirect || modelinfo.IsRedirect;
                        versionValidation.RedirectUrl = versionValidation.IsRedirect ? ManageCarUrl.CreateVersionUrl(vmr.MakeName, modelinfo.ModelMaskingName, vmr.VersionMaskingName, isMsite)
                            : string.Empty;
                        versionValidation.ModelId = vmr.ModelId;
                        versionValidation.VersionId = vmr.VersionId;
                    }
                }
                else
                {
                    if (modelinfo.IsRedirect)
                    {
                        versionValidation.IsRedirect = true;
                        versionValidation.RedirectUrl = modelinfo.RedirectUrl;
                    }
                    else
                    {
                        versionValidation.IsValid = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return versionValidation;
        }

        public bool CheckTyresExists(int versionId)
        {
            var tyres = _tyresBl.GetTyresByCarVersion(versionId, 0, 10);
            return (tyres != null && tyres.Count > 0);
        }


        public List<List<Carwale.Entity.CompareCars.Color>> GetVersionsColors(List<int> versionIds)
        {
            List<List<Carwale.Entity.CompareCars.Color>> versionsColors = new List<List<Carwale.Entity.CompareCars.Color>>();
            try
            {
                foreach (var versionId in versionIds)
                {
                    var colors = _carVersionsCache.GetVersionColors(versionId);
                    versionsColors.Add(colors);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return versionsColors;
        }

    }

}
