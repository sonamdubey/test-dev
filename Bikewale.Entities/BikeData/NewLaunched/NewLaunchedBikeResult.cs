using System.Collections.Generic;

namespace Bikewale.Entities.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   New Launched Bike Result
    /// </summary>
    public class NewLaunchedBikeResult
    {
        public IEnumerable<NewLaunchedBikeEntityBase> Bikes { get; set; }
        public InputFilter Filter { get; set; }
        public uint TotalCount { get; set; }
        public uint MinSpecsCount { get; set; }
    }
}
