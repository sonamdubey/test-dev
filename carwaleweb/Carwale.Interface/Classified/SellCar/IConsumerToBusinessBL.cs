using Carwale.Entity.Classified.SellCarUsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface IConsumerToBusinessBL
    {
        void PushToIndividualStockQueue(int inquiryId, int hotLeadPrice = 0, bool optForGuaranteedSales = false);
        void PushToIndividualStockQueue(int tempInquiryId, C2BActionType action, int inquiryId = -1, int hotLeadPrice = 0, bool optForGuaranteedSales = false);
    }
}
