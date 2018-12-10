using System;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class Cities
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public int StdCode { get; set; }
        public bool IsPopular { get; set; }
        public DateTime CityEntryDate { get; set; }
        public string CityMaskingName { get; set; }
        public int BWCityOrder { get; set; }
        public string StateName { get; set; }
        public string StateMaskingName { get; set; }
        public bool IsAreaAvailable { get; set; }
        public bool IsDuplicateCityName { get; set; }
    }
}
