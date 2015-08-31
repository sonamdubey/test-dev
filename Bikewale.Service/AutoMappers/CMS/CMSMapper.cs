using AutoMapper;
using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.CMS.Photos;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.CMS
{
    public class CMSMapper
    {
        internal static List<DTO.CMS.Photos.CMSModelImageBase> Convert(List<Entities.CMS.Photos.ModelImage> objImageList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            return Mapper.Map<List<ModelImage>, List<CMSModelImageBase>>(objImageList);
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

        internal static List<CMSArticleSummary> Convert(List<ArticleSummary> objRecentArticles)
        {
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            return Mapper.Map<List<ArticleSummary>, List<CMSArticleSummary>>(objRecentArticles);
        }

        internal static List<Bikewale.DTO.CMS.Articles.CMSContent> Convert(List<Bikewale.Entities.CMS.Articles.CMSContent> objFeaturedArticles)
        {
            Mapper.CreateMap<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>();
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
            return Mapper.Map<List<Bikewale.Entities.CMS.Articles.CMSContent>, List<Bikewale.DTO.CMS.Articles.CMSContent>>(objFeaturedArticles);
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