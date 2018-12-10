using System;
using System.Collections.Generic;

namespace Carwale.Entity.Classified.CarDetails
{
    [Serializable]
    public class Features : Sortable
    {
        public string CategoryName{ get; set; }
        public List<FeatureItems> FeatureItemList { get; set; }
    }

    [Serializable]
    public class FeatureItems : Sortable
    {
        public string ItemName { get; set; }
        public bool ItemValue { get; set; }
    }

    [Serializable]
    public class FeatureList
    {
        public string CategoryName { get; set; }
        public List<FeatureListItems> Items { get; set; }
    }

    [Serializable]
    public class FeatureListItems
    {
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public int DataTypeId { get; set; }
    }

    [Serializable]
    public class UsedCarFeatures
    {
        public string Features_SafetySecurity { get; set;}
        public string Features_Comfort { get; set; }
        public string Features_Others { get; set; }
    }
}
