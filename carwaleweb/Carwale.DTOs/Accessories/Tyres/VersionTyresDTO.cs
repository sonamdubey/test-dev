using Newtonsoft.Json;

namespace Carwale.DTOs.Accessories.Tyres
{
    public class VersionTyresDTO : TyreListDTO
    {
        [JsonProperty("versionTyreSize")]
        public string VersionTyreSize { get; set; }
    }
}
