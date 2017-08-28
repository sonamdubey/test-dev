using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    
   [Serializable, DataContract]
    public class PwaBikeMakeEntityBase
    {
        [DataMember]
        public int MakeId { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string Href { get; set; }
        [DataMember]
        public string Title { get; set; }        
    }
}
