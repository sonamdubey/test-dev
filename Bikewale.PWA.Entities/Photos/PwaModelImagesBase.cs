using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.PWA.Entities.Photos
{
    /// <summary>
    /// Created by  : Rajan Chauhan on 24 Feb 2018
    /// </summary>
    [Serializable, DataContract]
    public class PwaModelImagesBase
    {
        [DataMember]
        public IEnumerable<PwaImageBase> ModelImages { get; set; }
        [DataMember]
        public int RecordCount { get; set; }
        [DataMember]
        public string BikeName { get; set; }
    }
}
