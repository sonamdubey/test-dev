using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.ServiceCenter
{
   // <summary>		
    /// Written By : Snehal Dange 28 July 2017		
    /// Description :All Properties related to service center.Used for adding record to database		
    /// </summary>		
    public class ServiceCenterCompleteData
    {		
      		
        public uint Id { get; set; }		
		
        		
        public string Name { get; set; }		
		
       		
        public string Address { get; set; }		
		
        public StateCityEntity Location { get; set; }		
		
		
		
        public string Phone { get; set; }		
		
       		
        public string Mobile { get; set; }		
		
       		
		
        public string Email { get; set; }		
       		
       		
        public uint AreaId { get; set; }		
		
        		
        public string Pincode { get; set; }		
		
        public double Lattitude { get; set; }		
		
        		
        public double Longitude { get; set; }		
		
       		
        public uint MakeId { get; set; }		
		
       		
		
		
        public uint DealerId { get; set; }		
		
        public ServiceCenterCompleteData()
        {		
            Location = new StateCityEntity();		
        }		
    }
}
