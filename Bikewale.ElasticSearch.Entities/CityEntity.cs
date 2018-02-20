using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 20 Feb 2018
    /// Description: Entity to store city details.
    /// </summary>
    public class CityEntity
    {
        public uint CityId { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }
    }
}
