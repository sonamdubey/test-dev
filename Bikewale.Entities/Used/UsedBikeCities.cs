
namespace Bikewale.Entities.Used
{    /// <summary>
    /// Created By : subodh jain 06 oct 2016
    /// Description : Bike Count and city details
    /// </summary>
    public class UsedBikeCities : Location.CityEntityBase
    {
        public uint bikesCount { get; set; }
        public uint priority { get; set; }
    }
}
