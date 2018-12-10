using AutoMapper;
using Carwale.BL.CMS;
using Carwale.BL.GrpcFiles;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Grpc.CMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.Editorial
{
    public class ExpertReviewAdapterMobile : IServiceAdapterV2
    {
        private readonly ICMSContent _cmsCacheRepository;
        private readonly IPhotos _photosCacheRepo;
        private readonly ICarModelCacheRepository _carModelCacheRepo;
        private readonly ulong _basicId;
        private readonly string _contentTypes = string.Empty;
        private readonly bool _isMobile = false;
        private readonly IPhotos _photosBL;
        private readonly IVideosBL _videosBL;

        public ExpertReviewAdapterMobile(ICMSContent cmsCacheRepository, ICarModelCacheRepository carModelCacheRepo,
            IPhotos photosCacheRepo, IPhotos photosBL, IVideosBL videosBL, ulong basicId, string contentTypes, bool isMobile = false)
        {
            _cmsCacheRepository = cmsCacheRepository;
            _photosCacheRepo = photosCacheRepo;
            _carModelCacheRepo = carModelCacheRepo;
            _basicId = basicId;
            _contentTypes = contentTypes;
            _isMobile = isMobile;
            _photosBL = photosBL;
            _videosBL = videosBL;
        }


        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(ContentDetailPagesDTO<U>(input), typeof(T));
        }

        private ContentDetailPagesDTO_V1 ContentDetailPagesDTO<U>(U input)
        {
            var expertReviewDTO = new ContentDetailPagesDTO_V1();
            ulong basicId = _basicId;
            Tuple<string, bool> inputTuple = (Tuple<string, bool>)Convert.ChangeType(input, typeof(U));
            string articleUrl = inputTuple.Item1;
            bool isRelatedArticle = inputTuple.Item2;

            try
            {
                ulong articleBasicId = (ulong)GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetBasicIdFromArticleUrl(articleUrl + (basicId > 0 ? "-" + basicId : string.Empty) + "/"));

                if (articleBasicId <= 0 && !isRelatedArticle)
                {
                    articleUrl = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticleUrlFromBasicId(basicId));

                    expertReviewDTO.ArticlePages = new ArticlePageDetails()
                    {
                        ArticleUrl = articleUrl
                    };
                    expertReviewDTO.IsRedirect = true;

                    return expertReviewDTO;
                }

                if (isRelatedArticle && articleBasicId <= 0)
                {
                    articleBasicId = basicId;
                }

                expertReviewDTO = new ContentDetailPagesDTO_V1()
                {
                    ArticlePages = _cmsCacheRepository.GetContentPages(new ArticleContentURI { BasicId = articleBasicId }),
                    RelatedArticles = Mapper.Map<List<Carwale.Entity.CMS.Articles.RelatedArticles>, List<Carwale.DTOs.CMS.Articles.RelatedArticlesDTO>>(
                                                                                       _cmsCacheRepository.GetRelatedContent(Convert.ToInt32(articleBasicId))),
                    ModelImages = _photosBL.GetArticlePhotos(new ArticlePhotoUri { basicId = articleBasicId }),
                    Videos = _videosBL.GetArticleVideos((int)articleBasicId)
                };
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ExpertReviewAdapter.GetExpertReviewDTO()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return expertReviewDTO;
        }
    }
}
