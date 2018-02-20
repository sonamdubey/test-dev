using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Document structure for ModelPrice ES Index.
    /// </summary>
    public class ModelPriceDocument : Document
    {
        public ModelEntity BikeModel { get; set; }
        public MakeEntity BikeMake { get; set; }
        public CityEntity City { get; set; }
        public IEnumerable<VersionEntity> VersionPrice { get; set; }
    }
}
