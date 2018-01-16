using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Pager;
using System;
using System.Runtime.Serialization;

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

    [Serializable, DataContract]
    public class ImagePager : PagerEntity
    {
        [DataMember]
        public int StartIndex { get; set; }

        [DataMember]
        public int EndIndex { get; set; }

        [DataMember]
        public int TotalPages { get; set; }

        [DataMember]
        public string NextPageUrl { get; set; }

        [DataMember]
        public string PrevPageUrl { get; set; }
    }
}
