using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BikeWaleOpr.Entities
{
    [Serializable, DataContract]
    public class ModelEntityBase
    {
        [DataMember]
        public UInt32 ModelId { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
    }
}
