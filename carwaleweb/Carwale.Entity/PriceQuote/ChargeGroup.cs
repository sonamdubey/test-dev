using Carwale.Entity.Common;
using System;

namespace Carwale.Entity.Price
{
    [Serializable]
    public class ChargeGroup : IdName
    {
        public int SortOrder { get; set; }
        public int Type { get; set; }
        public int SelectionOption { get; set; }
        public bool HasComponents { get; set; }
        public string Explanation { get; set; }
    }
}
