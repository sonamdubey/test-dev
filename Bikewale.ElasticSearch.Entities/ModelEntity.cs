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
    /// Description: Bike Model
    /// </summary>
    public class ModelEntity
    {
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public BikeStatus ModelStatus { get; set; }

    }
}
