using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaBikeVideoEntity
    {
        [DataMember]
        public string VideoTitle { get; set; }
        [DataMember]
        public string VideoUrl { get; set; }
        [DataMember]
        public string VideoId { get; set; }
        [DataMember]
        public string Views { get; set; }
        [DataMember]
        public string Likes { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }
        [DataMember]
        public uint BasicId { get; set; }
        [DataMember]        
        public string VideoTitleUrl { get; set; }
        [DataMember]
        public string DisplayDate { get; set; }
        [DataMember]
        public string SectionTitle { get; set; }
        [DataMember]
        public string SectionUrl { get; set; }
    }
}
