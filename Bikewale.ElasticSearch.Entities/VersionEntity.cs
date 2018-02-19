using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Entity to store version information.
    /// </summary>
    public class VersionEntity
    {
        public uint VersionId { get; set; }
        public string VersionName { get; set; }
        public IEnumerable<SpecsEntity> Specs { get; set; }
        public IEnumerable<PriceEntity> PriceList { get; set; }
    }
}
