using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Deepak Israni on 9th Feb 2018
    /// Description : To get the count of bike models with expert reviews and total number of expert reviews
    /// </summary>
    [Serializable]
    public class ExpertReviewCountEntity
    {
        public uint MakeId { get; set; }
        public uint ModelCount { get; set; }
        public uint ExpertReviewCount { get; set; }
    }
}
