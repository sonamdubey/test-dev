
namespace Bikewale.Entities.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Entity created to store input parameters for getting customized filters
    /// </summary>
    public class CustomInputFilters
    {
        public ushort MinMileage { get; set; }
        public long MinPrice { get; set; }
        public float MinDisplacement { get; set; }
        public uint MakeCategoryId { get; set; }
    }
}
