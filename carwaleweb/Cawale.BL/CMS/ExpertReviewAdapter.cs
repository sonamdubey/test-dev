using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS;
using Carwale.Entity.CMS;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Entity.Common;
using Carwale.Entity;
using Carwale.BL.CMS;
using Carwale.Interfaces.CMS;
using Carwale.BL.GrpcFiles;
using Grpc.CMS;

namespace Carwale.BL.Editorial
{
    public class ExpertReviewAdapter : IServiceAdapterV2
    {
        private readonly ICMSContent _cmsCacheRepository;
        private readonly IPhotos _photosCacheRepo;
        private readonly ICarModelCacheRepository _carModelCacheRepo;
        private readonly ulong _basicId;
        private readonly string _contentTypes = string.Empty;
        private readonly bool _isMobile = false;
        private readonly ICarModels _carModelBL;
        private readonly IVideosBL _videosBL;

        public ExpertReviewAdapter(ICarModels carModelBL,ICMSContent cmsCacheRepository, ICarModelCacheRepository carModelCacheRepo,
            IPhotos photosCacheRepo, IVideosBL videosBL, ulong basicId, string contentTypes, bool isMobile = false)
        {
            _cmsCacheRepository = cmsCacheRepository;
            _photosCacheRepo = photosCacheRepo;
            _carModelCacheRepo = carModelCacheRepo;
            _basicId = basicId;
            _contentTypes = contentTypes;
            _isMobile = isMobile;
            _carModelBL = carModelBL;
            _videosBL = videosBL;
        }
        
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(ContentDetailPagesDTO<U>(input), typeof(T));
        }

        private ContentDetailPagesDTO ContentDetailPagesDTO<U>(U input)
        {
            var expertReviewDTO = new ContentDetailPagesDTO();
            ulong basicId = _basicId;
            string articleUrl = (string)Convert.ChangeType(input, typeof(U));

            try
            {
                ulong articleBasicId = (ulong)GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetBasicIdFromArticleUrl(articleUrl + (basicId > 0 ? "-" + basicId : string.Empty) + "/"));

                if (articleBasicId <= 0)
                {
                    articleUrl = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticleUrlFromBasicId(basicId));

                    expertReviewDTO.ArticlePages = new ArticlePageDetails() 
                    {
                        ArticleUrl = articleUrl
                    };
                    expertReviewDTO.IsRedirect = true;

                    return expertReviewDTO;
                }

                expertReviewDTO = new ContentDetailPagesDTO
                {
                    ArticlePages = _cmsCacheRepository.GetContentPages(new ArticleContentURI { BasicId = articleBasicId }),
                    Videos = Mapper.Map<List<Video>, List<VideoDTO>>(_videosBL.GetArticleVideos((int)articleBasicId))
                };

                if (expertReviewDTO.ArticlePages != null)
                {
                    var RelatedArticles = _cmsCacheRepository.GetRelatedArticles(new ArticleRelatedURI
                    {
                        ApplicationId = Convert.ToUInt16(CMSAppId.Carwale),
                        BasicId = (uint)_basicId,
                        ContentTypes = _contentTypes,
                        TotalRecords = 10
                    });

                    if (RelatedArticles != null && RelatedArticles.Count > 0)
                    {
                        RelatedArticles.RemoveAll(x => x.BasicId == articleBasicId);
                        expertReviewDTO.RelatedArticleIds = RelatedArticles.Select(x => x.BasicId).Distinct().ToList();
                    }

                    if (!_isMobile)
                    {
                        int modelId = 0;
                        bool isVehicleTagged = expertReviewDTO.ArticlePages != null && expertReviewDTO.ArticlePages.VehiclTagsList.Count > 0;
                        modelId = isVehicleTagged ? expertReviewDTO.ArticlePages.VehiclTagsList[0].ModelBase.ModelId : 0;
                        var modelDetails = _carModelCacheRepo.GetModelDetailsById(modelId);
                        bool isExpertReviewAvail = true;
                        var videos = _videosBL.GetVideosByModelId(modelId, CMSAppId.Carwale, 1, -1);
                        modelDetails.VideoCount = videos != null ? videos.Count : 0;
                        expertReviewDTO.SubNavigation = _carModelBL.GetModelQuickMenu(modelDetails, expertReviewDTO.ArticlePages, false, isExpertReviewAvail, "ExpertReviewDetailsPage",""); 
                        expertReviewDTO.SubNavigation.PQPageId = expertReviewDTO.ArticlePages.CategoryId == 2 ? 49 : 44;
                        expertReviewDTO.SubNavigation.PageId = 11;
                        expertReviewDTO.SubNavigation.Page = Entity.Enum.Pages.ExpertReviewDetails;
                        expertReviewDTO.SubNavigation.ShowColorsLink = CMSCommon.IsModelColorPhotosPresent(_carModelCacheRepo.GetModelColorsByModel(modelId));
                        expertReviewDTO.ModelImages = GetPhotosDetails(articleBasicId);
                        

                        expertReviewDTO.CarWidgetModel = new CarRightWidget();
                        Pagination page = new Pagination { PageNo = 1, PageSize = 5 };
                        expertReviewDTO.CarWidgetModel.PopularModels = _carModelBL.GetTopSellingCarModels(page);
                        expertReviewDTO.CarWidgetModel.UpcomingCars = _carModelBL.GetUpcomingCarModels(page);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ExpertReviewAdapter.GetExpertReviewDTO()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return expertReviewDTO;
        }
        private List<ModelImage> GetPhotosDetails(ulong basicId)
        {
            var articleURI = new ArticlePhotoUri()
            {
                basicId = basicId
            };
            return _photosCacheRepo.GetArticlePhotos(articleURI);
        }
    }
}
