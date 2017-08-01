using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla
    /// Description :   Entity holding primary dealer information.
    /// </summary>
    public class DealerMakeEntity
    {
        public uint DealerId { get; set; }
        public string DealerName { get; set; }
        public uint MakeId { get; set; }
    }
}
