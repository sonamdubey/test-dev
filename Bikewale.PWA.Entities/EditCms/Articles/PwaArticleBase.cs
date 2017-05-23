using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>
    [Serializable, DataContract]
    public class PwaArticleBase
    {
        [DataMember]
        public ulong BasicId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string ArticleUrl { get; set; }
    }
}
