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
    public class CityBrandServiceCenters
    {
        [DataMember]
        public int CityId { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string CityMaskingName { get; set; }

        [DataMember]
        public int ServiceCenterCount { get; set; }
    }
}
