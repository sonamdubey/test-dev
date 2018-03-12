using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Modified By : Pratibha Verma on 26 Feb, 2018
    /// </summary>
    [Serializable, DataContract]
    public class PwaMakeBikeBase : PwaBikeCms
    {
        [DataMember]
        public List<PwaMakeBikeEntity> MakeList { get; set; }
    }
}
