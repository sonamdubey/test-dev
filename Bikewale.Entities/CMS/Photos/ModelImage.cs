using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Photos
{
    [Serializable, DataContract]
    public class ModelImage
    {                          
        [DataMember]
        public uint ImageId { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string ImagePathThumbnail { get; set; }
        [DataMember]
        public string ImagePathLarge { get; set; }
        [DataMember]
        public short MainImgCategoryId { get; set; }
        [DataMember]
        public string ImageCategory { get; set; }
        [DataMember]
        public string Caption { get; set; }
        [DataMember]
        public string ImageName { get; set; }
        [DataMember]
        public string AltImageName { get; set; }
        [DataMember]
        public string ImageTitle { get; set; }
        [DataMember]
        public string ImageDescription { get; set; }
        [DataMember]
        public BikeMakeEntityBase MakeBase { get; set; }
        [DataMember]
        public BikeModelEntityBase ModelBase { get; set; }
        [DataMember]
        public string OriginalImgPath{ get; set; }
    }

}
