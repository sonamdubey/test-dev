using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.Dealers
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
    /// Description :   Entity for holding dealer and version details.
    /// </summary>
    public class VersionPriceEntity
    {
        public uint VersionId { get; set; }
        public uint ItemCategoryId { get; set; }
        public string ItemName { get; set; }
        public uint ItemValue { get; set; }
    }
}
