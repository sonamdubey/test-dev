using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
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
    }
}
