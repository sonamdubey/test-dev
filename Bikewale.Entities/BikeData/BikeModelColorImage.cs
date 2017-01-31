using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 30 Jan 2017
    /// Desc: Model images by colors, ColorCodeBase and ModelColorBase
    /// </summary>

    public class ColorCodeBase
    {
        public uint Id { get; set; }
        public uint ModelColorId { get; set; }
        public string HexCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class ModelColorBase
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("colors")]
        public IEnumerable<ColorCodeBase> ColorCodes { get; set; }
    }


    public class ModelColorImage : ModelColorBase
    {

        public string Host { get; set; }
        public string OriginalImagePath { get; set; }
        public bool IsImageExists { get; set; }
        public uint BikeModelColorId { get; set; }
    }
}
