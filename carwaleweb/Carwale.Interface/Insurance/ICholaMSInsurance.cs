using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CarData;
using Carwale.Entity.Customers;
using Carwale.Entity.Geolocation;

namespace Carwale.Interfaces.Insurance
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 24 April 2015
    /// </summary>
    public interface ICholaMSInsurance
    {
        void GetQuotationStatus(string cwLeadId, out string statusId, out string status, out string quotation);
        Tuple<CarVersionDetails, CustomerMinimal, States, City,string,string,string> GetCustomerDetails(string cwLeadId);
    }
}
