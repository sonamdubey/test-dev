

using Bikewale.Entities.PWA.Articles;
using System.Web;

namespace Bikewale.Interfaces.PWA.CMS
{
    /// <summary>
    /// Created By : Prasad Gawde
    /// Summary : Interface for PWA News Rendering.
    /// </summary> 
    public interface IPWACMSCacheRepository
    {
        IHtmlString GetNewsListDetails(string key, PwaNewsArticleListReducer reducer,string url, string containerId,string componentName);

        IHtmlString GetNewsDetails(string key, PwaNewsDetailReducer reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoListDetails(string key, PwaAllVideos reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoBySubCategoryListDetails(string key, PwaVideosBySubcategory reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoDetails(string key, PwaVideoDetailReducer reducer, string url, string containerId, string componentName);
    }
}
