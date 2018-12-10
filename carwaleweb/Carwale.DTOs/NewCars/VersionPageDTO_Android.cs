using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini on 08/01/15
    /// Modified By : Shalini on 13/02/15 to include ModelDetails
    /// </summary>
    public class VersionPageDTO_Android
    {
        public CarVersionDetails VersionDetails { get; set; }
        public CCarData VersionData { get; set; }
        public List<CarVersions> OtherCarVersions { get; set; }
        public bool OfferExists { get; set; }
        public CarModelDetails ModelDetails { get; set; }
        public string CallSlugNumber { get; set; }
    }
}
