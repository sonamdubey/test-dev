
namespace Bikewale.Entities.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Entity created to store input parameters for getting customized filters
    /// </summary>
    public class CustomInputFilters
    {
        public int MinMileage { get; set; }
        public int MinPrice { get; set; }
        public int MinDisplacement { get; set; }
        public uint MakeCategoryId { get; set; }
    }
}
