using System;
using System.Collections.Generic;

namespace Carwale.Entity.Price
{
    [Serializable]
    public class Charge : ChargeBase
    {
        public int Scope { get; set; }
        public string Explanation { get; set; }
        public int SortOrder { get; set; }
        public List<ChargeBase> Components { get; private set; }

        public Charge()
        {
            Components = new List<ChargeBase>();
        }
    }
}
