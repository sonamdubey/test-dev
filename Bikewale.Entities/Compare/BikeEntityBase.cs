using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    public class BikeEntityBase
    {
        public uint VersionId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string HostUrl { get; set; }
        public int Price { get; set; }
        public string ImagePath { get; set; }
        public UInt16 VersionRating { get; set; }
        public UInt16 ModelRating { get; set; }
    }
}
