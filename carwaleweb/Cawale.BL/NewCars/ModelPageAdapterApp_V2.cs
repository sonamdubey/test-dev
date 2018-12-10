using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.OffersV1;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.BL.NewCars
{
    public class ModelPageAdapterApp_V2 : IServiceAdapterV2
    {
        private readonly ICarModels _carModelsBL;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICarVersions _carVersionsBL;
        private readonly IVideosBL _carModelVideos;
        private readonly INewCarDealers _newCarDealersBL;
        private readonly ICarPriceQuoteAdapter _carprice;
        private readonly ICarMileage _carMileage;
        private readonly IPrices _prices;
        private readonly IPQGeoLocationBL _geoLocationBL;
        private readonly IOffersAdapter _offersAdapter;

        public ModelPageAdapterApp_V2(IUnityContainer container, ICarPriceQuoteAdapter carprice, ICarModels carModelsBL, ICarMileage carMileage,
            ICarModelCacheRepository carModelsCacheRepo, ICarVersions carVersionsBL, IVideosBL carModelVideos,
            INewCarDealers newCarDealersBL, IPrices prices, IPQGeoLocationBL geoLocationBL, IOffersAdapter offersAdapter)
        {
            try
            {
                _carprice = carprice;
                _carModelsBL = carModelsBL;
                _carModelsCacheRepo = carModelsCacheRepo;
                _carVersionsBL = carVersionsBL;
                _carModelVideos = carModelVideos;
                _newCarDealersBL = newCarDealersBL;
                _carMileage = carMileage;
                _prices = prices;
                _geoLocationBL = geoLocationBL;
                _offersAdapter = offersAdapter;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ModelPageAdapterAndroid_V1()");
                objErr.LogException();
            }
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetModelPageDtoForApp(input), typeof(T));
        }


        /// <summary>
        /// Returns the complete details 
        /// </summary>
        /// <returns></returns>
        private ModelPageDTOApp_V2 GetModelPageDtoForApp<U>(U input)
        {
            ModelPageDTOApp_V2 modelPageDTO = null;
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));
                var modelDetails = _carModelsCacheRepo.GetModelDetailsById(inputParam.ModelDetails.ModelId);
                var versionDetailsList = _carVersionsBL.GetCarVersions(inputParam.ModelDetails.ModelId, Status.New);
                var request = HttpContext.Current.Request;
                var cid = request.Headers["IMEI"];
                var similarCars = (modelDetails.New || modelDetails.Futuristic) ? _carModelsBL.GetSimilarCarsByModel(inputParam.ModelDetails.ModelId, cid) : new List<SimilarCarModels>();

                modelPageDTO = new ModelPageDTOApp_V2()
                {
                    /* Returns the Model Details */

                    ModelDetails = Mapper.Map<CarModelDetails, CarModelDetailsDtoV2>(modelDetails),

                    /* Returns the List of Model Colours */
                    ModelColors = Mapper.Map<List<ModelColors>, List<ModelColorsDTO>>(_carModelsCacheRepo.GetModelColorsByModel(inputParam.ModelDetails.ModelId)),

                    /*Returns the List of ModelVideos */
                    ModelVideos = Mapper.Map<List<Video>, List<VideoDTO>>(_carModelVideos.GetVideosByModelId(inputParam.ModelDetails.ModelId, CMSAppId.Carwale, 0, 0)),

                    /*Returns the List of Similar Car Models*/
                    SimilarCars = Mapper.Map<List<SimilarCarModels>, List<SimilarCarModelsDtoV3>>(similarCars),

                    /*Returns the List of New Car Versions */
                    NewCarVersions = Mapper.Map<List<CarVersions>, List<CarVersionDtoV3>>(versionDetailsList),

                    CallSlugNumber = _newCarDealersBL.CallSlugNumberByModelId(inputParam.ModelDetails.ModelId),

                    MileageData = Mapper.Map<List<MileageDataEntity>, List<MileageDataDTO_V1>>(_carMileage.GetMileageData(versionDetailsList)),

                    OrpText = inputParam.CustLocation.CityId <= 0 ? ConfigurationManager.AppSettings["ShowPriceInCityText"] : string.Empty,

                    City = inputParam.CustLocation.CityId > 0 ? Mapper.Map<City, CityDTO>(_geoLocationBL.GetCityById(inputParam.CustLocation.CityId)) : new CityDTO(),
                };

                if (modelPageDTO.ModelDetails != null)
                {
                    modelPageDTO.ModelDetails.ThreeSixtyAvailability = new ThreeSixtyAvailabilityDTO()
                    {
                        Is360ExteriorAvailable = modelDetails.Is360ExteriorAvailable,
                        Is360InteriorAvailable = modelDetails.Is360InteriorAvailable,
                        Is360OpenAvailable = modelDetails.Is360OpenAvailable,
                    };


                }
                modelPageDTO.ModelDetails.ShareUrl = ManageCarUrl.CreateModelUrl(modelPageDTO.ModelDetails.MakeName, modelPageDTO.ModelDetails.MaskingName, true);
                modelPageDTO.ModelDetails.IsDiscontinuedCar = (!modelPageDTO.ModelDetails.Futuristic && !modelDetails.New);
                modelPageDTO.ModelDetails.ModelRating = Format.GetAbsReviewRate(Convert.ToDouble(modelPageDTO.ModelDetails.ModelRating));
                for (int i = 0; i < modelPageDTO.SimilarCars.Count; i++)
                {
                    modelPageDTO.SimilarCars[i].ReviewRateNew = Format.GetAbsReviewRate(Convert.ToDouble(similarCars[i].ReviewRate));
                }

                //get prices for model,similarcars and versions
                modelPageDTO = GetPriceForModels(modelPageDTO, inputParam, modelDetails.MinPrice.ToString());
                //get prices for versions
                modelPageDTO = GetPriceForVersions(modelPageDTO, inputParam);
                if (modelPageDTO.ModelDetails != null && modelPageDTO.ModelDetails.New && modelPageDTO.NewCarVersions != null)
                {
                    var versionIndex = modelPageDTO.NewCarVersions.First(x => x.PriceOverview.PriceForSorting > 0);
                    modelPageDTO.ModelDetails.VersionId = versionIndex.Id;
                    modelPageDTO.ModelDetails.VersionName = versionIndex.Version;
                    var offerInput = SetOfferInputs(modelDetails, versionIndex.Id, inputParam.CustLocation);
                    modelPageDTO.Offer = Mapper.Map<OfferDto>(_offersAdapter.GetOffers(offerInput));
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return modelPageDTO;
        }

        private OfferInput SetOfferInputs(CarModelDetails modelDetails, int versionId, Location cityDetails)
        {
            var offerInput = new OfferInput
            {
                ApplicationId = (int)Application.CarWale,
                MakeId = modelDetails.MakeId,
                ModelId = modelDetails.ModelId,
                VersionId = versionId
            };

            if (cityDetails == null || cityDetails.CityId < 1)
            {
                offerInput.CityId = -1;
                offerInput.StateId = -1;
            }
            else
            {
                offerInput.CityId = cityDetails.CityId;
                offerInput.StateId = cityDetails.StateId;
            }

            return offerInput;
        }

        private ModelPageDTOApp_V2 GetPriceForModels(ModelPageDTOApp_V2 modelPageDTO, CarDataAdapterInputs inputParam, string minPrice)
        {
            try
            {
                var modelList = modelPageDTO.SimilarCars != null ? modelPageDTO.SimilarCars.Select(d => d.ModelId).Distinct().ToList() : new List<int>();
                if (modelPageDTO.ModelDetails.New)
                {
                    modelList.Add(inputParam.ModelDetails.ModelId);
                }
                else
                {
                    modelPageDTO.ModelDetails.PriceOverview = new PriceOverviewDtoV3();
                    modelPageDTO.ModelDetails.PriceOverview.Price = ConfigurationManager.AppSettings["rupeeSymbol"] + Format.PriceLacCr(minPrice.ToString());
                    modelPageDTO.ModelDetails.PriceOverview.PricePrefix = ConfigurationManager.AppSettings["DiscontinuePriceText"] ?? string.Empty;
                }

                if (modelList != null && modelList.Count > 0)
                {
                    var carModelPrices = _carprice.GetModelsCarPriceOverview(modelList, inputParam.CustLocation.CityId, true);
                    if (carModelPrices != null)
                    {
                        modelPageDTO.SimilarCars.ForEach(cm =>
                        {
                            PriceOverview versionPriceOverview;
                            carModelPrices.TryGetValue(cm.ModelId, out versionPriceOverview);
                            cm.PriceOverview = Mapper.Map<PriceOverviewDtoV3>(versionPriceOverview != null ? versionPriceOverview : new PriceOverview());
                        });
                    }
                    if (modelPageDTO.ModelDetails.New && carModelPrices != null)
                    {
                        PriceOverview priceOverview = null;
                        carModelPrices.TryGetValue(inputParam.ModelDetails.ModelId, out priceOverview);
                        bool isPriceAvailable = priceOverview != null;
                        modelPageDTO.PriceBreakUpText = (isPriceAvailable
                                                            && inputParam.CustLocation.CityId > 0
                                                            && priceOverview.PriceStatus == (int)PriceBucket.HaveUserCity)
                                                        ? ConfigurationManager.AppSettings["ViewPriceBreakupText"] ?? string.Empty
                                                        : string.Empty;
                        modelPageDTO.ModelDetails.PriceOverview = Mapper.Map<PriceOverviewDtoV3>(priceOverview != null ? priceOverview : new PriceOverview());
                        modelPageDTO.EmiInfo = Mapper.Map<EMIInformation, EmiInformationDtoV2>((isPriceAvailable) ? _prices.GetEmiInformation(CustomParser.parseIntObject(priceOverview.Price)) : null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelPageDTO;
        }

        private ModelPageDTOApp_V2 GetPriceForVersions(ModelPageDTOApp_V2 modelPageDTO, CarDataAdapterInputs inputParam)
        {
            try
            {
                var versionList = modelPageDTO.NewCarVersions != null ? modelPageDTO.NewCarVersions.Select(d => d.Id).Distinct().ToList() : null;
                if (versionList != null && versionList.Count > 0)
                {
                    var carVersionsPrice = _carprice.GetVersionsPriceForSameModel(inputParam.ModelDetails.ModelId, versionList, inputParam.CustLocation.CityId, true);
                    if (carVersionsPrice != null)
                    {
                        modelPageDTO.NewCarVersions.ForEach(cv =>
                        {
                            PriceOverview versionPriceOverview;
                            carVersionsPrice.TryGetValue(cv.Id, out versionPriceOverview);
                            cv.PriceOverview = Mapper.Map<PriceOverviewDtoV3>(versionPriceOverview != null ? versionPriceOverview : new PriceOverview());
                        });
                        modelPageDTO.NewCarVersions = modelPageDTO.NewCarVersions.AsEnumerable().OrderBy(d => d.PriceOverview.PriceStatus)
                                                    .GroupBy(d => d.PriceOverview.PriceStatus)
                                                    .SelectMany(g => g.OrderBy(b => b.PriceOverview.PriceForSorting))
                                                    .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelPageDTO;
        }
    }
}
