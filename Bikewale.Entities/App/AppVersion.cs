using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   APP Version Entity
    /// Created On  :   07 Dec 2015
    /// </summary>
    [Serializable, DataContract]
    public class AppVersion
    {
        [DataMember]
        public uint Id { get; set; }
        [DataMember]
        public bool IsSupported { get; set; }
        [DataMember]
        public bool IsLatest { get; set; }
    }
}
