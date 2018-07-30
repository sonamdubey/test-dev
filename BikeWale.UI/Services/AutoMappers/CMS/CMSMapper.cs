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
using System.Linq;

namespace Bikewale.Service.AutoMappers.CMS
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : class to map the cms entities to the DTOs.
    /// Modified By : Rajan Chauhan on 13 Jan 2018
    /// Description : Added ModelImages to ModelImageList
    /// </summary>
    public class CMSMapper
    {
        internal static ModelImageList Convert(IEnumerable<ModelImages> objImageList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            Mapper.CreateMap<ModelImages, CMSModelImages>();
            var obj = new ModelImageList();
            obj.Models = Mapper.Map<IEnumerable<ModelImages>, IEnumerable<CMSModelImages>>(objImageList);
            obj.RecordCount = obj.Models.Count();

            return obj;
        }

        /// <summary>
        /// Converts the specified image wrapper.
        /// </summary>
        /// <param name="ImageWrapper">The image wrapper.</param>
        /// <returns></returns>
        internal static ModelImageList Convert(ModelImageWrapper ImageWrapper)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            Mapper.CreateMap<ModelImages, CMSModelImages>();
            Mapper.CreateMap<ModelImageWrapper, ModelImageList>();
            return Mapper.Map<ModelImageWrapper, ModelImageList>(ImageWrapper);
        }

        internal static List<CMSModelImageBase> Convert(IEnumerable<ModelImage> objImageList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            return Mapper.Map<IEnumerable<ModelImage>, List<CMSModelImageBase>>(objImageList);
        }

        internal static CMSArticlePageDetails Convert(ArticlePageDetails objFeaturedArticles)
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

        internal static List<CMSArticleSummary> Convert(IEnumerable<Entities.CMS.Articles.ArticleSummary> objRecentArticles)
        {
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            return Mapper.Map<IEnumerable<Entities.CMS.Articles.ArticleSummary>, List<CMSArticleSummary>>(objRecentArticles);
        }

        internal static List<CMSArticleSummaryMin> ConvertV2(IEnumerable<Entities.CMS.Articles.ArticleSummary> objRecentArticles)
        {
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummaryMin>();
            return Mapper.Map<IEnumerable<Entities.CMS.Articles.ArticleSummary>, List<CMSArticleSummaryMin>>(objRecentArticles);
        }

        internal static Bikewale.DTO.CMS.Articles.CMSContent Convert(Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles)
        {
            Mapper.CreateMap<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>();
            Mapper.CreateMap<ArticleBase, CMSArticleBase>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
            return Mapper.Map<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>(objFeaturedArticles);
        }

        internal static CMSImageList Convert(Bikewale.Entities.CMS.Photos.CMSImage objPhotos)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<Bikewale.Entities.CMS.Photos.CMSImage, CMSImageList>();
            Mapper.CreateMap<ModelImage, CMSModelImageBase>();
            return Mapper.Map<Bikewale.Entities.CMS.Photos.CMSImage, CMSImageList>(objPhotos);
        }
    }
}