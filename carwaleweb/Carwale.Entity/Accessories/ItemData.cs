using Carwale.Entity.Common;
using System;
using System.Collections.Generic;

namespace Carwale.Entity.Accessories.Tyres
{
    [Serializable]
    public class ItemData
    {
        public ItemSummary ItemSummary { get; set; }

        public List<IdName> FeatureCategories { get; set; }

        public List<ItemFeature> ItemFeatures { get; set; }
    }
}
