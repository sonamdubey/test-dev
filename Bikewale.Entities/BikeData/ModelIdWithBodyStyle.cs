using System;
using System.Runtime.Serialization;
using Bikewale.Entities.GenericBikes;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by   : Vivek Singh Tomar on 11th Jan 2018
    /// Descrioption : To store model id with it's bodystyle
    /// </summary>

    [Serializable, DataContract]
    public class ModelIdWithBodyStyle
    {
        [DataMember]
        public uint MakeId { get; set; }
        [DataMember]
        public uint ModelId { get; set; }
        [DataMember]
        public EnumBikeBodyStyles BodyStyle { get; set; }
    }
}
