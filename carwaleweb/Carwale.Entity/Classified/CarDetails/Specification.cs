using System;
using System.Collections.Generic;

namespace Carwale.Entity.Classified.CarDetails
{
    [Serializable]
    public class Specification : Sortable
    {
        public ushort CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SpecItems> SpecificationList { get; set; }
    }

    [Serializable]
    public class SpecItems : Sortable
    {
        public string SpecName { get; set; }
        public string SpecValue { get; set; }
        public string SpecUnit { get; set; }
    }

    [Serializable]
    public class SpecificationList
    {
        public string CategoryName { get; set; }
        public List<SpecificationListItems> Items { get; set; }
    }

    public class SpecificationListItems
    {
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public string ItemUnit { get; set; }
    }
}
