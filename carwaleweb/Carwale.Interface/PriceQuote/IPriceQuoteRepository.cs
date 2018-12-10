using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
    public interface IPriceQuoteRepository<T>
    {
        T GetOnRoadPrice(T t);
        T GetOnRoadPriceandPQId(T t);
        T GetInquiryandQuoteId(T t);
        void UpdateCRMLeadId(T t);
        void UpdateCustomerId(T t);
        bool CanBePushToCRM(T t);
    }
}
