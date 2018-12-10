using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IPQConcreteClass<T>
    {
        void GetPriceQuote(T t);
    }
}

