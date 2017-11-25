using Bikewale.Entities.PWA.Articles;
using System.Web;

namespace Bikewale.Interfaces.PWA.CMS
{
    /// <summary>
    /// Created By : Prasad Gawde
    /// Summary : Interface for PWA News Rendering.
    /// </summary> 
    public interface IPWACMSContentRepository
    {
        IHtmlString GetNewsListDetails(PwaNewsArticleListReducer reducer, string url, string containerId, string componentName);

        IHtmlString GetNewsDetails(PwaNewsDetailReducer reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoListDetails(PwaAllVideos reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoBySubCategoryListDetails(PwaVideosBySubcategory reducer, string url, string containerId, string componentName);

        IHtmlString GetVideoDetails(PwaVideoDetailReducer reducer, string url, string containerId, string componentName);
    }
}
