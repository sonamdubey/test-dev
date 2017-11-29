using System;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By :Snehal Dange on 21st Nov 2017
    /// Description: Entity for footer categories
    /// </summary>
    [Serializable]
    public class MakeFooterCategory
    {
        public uint CategoryId { get; set; }
        public uint MakeId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
    }
}
