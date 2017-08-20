using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.ServiceCenter
{
  /// <summary>		
    /// Created By : Snehal Dange		
    /// Created On  : 28 July 2017		
    /// Description : Service center Details .Entity with some data to be displayed on page.		
    /// </summary>		
   public class ServiceCenterDetails
    {		
            public uint Id { get; set; }		
            public string Name { get; set; }		
            public string Address { get; set; }		
            public string Phone { get; set; }		
            public string Mobile { get; set; }		
            public string Lattitude { get; set; }		
            public string Longitude { get; set; }		
            public string CityMaskingName { get; set; }		
            public string MakeMaskingName { get; set; }		
        }
}
