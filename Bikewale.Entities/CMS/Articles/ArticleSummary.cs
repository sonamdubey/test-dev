using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// Modified By : Monika Korrapati on 22 Nov 2018
    /// Description : Added ModifiedDate.
    /// </summary> 

    [Serializable, DataContract]
    public class ArticleSummary : ArticleBase
    {
        [DataMember]
        public ushort CategoryId { get; set; }
		[DataMember]
		public string SubCategory { get; set; }
		[DataMember]
		public UInt32 SubCategoryId { get; set; }
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
        public string AuthorMaskingName { get; set; }
        [DataMember]
        public DateTime DisplayDate { get; set; }
        [DataMember]
        public uint Views { get; set; }
        [DataMember]
        public bool IsSticky { get; set; }
        [DataMember]
        public uint FacebookCommentCount { get; set; }
        [DataMember]
        public string OriginalImgUrl { get; set; }
        [DataMember]
        public bool IsFeatured { get; set; }
        [DataMember]
        public string FormattedDisplayDate { get; set; }
        [DataMember]
        public string ShareUrl { get; set; }
        [DataMember]
        public UInt32 ArticleWordCount { get; set; }
        [DataMember]
        public UInt32 EstimatedReadingTime { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public DateTime ModifiedDate { get; set; }
    }
}
