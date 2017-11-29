using System.Collections.Generic;

namespace Bikewale.Models.Survey
{
    public class BajajSurveyVM : ModelBase
    {
        public string CurrentBike { get; set; }
        public string BikeToPurchase { get; set; }
        public string RecentBikeCommercial { get; set; }
        public string SeenThisAd { get; set; }
        public string viewscount { get; set; }
        public string AllMedium { get; set; }

        //public List<string> InternetOptions { get; set; }

        public string Age { get; set; }
        public string Handset { get; set; }
        public string City { get; set; }

        public bool IsSubmitted { get; set; }
        public bool IsMobile { get; set; }
        public string Source { get; set; }

        public List<string> MultipleModel { get; set; }
        public List<string> AdMedium { get; set; }
    }
}