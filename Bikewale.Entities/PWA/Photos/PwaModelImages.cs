using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.PWA.Entities.Photos
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 12 Feb 2018.
    /// Modified by : Rajan Chauhan on 14th Feb 2018
    /// Description : Added ModelName and MakeName property
    /// Modified by : Rajan Chauhan on 26 Feb 2018
    /// Description : Moved BikeName from PwaImageList to PwaModelImages
    /// </summary>
    [Serializable, DataContract]
    public class PwaModelImages : PwaImageList
    {
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelImagePageUrl { get; set; }
        [DataMember]
        public string BikeName { get; set; }
    }
}
