using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified
{
    public interface IClassifiedEmailAlertRepository
    {
        bool SaveNdUsedCarAlertCustomerList(NdUsedCarAlert alertData);
        bool UnsubscribeNdUsedCarAlertCustomer(int ucAlertid, string email, out int cityId);
    }
}
