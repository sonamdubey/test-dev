using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary> 
    [DataContract, Serializable]
    public class ArticleDetails : ArticleSummary
    {
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public List<string> TagsList { get; set; }
        [DataMember]
        public List<VehicleTag> VehiclTagsList { get; set; }
        [DataMember]
        public ArticleBase NextArticle { get; set; }
        [DataMember]
        public ArticleBase PrevArticle { get; set; }
        [DataMember]
        public string MainImgCaption { get; set; }
        [DataMember]
        public bool IsMainImageSet { get; set; }
        [DataMember]
        public string AuthorMaskingName { get; set; }
    }
}
