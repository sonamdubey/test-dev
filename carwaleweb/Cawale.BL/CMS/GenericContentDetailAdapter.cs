using AutoMapper;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.CMS;
using Carwale.DTOs.CarData;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Entity;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Entity.CMS.Photos;
using Carwale.DTOs.CMS.Photos;

namespace Carwale.BL.CMS
{
    public class GenericContentDetailAdapter : IServiceAdapter
    {
        private readonly int _articleId;
        private readonly EnumGenericContentType _catgoryId;
        private readonly ICMSContent _cmsBl;
        private readonly IVideosBL _videoBl;
        private readonly IUserReviewsCache _userReviewCache;
        private readonly IPhotos _photoCache;
        public GenericContentDetailAdapter(IUnityContainer container, IVideosBL videoBl, ICMSContent cmsBl, IUserReviewsCache userReviewCache, IPhotos photoCache, int articleId, EnumGenericContentType categoryId)
        {
            _articleId = articleId;
            _catgoryId = categoryId;
            _videoBl = videoBl;
            _cmsBl = cmsBl;
            _userReviewCache = userReviewCache;
            _photoCache = photoCache;
        }     
        public T Get<T>()
        {
            return (T)Convert.ChangeType(GenericContentDetailDTO(), typeof(T));
        }

        public GenericContentDetailDTO GenericContentDetailDTO()
        {
            GenericContentDetailDTO detailDto = new GenericContentDetailDTO();
            ArticleContentURI queryString = new ArticleContentURI();            
            List<ModelImage> list = new List<ModelImage>();
            queryString.BasicId = (ulong)_articleId;

            switch ((int)_catgoryId)
            {
                case (int)EnumGenericContentType.news: 
                case (int)EnumGenericContentType.features:        
                    detailDto = new GenericContentDetailDTO()
                    {
                        News = Mapper.Map<Carwale.Entity.CMS.Articles.ArticleDetails, Carwale.DTOs.CMS.Articles.ArticleDetails_V1>(_cmsBl.GetContentDetails(queryString))
                    };
                    break;

                case (int)EnumGenericContentType.expertreviews:              
                    detailDto = new GenericContentDetailDTO()
                    {
                       ExpertReview = Mapper.Map<ArticlePageDetails, Carwale.DTOs.CMS.Articles.ArticlePageDetails_V1>(_cmsBl.GetContentPages(queryString))
                    };
                    break;
                case (int)EnumGenericContentType.videos:
                     detailDto = new GenericContentDetailDTO()
                      {
                     VideoContent = Mapper.Map<Video,VideoDTO_V1>(_videoBl.GetVideoByBasicId(_articleId, CMSAppId.Carwale))
                      };
                    break;
                case (int)EnumGenericContentType.userreviews:
                    detailDto = new GenericContentDetailDTO()
                    {
                        UserReview = Mapper.Map<UserReviewDetail,UserReviewDetailDTO>(_userReviewCache.GetUserReviewDetailById(_articleId,CMSAppId.Carwale))
                    };
                    break;
                case (int)EnumGenericContentType.galleries:
                    ArticlePhotoUri cmsPhotoId = new ArticlePhotoUri();
                    cmsPhotoId.basicId = (ulong)_articleId;
                    detailDto = new GenericContentDetailDTO()
                    {                                                                
                    PhotoGallery =   Mapper.Map<List<ModelImage>,List<ModelImageDTO>>(_photoCache.GetArticlePhotos(cmsPhotoId))
                        
                    };
                    break;   
      
            }
           
            return detailDto;
        }
    }

}
