using Carwale.DAL.ApiGateway;
using Carwale.Interfaces.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DAL.ApiGateway.Extensions.CarData;
using AutoMapper;
using Carwale.Notifications.Logs;
using Carwale.Entity;
using Google.Protobuf.Collections;
using System.Collections;
using System.Configuration;
using Carwale.Interfaces.SponsoredCar;
using Microsoft.Practices.Unity;
using Carwale.Entity.Enum;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Deals;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.BL.CMS;
using VehicleData.Service.ProtoClass;
using Carwale.Entity.ViewModels.CarData;


namespace Carwale.BL.CarData
{
    public class CarDataLogic : ICarDataLogic
    {
        private readonly IUnityContainer _container;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        public CarDataLogic(IUnityContainer container, ICarVersionCacheRepository carVersionsCacheRepo)
        {
            _container = container;
            _carVersionsCacheRepo = carVersionsCacheRepo;
        }
        public List<Entity.CarData.CarDataPresentation> GetCombinedCarData(List<int> versionIds)
        {
            try
            {
                List<Entity.CarData.CarDataPresentation> viewData = new List<Entity.CarData.CarDataPresentation>();
                IApiGatewayCaller _apiGatewayCaller = _container.Resolve<IApiGatewayCaller>();
                IApiGatewayCaller gatewayCaller = _apiGatewayCaller.GenerateCarDataCallerRequest(versionIds);
                if (gatewayCaller != null)
                {
                    _apiGatewayCaller.Call();
                    for (int i = 0; i < versionIds.Count; i++)
                    {
                        CarDataPresentation versionData = new CarDataPresentation();
                        var result = _apiGatewayCaller.GetResponse<VehicleDataList>(0);
                        if (result != null)
                        {
                            versionData.Specifications = Mapper.Map<RepeatedField<Category>, List<Entity.CarData.CarData>>(result.VehicleData[i].Specifications);
                            versionData.Features = Mapper.Map<RepeatedField<Category>, List<Entity.CarData.CarData>>(result.VehicleData[i].Features);
                            versionData.Overview = Mapper.Map<RepeatedField<Item>, List<Entity.CarData.CategoryItem>>(result.VehicleData[i].Overview);
                            viewData.Add(versionData);
                        }
                        else
                        {
                            viewData.Add(new Entity.CarData.CarDataPresentation());
                        }
                    }

                }
                return viewData;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error at GetCombinedCarData() function in CarData.cs in Business Layer");
                return null;
            }

        }


        public Entity.CarData.CarDataPresentation GetCombinedCarDataOldApp(List<int> versionIds)
        {
            try
            {
                Entity.CarData.CarDataPresentation viewData = new Entity.CarData.CarDataPresentation();
                IApiGatewayCaller _apiGatewayCaller = _container.Resolve<IApiGatewayCaller>();
                IApiGatewayCaller gatewayCaller = _apiGatewayCaller.GenerateCarDataCallerRequestOldApp(versionIds);
                if (gatewayCaller != null)
                {
                    _apiGatewayCaller.Call();
                    var result = _apiGatewayCaller.GetResponse<VehicleDataOldApp>(0);
                    if (result != null)
                    {
                        viewData.Specifications = Mapper.Map<RepeatedField<CategoryOldApp>, List<Entity.CarData.CarData>>(result.Specifications);
                        viewData.Features = Mapper.Map<RepeatedField<CategoryOldApp>, List<Entity.CarData.CarData>>(result.Features);
                        viewData.Overview = Mapper.Map<RepeatedField<ItemOldApp>, List<Entity.CarData.CategoryItem>>(result.Overview);
                    }
                }
                return viewData;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error at GetCombinedCarData() function in CarData.cs in Business Layer");
                return null;
            }
        }

