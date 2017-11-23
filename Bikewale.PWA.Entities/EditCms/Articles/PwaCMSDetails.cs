using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaCMSDetails
    {
        [DataMember]
        public CmsType Type { get; set; }        
    }

    public enum CmsType
    {
        News=0,
        Videos=1,
    }
}
