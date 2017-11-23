using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using System.Web;

namespace Bikewale.Models.Videos
{
    public class VideosByCategoryPageVM : ModelBase
    {
        public BikeVideosListEntity Videos { get; set; }
        public string CanonicalTitle { get; set; }
        public string TitleName { get; set; }
        public string PageHeading { get; set; }
        public string CategoryId { get; set; }
        public PwaReduxStore Store { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public string WindowState { get; set; }
    }
}
