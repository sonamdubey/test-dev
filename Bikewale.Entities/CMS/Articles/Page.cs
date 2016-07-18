using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>    
    [Serializable, DataContract]
    public class Page
    {
        [DataMember]
        public ulong pageId { get; set; }
        [DataMember]
        public ushort Priority { get; set; }
        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string Content { get; set; }
    }
}
