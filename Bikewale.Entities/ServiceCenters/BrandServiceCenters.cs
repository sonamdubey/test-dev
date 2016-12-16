using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By  : Aditi Srivastava on 15 Dec 2016
    /// Description : To hold service center info by brand
    /// </summary>
   [Serializable, DataContract]
    public class BrandServiceCenters
    {
       [DataMember]
       public int MakeId { get; set; }

       [DataMember]
       public string MakeName { get; set; }

       [DataMember]
       public string MakeMaskingName { get; set; }

       [DataMember]
       public int ServiceCenterCount { get; set; }

       [DataMember]
       public string LogoURL { get; set; }

       [DataMember]
       public string HostURL { get; set; }
     }
}
