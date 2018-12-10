using System;
using Carwale.Entity.Common;

namespace Carwale.Entity.Price
{
    [Serializable]
    public class ChargeBase : IdName
    {
        public int ChargeGroupId { get; set; }
    }
}
