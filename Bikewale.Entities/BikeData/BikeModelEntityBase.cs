using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class BikeModelEntityBase
    {
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
    }
}
