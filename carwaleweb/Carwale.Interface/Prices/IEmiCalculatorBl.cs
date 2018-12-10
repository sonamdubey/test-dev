using Carwale.Entity.Price;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.Prices
{
    public interface IEmiCalculatorBl
    {
        int CalculateDownPaymentDefaultValue(int downPaymentMinValue, int exShowroom);
        Tuple<int, int> GetDownPaymentValues(List<ChargeGroupPrice> compulsoryPrices);
    }
}
