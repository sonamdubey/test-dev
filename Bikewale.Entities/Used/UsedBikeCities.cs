
namespace Bikewale.Entities.Used
{
    public class UsedBikeCities : Location.CityEntityBase
    {
        public uint bikesCount { get; set; }
        public uint priority { get; set; }
    }
}
