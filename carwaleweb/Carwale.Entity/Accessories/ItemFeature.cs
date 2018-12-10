using System;

namespace Carwale.Entity.Accessories.Tyres
{
    [Serializable]
    public class ItemFeature
    {
        public int Id { get; set; }
        public int FeatureCategoryId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
