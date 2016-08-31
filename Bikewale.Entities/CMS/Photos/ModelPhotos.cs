using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
using System.Runtime.Serialization;
using Bikewale.Entities.BikeData;


namespace Bikewale.Entities.CMS.Photos
{
    /// <summary>
    /// Created By : Aditi Srivastava on 17th Aug,2016
    /// Description: To set requred details of first image(model image) in moidel gallery
    /// </summary>
    [Serializable, DataContract]
    public class ModelPhotos
    {
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string HostURL { get; set; }
        [DataMember]
        public string OriginalImgPath { get; set; }
        [DataMember]
        public string ImageCategory { get; set; }
    }

}
