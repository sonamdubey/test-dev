using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaMakeScooterBase : PwaBikeCms
    {
        [DataMember]
        public List<PwaMakeScooterEntity> MakeList { get; set; }
    }
}
