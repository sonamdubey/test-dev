using AutoMapper;
using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.CMS.Photos;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.CMS
{
    public class CMSMapper
    {
        internal static List<DTO.CMS.Photos.CMSModelImageBase> Convert(IEnumerable<Entities.CMS.Photos.ModelImage> objImageList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            return Mapper.Map<IEnumerable<ModelImage>, List<CMSModelImageBase>>(objImageList);
        }

        internal static DTO.CMS.Articles.CMSArticlePageDetails Convert(Entities.CMS.Articles.ArticlePageDetails objFeaturedArticles)
        {
            Mapper.CreateMap<ArticlePageDetails, CMSArticlePageDetails>();
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<Page, CMSPage>();
            return Mapper.Map<ArticlePageDetails, CMSArticlePageDetails>(objFeaturedArticles);
        }

        internal static CMSArticleDetails Convert(ArticleDetails objNews)
        {
            Mapper.CreateMap<ArticleDetails, CMSArticleDetails>();
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            return Mapper.Map<ArticleDetails, CMSArticleDetails>(objNews);
        }

        internal static List<CMSArticleSummary> Convert(List<Entities.CMS.Articles.ArticleSummary> objRecentArticles)
        {
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            return Mapper.Map<List<Entities.CMS.Articles.ArticleSummary>, List<CMSArticleSummary>>(objRecentArticles);
        }

        internal static Bikewale.DTO.CMS.Articles.CMSContent Convert(Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles)
        {
            Mapper.CreateMap<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>();
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
            return Mapper.Map<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>(objFeaturedArticles);
        }

        internal static CMSImageList Convert(CMSImage objPhotos)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<CMSImage, CMSImageList>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            return Mapper.Map<CMSImage, CMSImageList>(objPhotos);
        }
    }
}