        public List<Carwale.Entity.CarData.CarModelSpecs> GetCarModelSpecs(IEnumerable<int> versionIds, int modelId)
        {
            List<Carwale.Entity.CarData.CarModelSpecs> modelSpecs = null;
            try
            {
                IApiGatewayCaller _apiGatewayCaller = _container.Resolve<IApiGatewayCaller>();
                IApiGatewayCaller gatewayCaller = _apiGatewayCaller.GenerateModelSpecsRequest(versionIds, modelId);
                if (gatewayCaller != null)
                {
                    _apiGatewayCaller.Call();
                    modelSpecs = Mapper.Map<RepeatedField<SpecsSummary>, List<Entity.CarData.CarModelSpecs>>(_apiGatewayCaller.GetResponse<ModelSpecsSummaryList>(0).Values);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelSpecs;
        }

        //to do
        public ModelDataSummary GetCarModelDataSummary(List<int> versionIds, int modelId)
        {
            ModelDataSummary modelData = null;
            try
            {
                IApiGatewayCaller _apiGatewayCaller = _container.Resolve<IApiGatewayCaller>();
                IApiGatewayCaller gatewayCaller = _apiGatewayCaller.GenerateModelDataSummaryRequest(versionIds, modelId);
                if (gatewayCaller != null)
                {
                    _apiGatewayCaller.Call();
                    modelData = Mapper.Map<ModelSpecsSummary, ModelDataSummary>(_apiGatewayCaller.GetResponse<ModelSpecsSummary>(0));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelData;
        }

        public Carwale.Entity.CompareCars.FeaturedCarDataEntity GetFeaturedCar(Carwale.Entity.CompareCars.CompareCarInputParam inputVal)
        {
            try
            {
                ISponsoredCarBL _sponsoredBL = _container.Resolve<ISponsoredCarBL>();
                Carwale.Entity.CompareCars.FeaturedCarDataEntity featuredCar = null;
                featuredCar = _sponsoredBL.GetFeaturedCarData(string.Join(",", inputVal.VersionIds), (int)CampaignCategory.SponsoredCarComparison, inputVal.CustLocation.CityId);
                return featuredCar;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public List<CarWithImageEntity> GetVersionDetails(List<int> versionList, int featuredVersionId, Location custLocation, Platform type)
        {
            try
            {
                List<CarWithImageEntity> carDetails = new List<CarWithImageEntity>();
                ICarPriceQuoteAdapter _iPrice = _container.Resolve<ICarPriceQuoteAdapter>();
                IDealerAdProvider _dealerAd = _container.Resolve<IDealerAdProvider>();
                ICampaign _campaignBl = _container.Resolve<ICampaign>();
                var prices = _iPrice.GetVersionsPriceForDifferentModel(versionList, custLocation.CityId,true);
                Dictionary<int, bool> colorConditionDict = new Dictionary<int, bool>();
                foreach (var version in versionList)
                {
                    var versionDetails = AutoMapper.Mapper.Map<CarVersionDetails, CarWithImageEntity>(_carVersionsCacheRepo.GetVersionDetailsById(version));
                    PriceOverview price = default(PriceOverview);
                    if (prices.TryGetValue(version, out price) && price != null)
                    {
                        versionDetails.MinAvgPrice = price.Price;
                        versionDetails.PriceOverview = price;
                    }
                    if (versionDetails.PriceOverview == null) versionDetails.PriceOverview = new PriceOverview();
                    versionDetails.DiscountSummary = custLocation != null ? GetDiscountSummary(versionDetails.ModelId, custLocation.CityId, version, featuredVersionId) : new DealsStock();
                    CarIdEntity carEntity = new CarIdEntity()
                    {
                        ModelId = versionDetails.ModelId,
                        MakeId = versionDetails.MakeId,
                        VersionId = versionDetails.VersionId
                    };
                    if (custLocation != null && versionDetails.IsNew)
                    {
                        versionDetails.SponsoredCampaign = _dealerAd.GetDealerAd(carEntity, custLocation, (int)Platform.CarwaleDesktop, 1, -1, 0);
                        if (versionDetails.SponsoredCampaign == null)
                            versionDetails.ShowCampaignLink = _campaignBl.IsCityCampaignExist(carEntity.ModelId, custLocation, (int)type, (int)Application.CarWale);
                    }
                    SetColorsFlag(versionDetails, colorConditionDict);
                    carDetails.Add(versionDetails);
                }

                return carDetails;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private DealsStock GetDiscountSummary(int modelId, int cityId, int versionId, int featuredVersionId)
        {
            DealsStock discountSummary = new DealsStock();
            try
            {
                IDeals _cardeals = _container.Resolve<IDeals>();
                IDealsCache _carDealsCache = _container.Resolve<IDealsCache>();
                discountSummary = (_cardeals.IsShowDeals(cityId, true) && versionId != featuredVersionId) ? _carDealsCache.GetAdvantageAdContent(modelId, (cityId > 0 ? cityId : 0), 0, versionId) : null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return discountSummary ?? new DealsStock();
        }

        void SetColorsFlag(CarWithImageEntity cwie, Dictionary<int, bool> colorConditionDict)
        {
            try
            {
                ICarModelCacheRepository _modelCache = _container.Resolve<ICarModelCacheRepository>();
                int modelId = cwie.ModelId;
                if (!colorConditionDict.ContainsKey(modelId))
                {
                    colorConditionDict.Add(modelId,
                        CMSCommon.IsModelColorPhotosPresent(_modelCache.GetModelColorsByModel(modelId))
                        );
                }
                cwie.ShowAllColorsLink = colorConditionDict[modelId];
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "SetColorsFlag()");
            }
        }

        public bool CheckForCampaign(List<CarWithImageEntity> carDetails, Location location)
        {
            foreach (var car in carDetails)
            {
                if ((car.SponsoredCampaign != null && car.SponsoredCampaign.Campaign.Id > 0) || car.ShowCampaignLink || location.CityId <= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
