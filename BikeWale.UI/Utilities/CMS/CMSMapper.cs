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

namespace Bikewale.Automappers
{
    public class CMSMapper
    {
        public static List<DTO.CMS.Photos.CMSModelImageBase> Convert(IEnumerable<Entities.CMS.Photos.ModelImage> objImageList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            return Mapper.Map<IEnumerable<ModelImage>, List<CMSModelImageBase>>(objImageList);
        }

        public static DTO.CMS.Articles.CMSArticlePageDetails Convert(Entities.CMS.Articles.ArticlePageDetails objFeaturedArticles)
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

        public static CMSArticleDetails Convert(ArticleDetails objNews)
        {
            Mapper.CreateMap<ArticleDetails, CMSArticleDetails>();
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            return Mapper.Map<ArticleDetails, CMSArticleDetails>(objNews);
        }

        public static List<CMSArticleSummary> Convert(IEnumerable<Entities.CMS.Articles.ArticleSummary> objRecentArticles)
        {
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            return Mapper.Map<IEnumerable<Entities.CMS.Articles.ArticleSummary>, List<CMSArticleSummary>>(objRecentArticles);
        }

        public static Bikewale.DTO.CMS.Articles.CMSContent Convert(Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles)
        {
            Mapper.CreateMap<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>();
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
            return Mapper.Map<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>(objFeaturedArticles);
        }

        public static CMSImageList Convert(Bikewale.Entities.CMS.Photos.CMSImage objPhotos)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<Bikewale.Entities.CMS.Photos.CMSImage, CMSImageList>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            return Mapper.Map<Bikewale.Entities.CMS.Photos.CMSImage, CMSImageList>(objPhotos);
        }

    }   // End of CMSMapper
}   // end of namespace
