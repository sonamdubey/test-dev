using Carwale.BL.GrpcFiles;
using Carwale.DAL.ApiGateway;
using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.Deals;
using Carwale.Entity.Enum;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.UserProfiling;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using EditCMSWindowsService.Messages;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carwale.Entity.ViewModels.CarData;
using Carwale.DAL.ApiGateway.Extensions;

using Carwale.Interfaces.CMS;

namespace Carwale.BL.NewCars
{
    public class MakePageAdapterMobile : IServiceAdapterV2
    {
        public MakePageAdapterMobile(ICarMakes carMakesBl, ICarModelCacheRepository carModelsCacheRepo,
        ICarMakesCacheRepository carMakesCacheRepo, IDeals carDeals, IDealsCache carDealsCache, IUserProfilingBL userProfilingBl, IPhotos photosBl, IMediaBL mediaBl, ICarVersionCacheRepository carVersionCacheRepo)
        {
            try
            {
                _carMakesCacheRepo = carMakesCacheRepo;
                _carDeals = carDeals;
                _carDealsCache = carDealsCache;
                _userProfilingBL = userProfilingBl;
                _carMakesBL = carMakesBl;
                _carModelsCacheRepo = carModelsCacheRepo;
                _photosBl = photosBl;
                _carVersionCacheRepo = carVersionCacheRepo;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MakePageAdapterMobile Dependency Resolution failed");
            }
        }
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly IDeals _carDeals;
        private readonly IDealsCache _carDealsCache;
        private readonly IUserProfilingBL _userProfilingBL;
        private readonly ICarMakes _carMakesBL;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICarVersionCacheRepository _carVersionCacheRepo;
        private readonly IPhotos _photosBl;
        private const int _requiredImageCount = 6;
        private const int _requiredModelCount = 6;
        private const uint _startIndex = 1;
        private const uint _endIndex = 6;

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetMakePageDTOForMobile<U>(input), typeof(T));
        }
        private MakePageDTO_Mobile GetMakePageDTOForMobile<U>(U input)
        {
            try
            {
                var makeDTO = new MakePageDTO_Mobile();
                MakePageInputParam inputParam = (MakePageInputParam)Convert.ChangeType(input, typeof(U));
                if (inputParam != null)
                {
                    List<ModelSummary> NewCarModelsDetails = null;
                    if (_carMakesBL != null)
                    {
                        NewCarModelsDetails = _carMakesBL.GetActiveModelsWithDetails(inputParam.CityId, inputParam.MakeId, true);
                    }
                    Dictionary<CarBodyStyle, Tuple<int[], string>> carRanksByBodyType = null;
                    if (_carModelsCacheRepo != null)
                    {
                        carRanksByBodyType = _carModelsCacheRepo.GetCarRanksByBodyType(ConfigurationManager.AppSettings["BestCarsBodyTypes"] ?? "1,3,6,10", CWConfiguration.TopCarByBodyTypeCount);
                    }
                    //to get discount summary using multiget
                    if (_carDeals != null && NewCarModelsDetails != null && _carDeals.IsShowDeals(inputParam.CityId, true))
                    {
                        Dictionary<int, DealsStock> deals = _carDealsCache.GetAdvantageAdContentV1(NewCarModelsDetails.Select(x => x.ModelId).ToList(), inputParam.CityId);
                        NewCarModelsDetails.ForEach(x =>
                        {
                            var carDeal = deals.ContainsKey(x.ModelId) ? deals[x.ModelId] : null; x.DiscountSummary = carDeal;
                        });
                    }

                    makeDTO = new MakePageDTO_Mobile()
                    {
                        NewCarModels = AutoMapper.Mapper.Map<List<ModelSummary>, List<CarModelSummaryDTOV2>>(NewCarModelsDetails),
                        MakeDetails = _carMakesCacheRepo.GetCarMakeDetails(inputParam.MakeId)
                    };
                    var apiGatewayCaller = new ApiGatewayCaller();
                    ManageCmsCalls(apiGatewayCaller, NewCarModelsDetails, makeDTO, inputParam.MakeId);

                    if (carRanksByBodyType != null && makeDTO.NewCarModels != null && makeDTO.NewCarModels.Count > 0)
                    {
                        makeDTO.NewCarModels.ForEach(x =>
                        {
                            if (carRanksByBodyType.ContainsKey(x.BodyStyleId))
                            {
                                int[] currentBodyStyleRanks = carRanksByBodyType[x.BodyStyleId].Item1;
                                x.NoOfTopCarsInBodyType = currentBodyStyleRanks.Length;
                                int rank = Array.IndexOf(currentBodyStyleRanks, x.ModelId);
                                if (rank >= 0)
                                {
                                    x.Rank = rank + 1;
                                }
                            }
                            x.VersionCount = x.New ? _carVersionCacheRepo.GetVersionCountByModel(x.ModelId) : 0;
                        });
                    }
                    if (makeDTO != null && makeDTO.MakeDetails != null)
                    {
                        string formattedMakeName = Format.FormatSpecial(makeDTO.MakeDetails.MakeName);
                        makeDTO.MetaData = new PageMetaTags
                        {
                            Title = string.Format("{0} Cars in India - Prices (GST Rates), Reviews, Photos & More - CarWale", System.Text.RegularExpressions.Regex.Replace(makeDTO.MakeDetails.MakeName, "maruti suzuki", "Maruti", System.Text.RegularExpressions.RegexOptions.IgnoreCase)),
                            Description = string.Format("{0} cars in India. Know everything you want to know about {0} car models. CarWale offers {0} history, reviews, photos and news etc. Find {0} dealers, participate in {0} discussions and know upcoming cars", makeDTO.MakeDetails.MakeName),
                            Canonical = ManageCarUrl.CreateMakeUrl(makeDTO.MakeDetails.MakeName, false, true),
                            Amphtml = $"https://www.carwale.com/m/{formattedMakeName}-cars/amp/",
                        };
                    }

                }
                if (makeDTO != null && makeDTO.NewCarModels.IsNotNullOrEmpty())
                {
                    foreach (var model in makeDTO.NewCarModels)
                    {
                        if (!model.New)
                        {
                            model.CarPriceOverview.PricePrefix = ConfigurationManager.AppSettings["MakePageUpcomingPriceText"];
                            model.CarPriceOverview.PriceLabel = string.Empty;
                        }
                    }
                }
                return makeDTO;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MakePageAdapterMobile.GetMakePageDTOForMobile()");
            }
            return null;
        }

        private void GetCarousalImages(IApiGatewayCaller apiGatewayCaller, MakePageDTO_Mobile makeDTO)
        {
            try
            {
                var cmsImagesResponse = apiGatewayCaller.GetResponse<GrpcModelsImageList>(1);
                List<CMSImage> modelImages = new List<CMSImage>();
                if (cmsImagesResponse != null)
                {
                    modelImages = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(cmsImagesResponse);
                }

                makeDTO.Images = new ImageCarousal()
                {
                    ModelPhotos = AutoMapper.Mapper.Map<List<CMSImage>, List<ModelImageCarousal>>(modelImages)
                };
                _photosBl.GetMakeImageGallary(makeDTO.Images.ModelPhotos);
                makeDTO.Images.ModelPhotos.ForEach(x => x.ModelImagePageUrl = ManageCarUrl.CreateImageListingPageUrl(x.MakeName, x.ModelMaskingName, true));
                makeDTO.Images.LandingUrl = ManageCarUrl.CreateMakeImagePageUrl(makeDTO.Images.MakeName, true);
                makeDTO.Images.Title = string.Format("{0} Images", makeDTO.Images.MakeName);
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MakePageAdapterMobile.GetCarousalImages()");
            }
        }

        private void GetVideos(IApiGatewayCaller apiGatewayCaller, MakePageDTO_Mobile makeDTO)
        {
            try
            {
                var cmsVideosResponse = apiGatewayCaller.GetResponse<GrpcVideosList>(0);

                if (cmsVideosResponse != null)
                {
                    makeDTO.MakeVideos = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(cmsVideosResponse);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MakePageAdapterMobile.GetVideos()");
            }
        }

        private void ManageCmsCalls(IApiGatewayCaller apiGatewayCaller, List<ModelSummary> newCarModels, MakePageDTO_Mobile makeDto, int makeId)
        {
            apiGatewayCaller.GetMakeVideos(makeId, Application.CarWale, _startIndex, _endIndex);

            var imagesCallAdded = false;
            if (newCarModels.IsNotNullOrEmpty())
            {
                var modelIds = newCarModels.FindAll(x => x.New).Select(item => item.ModelId).Take(_requiredModelCount).ToList().ToDelimatedString(',');
                apiGatewayCaller.GetModelsImages(modelIds, _requiredImageCount, Application.CarWale);
                imagesCallAdded = true;
            }

            apiGatewayCaller.Call();

            GetVideos(apiGatewayCaller, makeDto);
            if (imagesCallAdded)
            {
                GetCarousalImages(apiGatewayCaller, makeDto);
            }
        }
    }
}
