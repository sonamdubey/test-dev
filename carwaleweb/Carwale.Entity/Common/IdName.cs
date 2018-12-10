using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Common
{
    [Serializable]
    public class IdName
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ValuationMake : IdName
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        //public List<IdName> makes { get; set; }
        //public string url { get; set; }
    }

    public class ValuationModel : IdName
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        //[JsonProperty("smallImage")]
        //public string SmallImage { get; set; }

        //[JsonProperty("largeImage")]
        //public string LargeImage { get; set; }
    }

   
    public class ValuationVersions : IdName
    {
        //public List<IdName> versions { get; set; }

        [JsonProperty("smallPicUrl")]
        public string SmallImage { get; set; }

        [JsonProperty("largePicUrl")]
        public string LargeImage { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }

    public class ValuationMakesDTO
    {
        [JsonProperty("makes")]
        public List<ValuationMake> Makes { get; set; }
    }
    
    public class ValuationModelsDTO
    {
        [JsonProperty("models")]
        public List<ValuationModel> Models { get; set; }
        //public string url { get; set; }
    }

    public class ValuationVersionsDTO
    {
        [JsonProperty("versions")]
        public List<ValuationVersions> Versions { get; set; }
    }



    /*public class CarModels
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MakeIdName
    {
        [JsonProperty("makes")]
        public List<IdNameAttributes> IdNameList { get; set; }

        [JsonProperty("url")]
        public string Url;
    }

    public class ModelIdName
    {
        [JsonProperty("models")]
        public List<IdNameAttributes> IdNameList { get; set; }

        [JsonProperty("url")]
        public string Url;
    }

    public class VersionIdName
    {
        [JsonProperty("versions")]
        public List<IdNameAttributes> IdNameList { get; set; }
    }*/
}
