using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.OffersV1
{
    public class OfferInput
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int ApplicationId { get; set; }
    }
}
