using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BikeWaleOpr.Entities
{
    [Serializable, DataContract]
    public class MakeEntityBase
    {
        [DataMember]
        public UInt32 Id { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
    }
}
