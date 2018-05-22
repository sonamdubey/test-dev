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
    /// Modified by : Rajan Chauhan on 26 Feb 2018
    /// Description : Moved BikeName to PwaModelImages
    /// </summary>
    [Serializable, DataContract]
    public class PwaImageList
    {
        [DataMember]
        public IEnumerable<PwaImageBase> ModelImages { get; set; }
        [DataMember]
        public int RecordCount { get; set; }
    }
}
