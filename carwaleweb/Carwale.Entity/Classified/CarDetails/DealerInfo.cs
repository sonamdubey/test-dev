using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 27 May 2015
    /// </summary>
    [Serializable]
    public class DealerInfo
    {
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        public string CertifiedUrl { get; set; }
        public string CertificationComment { get; set; }
        public string MaskingNumber { get; set; }
        public string MaskingNumberFormatted { get; set; }
        //public string DealerImageUrl { get; set; }
        public string OrganizationName { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public bool IsTestDrive { get; set; }
        public bool IsBookOnline { get; set; }
        public string DealerShowroomUrl { get; set; }
        public string DealerProfileHostUrl { get; set; }
        public string DealerProfileImagePath { get; set; }
        public string RatingText { get; set; }
    }
}
