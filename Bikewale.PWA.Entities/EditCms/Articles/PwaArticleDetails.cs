using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
    /// Modified By : Ashish G. Kamble on 5 Jan 2018
    /// Modified : Added Tags property
    /// </summary>
    [Serializable, DataContract]
    public class PwaArticleDetails : PwaArticleSummary
    {
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public PwaArticleBase NextArticle { get; set; }
        [DataMember]
        public PwaArticleBase PrevArticle { get; set; }
        [DataMember]
        public string AuthorMaskingName { get; set; }
        [DataMember]
        public string Tags { get; set; }        
    }
}
