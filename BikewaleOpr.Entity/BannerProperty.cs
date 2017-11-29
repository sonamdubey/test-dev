using System;

namespace BikewaleOpr.Entity
{
    public class BannerProperty
    {
        public DateTime StartDate { get; set; }        
        public DateTime EndDate { get; set; }        
        public string BannerDescription { get; set; }               
        public uint BannerId { get; set; }  
        public uint IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
