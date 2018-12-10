namespace Carwale.DTOs.Geolocation
{
    public class LocationsDTO
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }
        public bool IsAreaAvailable { get; set; }
        public bool IsDuplicate { get; set; }
        public string AreaName { get; set; }
        public int AreaId { get; set; }
        public string PinCode { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
