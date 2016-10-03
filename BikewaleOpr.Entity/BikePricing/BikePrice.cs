using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.BikePricing
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 Sept 2016
    /// Class to hold the bike price 
    /// </summary>
    public class BikePrice
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public uint VersionId { get; set; }
        public string Price { get; set; }
        public string RTO { get; set; }
        public string Insurance { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
