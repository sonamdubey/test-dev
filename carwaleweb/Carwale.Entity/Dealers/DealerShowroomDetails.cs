using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
   //Created on 10/03/15 by Shalini
    //[JsonObject]
    public class DealerShowroomDetails
    {
        public DealerDetails objDealerDetails { get; set; }
        public List<AboutUsImageEntity> objImageList { get; set; }
        public List<CarModelSummary> objModelDetails { get; set; }
    }
}
