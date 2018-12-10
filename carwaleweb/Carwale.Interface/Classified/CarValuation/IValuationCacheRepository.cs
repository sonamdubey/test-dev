using Carwale.Entity.Classified.CarValuation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.CarValuation
{
    public interface IValuationCacheRepository
    {
        Valuation GetValuation(ValuationUrlParameters valuationUrlParameters);
    }
}
