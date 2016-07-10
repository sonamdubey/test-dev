using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BikeWaleOpr.Entities
{
    [Serializable,DataContract]
    public class VersionEntityBase
    {
        [DataMember]
        public UInt32 VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }
    }
}
