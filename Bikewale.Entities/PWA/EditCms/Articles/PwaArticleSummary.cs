using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By  : Prasad Gawde
    ///  Modified by : Pratibha Verma on 30 April 2018
    ///  Description : Added MakeName and ModelName
    /// </summary>
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
        public string DisplayDateTime { get; set; }
        [DataMember]
        public string ArticleApi { get; set; }
        [DataMember]
        public string ShareUrl { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
    }
}
