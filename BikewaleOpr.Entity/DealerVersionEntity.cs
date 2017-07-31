using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
    /// Description :   Entity for holding version and price details.
    /// </summary>
    public class DealerVersionEntity
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public uint VersionId { get; set; }
        public uint NumberOfDays { get; set; }
        public uint BikeModelId { get; set; }
    }
}
