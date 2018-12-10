using System;

namespace Carwale.Entity.Price
{
    [Serializable]
    public class ChargePrice 
    {
        public Charge Charge { get; set; }
        public int Price { get; set; }
    }
}
