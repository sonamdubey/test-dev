using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCarSearch
{
    /* Author: Rakesh Yadav
     * Date Created: 19 June 2013
    */
    public class ModelVersion
    {
        public string Name { get; set; }
        public string Features { get; set; }
        public string ExShowroomPrice { get; set; }
        public string VersionUrl { get; set; }
        public bool IsMatchingCriteria { get; set; }
        public string VersionId { get; set; }
        public string FuelType { get; set; }
    }
}
