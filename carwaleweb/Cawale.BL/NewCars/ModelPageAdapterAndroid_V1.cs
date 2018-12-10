using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.BL.NewCars
{
    public class ModelPageAdapterAndroid_V1 : IServiceAdapter
    {
        private readonly int _modelId;
        private readonly int _cityId;
        private readonly ICarModels _carModelsBL;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICarVersions _carVersionsBL;
        private readonly IVideosBL _carModelVideos;
        private readonly INewCarDealers _newCarDealersBL;
        private readonly ICarPriceQuoteAdapter _carprice;
        private readonly ICarMileage _carMileage;
        private readonly IPrices _prices;


        public ModelPageAdapterAndroid_V1(IUnityContainer container, ICarPriceQuoteAdapter carprice, ICarModels carModelsBL, ICarMileage carMileage,
            ICarModelCacheRepository carModelsCacheRepo, ICarVersions carVersionsBL, IVideosBL carModelVideos,
            INewCarDealers newCarDealersBL,
            int modelId, CustLocation custLocation, IPrices prices)
        {
            try
            {
                _modelId = modelId;
                _cityId = custLocation.CityId;
                _carprice = carprice;
                _carModelsBL = carModelsBL;
                _carModelsCacheRepo = carModelsCacheRepo;
                _carVersionsBL = carVersionsBL;
                _carModelVideos = carModelVideos;
                _newCarDealersBL = newCarDealersBL;
                _carMileage = carMileage;
                _prices = prices;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ModelPageAdapterAndroid_V1()");
                objErr.LogException();
            }
        }
        public T Get<T>()
        {
            return (T)Convert.ChangeType(GetModelPageDTOForAndroid(), typeof(T));
        }

        /// <summary>
        /// Returns the complete details 
        /// </summary>
        /// <returns></returns>
        private ModelPageDTO_Android_V1 GetModelPageDTOForAndroid()
        {
            ModelPageDTO_Android_V1 modelPageDTO;
            try
            {
                var modelDetails = _carModelsCacheRepo.GetModelDetailsById(_modelId);
                var versionDetailsList = _carVersionsBL.GetCarVersions(_modelId, Status.New);
                var request = HttpContext.Current.Request;
                var cid = request.Headers["IMEI"];
                var similarCars = (modelDetails.New || modelDetails.Futuristic) ? _carModelsBL.GetSimilarCarsByModel(_modelId, cid) : new List<SimilarCarModels>();

                modelPageDTO = new ModelPageDTO_Android_V1()
                {
                    /* Returns the Model Details */

                    ModelDetails = Mapper.Map<CarModelDetailsDTO_V1>(modelDetails),

                    /* Returns the List of Model Colours */
                    ModelColors = Mapper.Map<List<ModelColors>, List<ModelColorsDTO>>(_carModelsCacheRepo.GetModelColorsByModel(_modelId)),

                    /*Returns the List of ModelVideos */
                    ModelVideos = Mapper.Map<List<Video>, List<VideoDTO>>(GetModelVideos()),

                    /*Returns the List of Similar Car Models*/
                    SimilarCars = Mapper.Map<List<SimilarCarModels>, List<SimilarCarModelsDTO_V1>>(similarCars),

                    /*Returns the List of New Car Versions */
                    NewCarVersions = Mapper.Map<List<CarVersions>, List<CarVersionDTO_V1>>(versionDetailsList),

                    CallSlugNumber = _newCarDealersBL.CallSlugNumberByModelId(_modelId),

                    DiscontinuedCarVersion = Mapper.Map<List<CarVersions>, List<CarVersionDTO_V1>>(_carVersionsBL.GetCarVersions(_modelId, Status.Discontinued)),

                    MileageData = Mapper.Map<List<MileageDataEntity>, List<MileageDataDTO_V1>>(_carMileage.GetMileageData(versionDetailsList))
                };
                if (modelPageDTO.ModelDetails != null)
                {
                    modelPageDTO.ModelDetails.ThreeSixtyAvailability = new ThreeSixtyAvailabilityDTO()
                    {
                        Is360ExteriorAvailable = modelDetails.Is360ExteriorAvailable,
                        Is360InteriorAvailable = modelDetails.Is360InteriorAvailable,
                        Is360OpenAvailable = modelDetails.Is360OpenAvailable
                    };
                }
                if (!string.IsNullOrEmpty(modelPageDTO.ModelDetails.MakeName))
                {
                    modelPageDTO.ModelDetails.ShareUrl = "https://www.carwale.com/" + Format.FormatSpecial(modelPageDTO.ModelDetails.MakeName) + "-cars/" + modelPageDTO.ModelDetails.MaskingName + "/";
                }
                modelPageDTO.ModelDetails.IsDiscontinuedCar = (!modelPageDTO.ModelDetails.Futuristic && !modelDetails.New);
                modelPageDTO.ModelDetails.ModelRating = Format.GetAbsReviewRate(Convert.ToDouble(modelPageDTO.ModelDetails.ModelRating));

                var versionList = versionDetailsList != null ? versionDetailsList.Select(d => d.Id).Distinct().ToList() : null;
                var modelList = similarCars != null ? similarCars.Select(d => d.ModelId).Distinct().ToList() : new List<int>();
                if (modelDetails.New)
                    modelList.Add(_modelId);

                var carModelPrices = _carprice.GetModelsCarPriceOverview(modelList, _cityId);
                var carVersionsPrice = _carprice.GetVersionsPriceForSameModel(_modelId, versionList, _cityId);

                if (modelDetails.New)
                {
                    if (carModelPrices != null)
                    {
                        PriceOverview priceOverview = null;
                        carModelPrices.TryGetValue(_modelId, out priceOverview);
                        bool isPriceAvailable = priceOverview != null;
                        modelPageDTO.ModelDetails.PriceOverview = Mapper.Map<PriceOverviewDTO>(priceOverview != null ? priceOverview : new PriceOverview());
                        modelPageDTO.EmiInfo = Mapper.Map<EMIInformation, EMIInformationDTO>((isPriceAvailable) ? _prices.GetEmiInformation(CustomParser.parseIntObject(priceOverview.Price)) : null);
                    }
                }
                else
                {
                    modelPageDTO.ModelDetails.PriceOverview = new PriceOverviewDTO();
                    modelPageDTO.ModelDetails.PriceOverview.Price = ConfigurationManager.AppSettings["rupeeSymbol"] + Carwale.Utility.Format.PriceLacCr(modelDetails.MinPrice.ToString());
                    modelPageDTO.ModelDetails.PriceOverview.PricePrefix = ConfigurationManager.AppSettings["DiscontinuePriceText"] ?? string.Empty;
                }

                for (int i = 0; i < modelPageDTO.SimilarCars.Count; i++)
                {
                    modelPageDTO.SimilarCars[i].ReviewRateNew = Format.GetAbsReviewRate(Convert.ToDouble(similarCars[i].ReviewRate));
                }

                if (carVersionsPrice != null)
                {
                    modelPageDTO.NewCarVersions.ForEach(cv =>
                    {
                        PriceOverview versionPriceOverview; carVersionsPrice.TryGetValue(cv.Id, out versionPriceOverview);
                        cv.PriceOverview = Mapper.Map<PriceOverviewDTO>(versionPriceOverview != null ? versionPriceOverview : new PriceOverview());
                    });
                    modelPageDTO.NewCarVersions = modelPageDTO.NewCarVersions.AsEnumerable().OrderBy(d => d.PriceOverview.PriceStatus)
                                                .GroupBy(d => d.PriceOverview.PriceStatus)
                                                .SelectMany(g => g.OrderBy(b => b.PriceOverview.PriceForSorting))
                                                .ToList();
                }

                if (carModelPrices != null)
                {
                    modelPageDTO.SimilarCars.ForEach(cm =>
                    {
                        PriceOverview versionPriceOverview; carModelPrices.TryGetValue(cm.ModelId, out versionPriceOverview);
                        cm.PriceOverview = Mapper.Map<PriceOverviewDTO>(versionPriceOverview != null ? versionPriceOverview : new PriceOverview());
                    });
                }

                foreach (var version in modelPageDTO.DiscontinuedCarVersion)
                {
                    version.PriceOverview = Mapper.Map<PriceOverviewDTO>(new PriceOverview());
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ModelPageAdapterAndroid_V1.GetModelPageDTOForAndroid()");
                objErr.LogException();
                modelPageDTO = new ModelPageDTO_Android_V1();
            }

            return modelPageDTO;
        }

        /// <summary>
        /// Returns Videos for the Model
        /// </summary>
        /// <returns></returns>
        private List<Video> GetModelVideos()
        {
            return _carModelVideos.GetVideosByModelId(_modelId, CMSAppId.Carwale, 0, 0);
        }
    }
}
