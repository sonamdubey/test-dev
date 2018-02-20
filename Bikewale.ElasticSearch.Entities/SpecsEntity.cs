using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Entity to store specification type and its value.
    /// </summary>
    public class SpecsEntity
    {
        public string SpecType { get; set; }
        public string SpecValue { get; set; }
    }
}
