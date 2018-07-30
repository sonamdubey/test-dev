using Bikewale.Entities.PWA.Articles;
using System.Web;

namespace Bikewale.Interfaces.PWA.CMS
{
    /// <summary>
    /// Created By : Prasad Gawde
    /// Summary : Interface for PWA News Rendering.
    /// Modified By : Rajan Chauhan on 26 Feb 2018
    /// Description : Added pageName to args for GetNewsDetails
    /// </summary> 
    public interface IPWACMSContentRepository
    {
        IHtmlString GetNewsListDetails(PwaNewsArticleListReducer reducer, string url, string containerId, string componentName);

        IHtmlString GetNewsDetails(PwaNewsDetailReducer reducer, string url, string containerId, string componentName, string pageName);

        IHtmlString GetVideoListDetails(PwaAllVideos reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoBySubCategoryListDetails(PwaVideosBySubcategory reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoDetails(PwaVideoDetailReducer reducer, string url, string containerId, string componentName);
    }
}
