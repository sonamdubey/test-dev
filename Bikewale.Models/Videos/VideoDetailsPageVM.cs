
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Mar 2017
    /// Description :   VideoDetailsPage View model
    /// </summary>
    public class VideoDetailsPageVM : ModelBase
    {
        public BikeVideoEntity Video { get; set; }
        public string Description { get; set; }
        public string DisplayDate { get; set; }
        public bool IsBikeTagged { get; set; }
        public string TaggedBikeName { get { return IsBikeTagged ? string.Format("{0} {1}", Video.MakeName, Video.ModelName) : ""; } }
        public uint VideoId { get; set; }
        public uint TaggedModelId { get; set; }
        public uint CityId { get; set; }
        public string SmallDescription { get; set; }
        public bool IsSmallDescription { get { return Video.Description.Length > 200; } }
        public PwaReduxStore Store { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public string WindowState { get; set; }
    }
}
