using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
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
    /// <summary>
    /// Created By:Shalini
    /// </summary>
    public class ModelPageAdapterAndroid : IServiceAdapter
    {
        private readonly int _modelId;
        private readonly ICarModels _carModelsBL;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICarVersions _carVersionsBL;
        private readonly IVideosBL _carModelVideos;
        private readonly INewCarDealers _newCarDealersBL;
        private readonly ICarPriceQuoteAdapter _carPrices;

        public ModelPageAdapterAndroid(IUnityContainer container, int modelId)
        {
            try
            {
                _modelId = modelId;
                _carModelsBL = container.Resolve<ICarModels>();
                _carModelsCacheRepo = container.Resolve<ICarModelCacheRepository>();
                _carVersionsBL = container.Resolve<ICarVersions>();
                _carModelVideos = container.Resolve<IVideosBL>();
                _newCarDealersBL = container.Resolve<INewCarDealers>();
                _carPrices = container.Resolve<ICarPriceQuoteAdapter>();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ModelPageAdapterAndroid()");
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
        private ModelPageDTO_Android GetModelPageDTOForAndroid()
        {
            var request = HttpContext.Current.Request;
            var cid = request.Headers["IMEI"];
            var modelDetails = _carModelsCacheRepo.GetModelDetailsById(_modelId);
            var simiarModels = (modelDetails.New || modelDetails.Futuristic) ? _carModelsBL.GetSimilarCarsByModel(_modelId, cid)
                : new List<SimilarCarModels>();
            var modelPageDTO = new ModelPageDTO_Android
            {
                /* Returns the Model Details */

                ModelDetails = Mapper.Map<CarModelDetailsDTO>(modelDetails),

                /* Returns the List of Model Colours */
                ModelColors = Mapper.Map<List<ModelColors>, List<ModelColorsDTO>>(_carModelsCacheRepo.GetModelColorsByModel(_modelId)),

                /*Returns the List of ModelVideos */
                ModelVideos = Mapper.Map<List<Video>, List<VideoDTO>>(GetModelVideos()),

                /*Returns the List of Similar Car Models*/
                SimilarCars = Mapper.Map<List<SimilarCarModels>, List<SimilarCarModelsDTO>>(simiarModels),
                
                /*Returns the List of New Car Versions */
                NewCarVersions = Mapper.Map<List<CarVersions>, List<CarVersionDTO>>(_carVersionsBL.GetCarVersions(_modelId, Status.All)),

                CallSlugNumber = _newCarDealersBL.CallSlugNumberByModelId(_modelId),

                DiscontinuedCarVersion = Mapper.Map<List<CarVersions>, List<CarVersionDTO>>(_carVersionsBL.GetCarVersions(_modelId, Status.Discontinued)),

            };

            modelPageDTO.ModelDetails.MaxPriceNew = Format.GetPrice(modelPageDTO.ModelDetails.MaxPrice.ToString());
            modelPageDTO.ModelDetails.MinPriceNew = Format.GetPrice(modelPageDTO.ModelDetails.MinPrice.ToString());
            modelPageDTO.ModelDetails.ModelRatingNew = Format.GetAbsReviewRate(Convert.ToDouble(modelPageDTO.ModelDetails.ModelRating.ToString()));
            modelPageDTO.ModelDetails.ExShowroomCity = ConfigurationManager.AppSettings["DefaultCityName"].ToString();
            modelPageDTO.ModelDetails.ExShowroomCityId = ConfigurationManager.AppSettings["DefaultCityId"].ToString();
            if (!string.IsNullOrEmpty(modelPageDTO.ModelDetails.MakeName))
                modelPageDTO.ModelDetails.shareUrl = "https://www.carwale.com/" + Format.FormatSpecial(modelPageDTO.ModelDetails.MakeName) + "-cars/" + modelPageDTO.ModelDetails.MaskingName + "/";

            modelPageDTO.ModelDetails.IsDiscontinuedCar = (modelPageDTO.ModelDetails.Futuristic == false && modelPageDTO.ModelDetails.New) == false ? true : false;

            if (modelPageDTO.NewCarVersions != null && modelPageDTO.NewCarVersions.Count > 0)
            {
                var versionList = modelPageDTO.NewCarVersions.Select(c => c.Id).ToList();
                var versionPrices = _carPrices.GetVersionsPriceForSameModel(modelPageDTO.ModelDetails.ModelId, versionList, Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCityId"].ToString()));
                for (int i = 0; i < modelPageDTO.NewCarVersions.Count; i++)
                {
                    var carversionprice = new PriceOverview();
                    versionPrices.TryGetValue(modelPageDTO.NewCarVersions[i].Id, out carversionprice);

                    modelPageDTO.NewCarVersions[i].MinPriceNew = carversionprice != null && carversionprice.PriceStatus == (int)PriceBucket.HaveUserCity ? Format.GetPrice(carversionprice.Price.ToString()) : Format.GetPrice("0");
                    modelPageDTO.NewCarVersions[i].ExShowRoomCityId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCityId"].ToString());
                    modelPageDTO.NewCarVersions[i].ExShowRoomCityName = ConfigurationManager.AppSettings["DefaultCityName"].ToString();
                }
            }

            foreach (var version in modelPageDTO.DiscontinuedCarVersion)
            {
                version.ExShowRoomCityId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCityId"].ToString());
                version.ExShowRoomCityName = ConfigurationManager.AppSettings["DefaultCityName"].ToString();
            }

            for (int i = 0; i < modelPageDTO.SimilarCars.Count; i++)
            {
                modelPageDTO.SimilarCars[i].ReviewRateNew = Format.GetAbsReviewRate(Convert.ToDouble(modelPageDTO.SimilarCars[i].ReviewRate));
                modelPageDTO.SimilarCars[i].MinPriceNew = Format.GetPrice(modelPageDTO.SimilarCars[i].MinPrice.ToString());
                modelPageDTO.SimilarCars[i].ExShowRoomCityId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCityId"].ToString());
                modelPageDTO.SimilarCars[i].ExShowRoomCityName = ConfigurationManager.AppSettings["DefaultCityName"].ToString();
                modelPageDTO.SimilarCars[i].SmallPic = modelPageDTO.SimilarCars[i].HostUrl.ToString() + Utility.ImageSizes._110X61 + modelPageDTO.SimilarCars[i].ModelImageOriginal;
                modelPageDTO.SimilarCars[i].LargePic = modelPageDTO.SimilarCars[i].HostUrl.ToString() + Utility.ImageSizes._210X118 + modelPageDTO.SimilarCars[i].ModelImageOriginal;
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
