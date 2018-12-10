using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Newtonsoft.Json;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 27 Aug 2014
    /// </summary>
    public class VehicleTag
    {
        public CarMakeEntityBase MakeBase { get; set; }
        public CarModelEntityBase ModelBase { get; set; }
        public CarVersionEntity VersionBase { get; set; }
    }

    public class VehicleTag_V1
    {
        [JsonProperty("makeBase")]
        public CarMakesDTO MakeBase { get; set; }
        [JsonProperty("modelBase")]
        public CarModelsDTO ModelBase { get; set; }
        [JsonProperty("versionBase")]
        public CarVersionsDTO VersionBase { get; set; }
    }
}
