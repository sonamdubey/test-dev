using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Models
{
    /// <summary>
    /// Summary: View model for write Review
    /// Created by: Sangram Nandkhile on 05 June 2017
    /// Modified by:
    /// </summary>
    public class WriteReviewContestVM : ModelBase
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public string QueryString { get; set; }
    }
}
