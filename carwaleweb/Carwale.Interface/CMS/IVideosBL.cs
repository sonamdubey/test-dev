using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CMS
{
    /// <summary>
    /// Modified by     :   Sumit Kate on 18 Feb 2016
    /// Description     :   Added new method VideoListEntity GetVideosBySubCategories(string subCategoryIds, CMSAppId applicationId, int startIndex, int endindex, VideoSortOrderCategory sortCriteria);
    /// </summary>
    public interface IVideosBL
    {
        List<Video> GetSimilarVideos(int basicId, CMSAppId applicationId, int topCount);
        List<Video> GetNewModelsVideosByMakeId(int makeId, CMSAppId applicationId, int startIndex, int endIndex);
        List<Video> GetVideosByModelId(int modelId, CMSAppId applicationId, int startIndex, int endIndex);
        List<Video> GetNewModelsVideosBySubCategory(EnumVideoCategory subCategoryId, CMSAppId applicationId, int startIndex, int endindex);
        VideoListEntity GetVideosBySubCategories(string subCategoryIds, CMSAppId applicationId, ushort pageNo, ushort pageSize, string sortCategory);
        Video GetVideoByBasicId(int basicId, CMSAppId applicationId);
        bool UpdateViewsAndLikes(int basicId, int likesCount, int viewsCount);
        VideoListing GetPopularNewModelVideos(ArticleByCatURI queryString);
        List<VideosEntity> GetVideoList(uint basicId, uint applicationId);
        List<Video> GetSimilarVideos(ArticleByCatURI queryString);
        List<RelatedArticles> GetRelatedVideoContent(int basicId);
        List<Video> GetVideosBySubCategory(List<Video> result, int startIndex, int endIndex);
        List<Video> GetArticleVideos(int basicId);
    }
}
