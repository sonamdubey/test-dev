using BikewaleOpr.Entity.ServiceCenter;

namespace BikewaleOpr.Models.ServiceCenter
{
    public class ServiceCenterCompleteDataVM
    {		
		
       public ServiceCenterCompleteData details { get; set; }		
		
       public string MakeName { get; set; }		
		
       		
		
        public ServiceCenterCompleteDataVM()
        {		
            details = new ServiceCenterCompleteData();		
        }		
    }
}
