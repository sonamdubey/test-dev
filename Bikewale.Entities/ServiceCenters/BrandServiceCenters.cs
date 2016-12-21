using Bikewale.Entities.BikeData;
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
    public class BrandServiceCenters : BikeMakeEntityBase
    {
       
       [DataMember]
       public int ServiceCenterCount { get; set; }

   
     }
}
