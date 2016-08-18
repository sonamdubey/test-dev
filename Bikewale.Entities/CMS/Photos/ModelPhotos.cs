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
    public class ModelPhotos
    {
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string HostURL { get; set; }
        [DataMember]
        public string OriginalImgPath { get; set; }
    }

}
