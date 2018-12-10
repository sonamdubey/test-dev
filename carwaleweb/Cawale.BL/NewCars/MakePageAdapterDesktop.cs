using Carwale.BL.GrpcFiles;
using Carwale.DAL.ApiGateway;
using Carwale.DTOs.NewCars;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.Deals;
using Carwale.Entity.Enum;
using Carwale.Entity.ViewModels;
using Carwale.Entity.ViewModels.CarData;
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
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Carwale.Interfaces.CMS;
using EditCMSWindowsService.Messages;
using Google.Protobuf;
using Carwale.DTOs.CarData;
using Carwale.DAL.ApiGateway.Extensions;

namespace Carwale.BL.NewCars
{
    /// <summary>
    /// Created By: Shalini 
    /// </summary>
    public class MakePageAdapterDesktop : IServiceAdapterV2
    {                 
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly ICarMakes _carMakesBL;                    
        private readonly IDeals _cardeals;
        private readonly IDealsCache _carDealsCache;
        private readonly IPhotos _photosBl;
        private const int _requiredImageCount = 6;
        private const int _requiredModelCount = 6;
        private readonly IUserProfilingBL _userProfilingBL;
        private readonly uint _startIndex = 1;
        private readonly uint _endIndex = 7;    
        public MakePageAdapterDesktop(IUnityContainer container)
        {
            try
            {                                      
                _carMakesBL = container.Resolve<ICarMakes>();               
                _carMakesCacheRepo = container.Resolve<ICarMakesCacheRepository>();                                          
                _cardeals = container.Resolve<IDeals>();
                _carDealsCache = container.Resolve<IDealsCache>();
                _photosBl = container.Resolve<IPhotos>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MakePageAdapterDesktop");                
            }
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetMakePageDTOForDesktop<U>(input), typeof(T));
        }
      
        private MakePageDTO_Desktop GetMakePageDTOForDesktop<U>(U input)
        {
            var makeDTO = new MakePageDTO_Desktop();
            try
            {
                MakePageInputParam inputParam = (MakePageInputParam)Convert.ChangeType(input, typeof(U));                
                var makeDetails = _carMakesCacheRepo.GetCarMakeDetails(inputParam.MakeId) ?? new CarMakeEntityBase();
                var pageMetaTags = _carMakesCacheRepo.GetMakePageMetaTags(inputParam.MakeId, Convert.ToInt16(Pages.MakePageId)) ?? new PageMetaTags();
                List<ModelSummary> NewCarModelsDetails = _carMakesBL.GetActiveModelsWithDetails(inputParam.CityId, inputParam.MakeId, true);
                         

                //to get discount summary using multiget
                if (_cardeals.IsShowDeals(inputParam.CityId, true))
                {
                    List<int> modellist = NewCarModelsDetails.Select(x => x.ModelId).ToList();
                    Dictionary<int, DealsStock> deals = _carDealsCache.GetAdvantageAdContentV1(modellist, inputParam.CityId) ?? new Dictionary<int, DealsStock>();
                    NewCarModelsDetails.ForEach(x => { var carDeal = deals.ContainsKey(x.ModelId) ? deals[x.ModelId] : null; x.DiscountSummary = carDeal; });
                }

                makeDTO = new MakePageDTO_Desktop()
                {

                    /********* Returns the List of New Car Models ***********************/
                    NewCarModels = AutoMapper.Mapper.Map<List<ModelSummary>,List<CarModelSummaryDTOV2>>(NewCarModelsDetails),                  

                    /********* Returns the Make Details **********************/
                    MakeDetails = makeDetails,                                  

                    /************Returns the Title for the Make Page **************************/
                    Title = !string.IsNullOrEmpty(pageMetaTags.Title) ? pageMetaTags.Title : _carMakesBL.Title(pageMetaTags.Title, makeDetails.MakeName),

                    /*********** Returns the Description for Make Page*************************/
                    Description =!string.IsNullOrEmpty(pageMetaTags.Description) ? pageMetaTags.Description : _carMakesBL.Description(pageMetaTags.Description, makeDetails.MakeName),

                    /********** Returns the Heading for Make Page ****************************/
                    Heading = !string.IsNullOrEmpty(pageMetaTags.Heading) ? pageMetaTags.Heading : _carMakesBL.Heading(pageMetaTags.Heading, makeDetails.MakeName),

                    /********** Returns the Summary for Make Page ****************************/
                    Summary = !string.IsNullOrEmpty(pageMetaTags.Summary) ? pageMetaTags.Summary : _carMakesBL.Summary(pageMetaTags.Summary, makeDetails.MakeName, makeDetails.MakeId),
                  

                    // Get locate dealers cities                                     

                    
                };

                var apiGatewayCaller = new ApiGatewayCaller();
                ManageCmsCalls(apiGatewayCaller,NewCarModelsDetails,makeDTO,inputParam.MakeId);
                
                if(makeDTO.NewCarModels.IsNotNullOrEmpty())
                {
                    foreach(var model in makeDTO.NewCarModels)
                    {
                        if(!model.New)
                        {
                            model.CarPriceOverview.PricePrefix = ConfigurationManager.AppSettings["MakePageUpcomingPriceText"];
                            model.CarPriceOverview.PriceLabel = string.Empty;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MakePageDTO_Desktop.GetMakePageDTOForDesktop()");                
            }
            return makeDTO;
        }

        private void GetCarousalImages(IApiGatewayCaller apiGatewayCaller, MakePageDTO_Desktop makeDTO)
        {
            try
            {
                var cmsImagesResponse = apiGatewayCaller.GetResponse<GrpcModelsImageList>(1);
                var modelImages = new List<CMSImage>();

                if (cmsImagesResponse != null)
                {
                    modelImages = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(cmsImagesResponse);
                }

                makeDTO.Images = new ImageCarousal()
                {
                    ModelPhotos = AutoMapper.Mapper.Map<List<CMSImage>, List<ModelImageCarousal>>(modelImages)
                };
                _photosBl.GetMakeImageGallary(makeDTO.Images.ModelPhotos);
                makeDTO.Images.ModelPhotos.ForEach(x => x.ModelImagePageUrl = ManageCarUrl.CreateImageListingPageUrl(x.MakeName, x.ModelMaskingName));
                makeDTO.Images.LandingUrl = ManageCarUrl.CreateMakeImagePageUrl(makeDTO.Images.MakeName);
                makeDTO.Images.Title = string.Format("{0} Images", makeDTO.Images.MakeName);
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MakePageAdapterDesktop.GetCarousalImages()");
            }
        }

        private void GetVideos(IApiGatewayCaller apiGatewayCaller, MakePageDTO_Desktop makeDto)
        {
            try
            {
                var cmsVideosResponse = apiGatewayCaller.GetResponse<GrpcVideosList>(0);

                if (cmsVideosResponse != null)
                {
                    makeDto.MakeVideos = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(cmsVideosResponse);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MakePageAdapterDesktop.GetVideos()");
            }
        }

        private void ManageCmsCalls(IApiGatewayCaller apiGatewayCaller, List<ModelSummary> newCarModels, MakePageDTO_Desktop makeDto,int makeId)
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
