using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 28-Jul-2017
    /// Summary: Entiry for bike model
    /// 
    /// </summary>
    public class BikeModel
    {
        public uint ModelId { get; set; }
        public string BikeName { get; set; }
        public string VersionId { get; set; }
        public uint Price { get; set; }
    }
}
