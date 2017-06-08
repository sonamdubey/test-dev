﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Survey
{
    public class BajajSurveyVM : ModelBase
    {
        public string CurrentBike { get; set; }
        public string BikeToPurchase { get; set; }
        public string RecentBikeCommercial { get; set; }
        public string SeenThisAd { get; set; }
        public string viewscount { get; set; }
        public string AdMedium { get; set; }

        //public List<string> InternetOptions { get; set; }

        public string Age { get; set; }
        public string Handset { get; set; }
        public string City { get; set; }

        public bool IsSubmitted { get; set; }
        public bool IsMobile { get; set; }

        public List<string> MultipleModel { get; set; }
    }
}