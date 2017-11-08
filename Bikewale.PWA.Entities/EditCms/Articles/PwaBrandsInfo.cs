using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaBrandsInfo
    {
        [DataMember]
        public IEnumerable<PwaBikeMakeEntityBase> TopBrands { get; set; }
        [DataMember]
        public IEnumerable<PwaBikeMakeEntityBase> OtherBrands { get; set; }
    }
}
