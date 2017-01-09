using System.Collections.Generic;

namespace BikewaleOpr.Entities
{
    public class ModelColorBase
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ColorCodeBase> ColorCodes { get; set; }
    }

    /// <summary>
    /// Created by: Sangram Nandkhile on 09 Jan 2017
    /// Desc: Model images by colors
    /// </summary>
    public class ModelColorImage : ModelColorBase
    {
        public string Host { get; set; }
        public string OriginalImagePath { get; set; }
        public bool IsImageExists { get; set; }
    }
}