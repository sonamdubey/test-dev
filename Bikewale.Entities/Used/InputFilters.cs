using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 10 Sept 2016
    /// Class to hold all the raw filter values from the client side for search result page
    /// </summary>
    public class InputFilters
    {
        public uint CityId { get; set; }
        public string Makes { get; set; }
        public string Models { get; set; }
        public string Budget { get; set; }
        public string Kms { get; set; }
        public string Age { get; set; }
        public string Owners { get; set; }
        public string ST { get; set; }
        public ushort SO { get; set; }
        public int PN { get; set; }
        public int PS { get; set; }
    }
}
