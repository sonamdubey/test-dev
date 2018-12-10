using Carwale.Entity.Classified.CarValuation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface ICTBuyingIndexClient
    {
        int GetBuyingIndex(int inquiryId);
        int GetBuyingIndex(ValuationUrlParameters valuationUrlParameters);
    }
}
