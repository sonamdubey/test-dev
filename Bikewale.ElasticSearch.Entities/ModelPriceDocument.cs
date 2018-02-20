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
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public uint MakeId { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public uint CityId { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }
        public IEnumerable<VersionEntity> VersionPrice { get; set; }
    }
}
