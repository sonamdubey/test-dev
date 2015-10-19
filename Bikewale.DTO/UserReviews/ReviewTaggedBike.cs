using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bikewale.Entities.DTO
{
    public class ReviewTaggedBike
    {
        public MakeBase MakeEntity { get; set; }
        public ModelBase ModelEntity { get; set; }
        public VersionBase VersionEntity { get; set; }

        public uint ReviewsCount { get; set; }
        public uint Price { get; set; }
    }
}
