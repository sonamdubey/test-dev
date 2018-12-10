using System;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class AreaCode
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int Pincode { get; set; }
    }
}
