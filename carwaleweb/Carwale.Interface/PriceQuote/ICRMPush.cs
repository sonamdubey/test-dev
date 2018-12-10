using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity;

namespace Carwale.Interfaces.PriceQuote
{
    public interface ICRMPush<T>
    {
        bool CanBePushToCRM(T t);
        void UpdateCRMResponseStatus(T t);
        void UpdateCustomerId(T t);
    }
}
