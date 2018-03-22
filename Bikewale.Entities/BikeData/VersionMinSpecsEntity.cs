using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By  : Rajan Chauhan on 21 Mar 2018
    /// Description : Entity for VersionMinSpec having VersionId and MinSpecsList
    /// </summary>
    public class VersionMinSpecsEntity
    {
        public int VersionId { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}
