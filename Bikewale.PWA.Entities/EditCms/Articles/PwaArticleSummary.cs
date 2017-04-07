using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{

    [Serializable, DataContract]
    public class PwaArticleSummary : PwaArticleBase
    {
        [DataMember]
        public ushort CategoryId { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string LargePicUrl { get; set; }
        [DataMember]
        public string SmallPicUrl { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string AuthorName { get; set; }
        [DataMember]
        public string DisplayDate { get; set; }
        [DataMember]
        public string ArticleApi { get; set; }
    }
}
