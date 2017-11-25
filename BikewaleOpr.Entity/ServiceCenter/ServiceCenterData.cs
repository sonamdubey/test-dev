using System.Collections.Generic;

namespace BikewaleOpr.Entity.ServiceCenter
{
    /// <summary>		
    /// Created By : Snehal Dange		
    /// Created On  : 28 July 2017		
    /// Description : Service center data with Count and List of service centers		
    /// </summary>		

    public class ServiceCenterData
    {		
        public uint Count { get; set; }		
        public IEnumerable<ServiceCenterDetails> ServiceCenters { get; set; }		
    }
}
