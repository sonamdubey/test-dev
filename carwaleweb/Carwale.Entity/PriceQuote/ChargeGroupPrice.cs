using System;
using System.Collections.Generic;

namespace Carwale.Entity.Price
{
    [Serializable]
    public class ChargeGroupPrice : ChargeGroup
    {
        public List<ChargePrice> ChargePrice { get; private set; }

        public ChargeGroupPrice()
        {
            ChargePrice = new List<ChargePrice>();
        }
    }
}
