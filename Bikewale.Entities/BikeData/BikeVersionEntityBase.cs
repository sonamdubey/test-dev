using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class BikeVersionEntityBase
    {
        [DataMember]
        public int VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }
    }
}
