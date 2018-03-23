using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By  : Rajan Chauhan on 21 Mar 2018
    /// Description : Entity for VersionMinSpec having VersionId and MinSpecsList
    /// </summary>
    [Serializable]
    public class VersionMinSpecsEntity
    {
        public int VersionId { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}
