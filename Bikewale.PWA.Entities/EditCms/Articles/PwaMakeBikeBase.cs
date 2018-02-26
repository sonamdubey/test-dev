using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaMakeBikeBase : PwaBikeCms
    {
        [DataMember]
        public List<PwaMakeBikeEntity> MakeList { get; set; }
    }
}
