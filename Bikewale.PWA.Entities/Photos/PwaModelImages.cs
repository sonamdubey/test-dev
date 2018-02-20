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
    /// </summary>
    [Serializable, DataContract]
    public class PwaModelImages
    {
        [DataMember]
        public IEnumerable<PwaImageBase> ModelImages { get; set; }
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public int RecordCount { get; set; }
        [DataMember]
        public string BikeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelImagePageUrl { get; set; }
    }
}
