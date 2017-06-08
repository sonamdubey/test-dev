using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Survey
{
    public class BajajSurveyVM
    {
        public string CurrentBike { get; set; }
        public string BikeToPurchase { get; set; }
        public string AdSeenForModel { get; set; }

        public string HasUsedSeenAd { get; set; }

        public string City { get; set; }
        public string ModelName { get; set; }
        public List<string> MultipleModel { get; set; }
        public List<string> InternetOptions { get; set; }
        public string OtherBike { get; set; }
        public string IsLongerFormat { get; set; }

        public string Age { get; set; }
        public string Handset { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsMobile { get; set; }
    }
}