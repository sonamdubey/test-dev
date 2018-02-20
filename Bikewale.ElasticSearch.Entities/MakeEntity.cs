using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Bike Make
    /// </summary>
    public class MakeEntity
    {
        public uint MakeId { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public BikeStatus MakeStatus { get; set; }

    }
}
