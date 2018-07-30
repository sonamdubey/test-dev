using Bikewale.PWA.Entities.Photos;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
    /// Modified By : Ashish G. Kamble on 5 Jan 2018
    /// Modified : Added Tags property
    /// Modified By : Rajan Chauhan on 27 Feb 2018
    /// Description : Added ImageGallery property
    /// Modified by : Ashutosh Sharma on 01 Mar 2018
    /// Description : Added TopContent and BottomContent.
    /// </summary>
    [Serializable, DataContract]
    public class PwaArticleDetails : PwaArticleSummary
    {
        [DataMember]
        public string TopContent { get; set; }
        [DataMember]
        public string BottomContent { get; set; }
        [DataMember]
        public PwaArticleBase NextArticle { get; set; }
        [DataMember]
        public PwaArticleBase PrevArticle { get; set; }
        [DataMember]
        public string AuthorMaskingName { get; set; }
        [DataMember]
        public string Tags { get; set; }
        [DataMember]
        public PwaImageList ImageGallery { get; set; }
    }
}
