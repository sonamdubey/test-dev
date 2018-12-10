using System;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class City : States
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
		public string CityMaskingName { get; set; }
		public int GroupMasterId { get; set; }
        public string GroupName { get; set; }
        public bool IsDuplicateCityName { get; set; }
    }
}
