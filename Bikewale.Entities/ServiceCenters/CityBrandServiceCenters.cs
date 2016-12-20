using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By  : Aditi Srivastava on 19 Dec 2016
    /// Description : To hold service center info of a brand in nearby cities
    /// </summary>
    [Serializable, DataContract]
    public class CityBrandServiceCenters :CityEntityBase
    {
        [DataMember]
        public int ServiceCenterCount { get; set; }

        [DataMember]
        public float Lattitude { get; set; }

        [DataMember]
        public float Longitude { get; set; }
    }
}
