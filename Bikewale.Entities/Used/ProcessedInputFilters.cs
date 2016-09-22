using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 10 Sept 2016
    /// /// Class to hold all the processed filter values from the client side for search result page
    /// </summary>
    public class ProcessedInputFilters
    {
        public uint CityId { get; set; }
        public string[] Make { get; set; }
        public string[] Model { get; set; }
        public string MinBudget { get; set; }
        public string MaxBudget { get; set; }
        public string Kms { get; set; }        
        public string Age { get; set; }        
        public string[] Owners { get; set; }        
        public string[] SellerTypes { get; set; }

        public ushort SortOrder { get; set; }

        // for paging
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
