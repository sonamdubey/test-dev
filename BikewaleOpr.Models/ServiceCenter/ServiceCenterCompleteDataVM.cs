
using BikewaleOpr.Entity.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
