using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces.PriceQuote;
using Carwale.Entity.PriceQuote;
namespace Carwale.Interfaces.PriceQuote
{
    public interface IPQ<T>
    {
         List<T> GetPQByIds(string pqIds);
         List<T> GetPQ(PQInput pqInputes);        
    }
}
