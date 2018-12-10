using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS;
using Carwale.DTOs.CMS.Articles;
using Carwale.DTOs.CMS.Media;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity;
using Carwale.Entity.Author;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.ThreeSixtyView;
using Carwale.Utility;
using System;
using System.Configuration;
using System.Collections.Generic;
using Carwale.DTOs.CMS.UserReviews;
using Carwale.Entity.CMS.UserReviews;
using EditCMSWindowsService.Messages;

namespace Carwale.Service.Mappers
{
    public static class CmsMappers
    {
        public static Uri urlDomainWithDefaultProtocol = new Uri(ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", ""));
        public static string domainUrlWithHttpProtocol = string.Format("http://{0}", urlDomainWithDefaultProtocol.Host);
        public static string domainUrlWithHttpsProtocol = string.Format("https://{0}", urlDomainWithDefaultProtocol.Host);

        public static void CreateMaps()
        {
            Mapper.CreateMap<ModelColors, ModelColorsDTO>();
            Mapper.CreateMap<ModelColors, Carwale.DTOs.CMS.Photos.ModelColorsDTO>()
                .ForMember(x => x.HexCode, member => member.MapFrom(s => new List<string>(s.HexCode.Split(','))))
                .ForMember(x => x.ColorName, member => member.MapFrom(s => s.Color))
                .ForMember(x => x.HostUrl, member => member.MapFrom(s => ConfigurationManager.AppSettings["CDNHostURL"]));
            Mapper.CreateMap<Video, VideoDTO>();
            Mapper.CreateMap<Video, VideoDTO_V1>();

            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleSummary, Carwale.DTOs.CMS.Articles.ArticleGist>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticlePageDetails, Carwale.DTOs.CMS.Articles.ArticlePageDetails>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.Page, Carwale.DTOs.CMS.Articles.Page>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleBase, Carwale.DTOs.CMS.Articles.ArticleBase>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleBase, ArticleBaseDTOV2>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleBase, Carwale.DTOs.CMS.Articles.ArticleBaseV2>()
                .ForMember(x => x.ArticleUrl, member => member.MapFrom(source => domainUrlWithHttpsProtocol + source.ArticleUrl));
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleSummary, Carwale.DTOs.CMS.Articles.ArticleSummary>();
            Mapper.CreateMap<Entity.CMS.Articles.ArticleSummary, Carwale.DTOs.CMS.Articles.ArticleSummaryDTOV2>()
                .ForMember(x => x.DisplayDate, o => o.MapFrom(s => ExtensionMethods.ConvertDateToDays(s.DisplayDate)));
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleSummary, Carwale.DTOs.CMS.Articles.ArticleSummaryDTOV3>()
                .ForMember(x => x.DisplayDate, o => o.MapFrom(s => s.DisplayDate.ToString()))
                .ForMember(x => x.FormattedDisplayDate, o => o.MapFrom(s => Carwale.Utility.ExtensionMethods.ConvertDateToDays(s.DisplayDate)))
                .ForMember(x => x.DetailPageUrl, o => o.MapFrom(s => ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", "") + "api/newsdetail/?id=" + s.BasicId));
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleSummary, Carwale.DTOs.CMS.Articles.ArticleSummaryDTOV4>()
                 .ForMember(x => x.DisplayDate, o => o.MapFrom(s => s.DisplayDate.ToString()))
                 .ForMember(x => x.ArticleUrl, o => o.MapFrom(s => domainUrlWithHttpsProtocol + s.ArticleUrl))
                 .ForMember(x => x.AuthorUrl, o => o.MapFrom(s => string.Format("{0}/authors/{1}", domainUrlWithHttpsProtocol, s.AuthorMaskingName)))
                 .ForMember(x => x.FormattedDisplayDate, o => o.MapFrom(s => Carwale.Utility.ExtensionMethods.ConvertDateToDays(s.DisplayDate)))
                 .ForMember(x => x.DetailPageUrl, o => o.MapFrom(s => ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", "") + "api/newsdetail/?id=" + s.BasicId));

            Mapper.CreateMap<Carwale.Entity.CMS.Articles.VehicleTag, Carwale.DTOs.CMS.Articles.VehicleTag>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleDetails, Carwale.DTOs.CMS.Articles.ArticleDetails>()
                 .ForMember(x => x.ShareUrl, o => o.MapFrom(s => System.Configuration.ConfigurationManager.AppSettings["WebApiHostUrl"].TrimEnd('/') + s.ArticleUrl));
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleDetails, Carwale.DTOs.CMS.Articles.ArticleDetails_V1>()
                  .ForMember(x => x.ShareUrl, o => o.MapFrom(s => System.Configuration.ConfigurationManager.AppSettings["WebApiHostUrl"].TrimEnd('/') + s.ArticleUrl));
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContent>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContentDTOV2>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContentDTOV3>();

            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticlePageDetails, Carwale.DTOs.CMS.Articles.ArticlePageDetails_V1>()
                .ForMember(x => x.VehicleTagsList, o => o.MapFrom(s => s.VehiclTagsList))
                .ForMember(x => x.ShareUrl, o => o.MapFrom(s => System.Configuration.ConfigurationManager.AppSettings["WebApiHostUrl"].TrimEnd('/') + s.ArticleUrl));
            Mapper.CreateMap<ModelImage, Carwale.DTOs.CMS.Photos.ModelImageDTO>();

            Mapper.CreateMap<Carwale.Entity.CMS.Articles.Page, Carwale.DTOs.CMS.Articles.Page_V1>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.VehicleTag, Carwale.DTOs.CMS.Articles.VehicleTag_V1>();

            Mapper.CreateMap<CarImageBase, CarImageBaseDTO>()
                .ForMember(dest => dest.OriginalImgPath,
                opts => opts.MapFrom(
                    src => src.ImagePath));

            Mapper.CreateMap<CMSSubCategoryV2, CMSSubCategory>();
            Mapper.CreateMap<Media, MediaDTO>();
            Mapper.CreateMap<PhotosListing, PhotosListingDTO>();
            Mapper.CreateMap<VideoListing, VideoListingDTO>();
            Mapper.CreateMap<CarImageBase, CarImageBaseDTO>()
                .ForMember(d => d.HostUrl, o => o.MapFrom(s => s.HostUrl))
                .ForMember(d => d.OriginalImgPath, o => o.MapFrom(s => s.ImagePath));

            Mapper.CreateMap<CarReviewBase, Carwale.DTOs.CarData.CarReviewDTO>()
                .ForMember(d => d.OverallRating, o => o.MapFrom(s => s.OverallRating))
                .ForMember(d => d.ReviewCount, o => o.MapFrom(s => s.ReviewCount));

            Mapper.CreateMap<UserReviewDetail, UserReviewDetailDTO>();

            Mapper.CreateMap<CMSSubCategoryV2, ContentSegmentDTO>()
                .ForMember(x => x.CategoryId, o => o.MapFrom(s => s.SubCategoryId))
                .ForMember(x => x.DisplayName, o => o.MapFrom(s => s.SubCategoryName))
                .ForMember(x => x.RecordCount, o => o.MapFrom(s => s.RecordCount));

            Mapper.CreateMap<ExpertReviews, NewsEntity>();

            Mapper.CreateMap<ThreeSixtyCamera, ThreeSixtyCameraDto>();
            Mapper.CreateMap<ThreeSixtyViewImage, ThreeSixtyViewImageDto>();
            Mapper.CreateMap<ThreeSixty, ThreeSixtyExteriorDtoApp>()
                .ForMember(d => d.Images, o => o.MapFrom(s => s.ExteriorImages));
            Mapper.CreateMap<ThreeSixty, ThreeSixtyInteriorDtoApp>()
                .ForMember(d => d.Images, o => o.MapFrom(s => s.InteriorImages));
            Mapper.CreateMap<ModelVideo, ModelVideoDTO>()
                .ForMember(d => d.Duration, o => o.MapFrom(s => Format.SecondsToTime(s.Duration)))
                .ForMember(d => d.DisplayDate, o => o.MapFrom(s => Format.GetDisplayTimeSpan(s.DisplayDate.ToString())))
                .ForMember(d => d.Views, o => o.MapFrom(s => Format.GetNumberInKilos(s.Views)));
            Mapper.CreateMap<CarModelDetails, ThreeSixtyAvailabilityDTO>();
            Mapper.CreateMap<CarOverviewDTO, ThreeSixtyAvailabilityDTO>();
            Mapper.CreateMap<CMSImage, Carwale.Entity.ViewModels.ModelImageCarousal>();
            Mapper.CreateMap<ModelImage, Carwale.Entity.ViewModels.ModelImage>();
            Mapper.CreateMap<GrpcModelImageList, ModelImage>();
            Mapper.CreateMap<GrpcArticleBase, Carwale.Entity.CMS.Articles.ArticleBase>();
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.ArticleSummary, Carwale.DTOs.CMS.Articles.ArticleSummary>();
            Mapper.CreateMap<GrpcArticleSummary, Carwale.Entity.CMS.Articles.ArticleSummary>()
                .ForMember(d => d.DisplayDate, o => o.MapFrom(s => CustomParser.parseDateObject(s.DisplayDate)))
                .ForMember(d => d.Tags, o => o.MapFrom(s => s.Tag))
                .ForMember(d=>d.Title,o=>o.MapFrom(s=>s.ArticleBase.Title))
                .ForMember(d => d.ArticleUrl, o => o.MapFrom(s => s.ArticleBase.ArticleUrl))
                .ForMember(d => d.BasicId, o => o.MapFrom(s => s.ArticleBase.BasicId))
                .ForMember(d => d.CompleteArticleUrl, o => o.MapFrom(s => s.ArticleBase.CompleteArticleUrl));
            Mapper.CreateMap<GrpcVideo, Video>()
                 .ForMember(d => d.DisplayDate, o => o.MapFrom(s => CustomParser.parseDateObject(s.DisplayDate)));


        }
    }
}
