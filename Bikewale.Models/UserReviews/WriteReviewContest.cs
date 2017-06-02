using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Models
{
    public class WriteReviewContestVM : ModelBase
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public string QueryString { get; set; }
    }
}
