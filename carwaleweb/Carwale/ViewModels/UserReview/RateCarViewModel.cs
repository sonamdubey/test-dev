using Carwale.Entity.CarData;
using Carwale.Entity.UserReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.ViewModels.UserReview
{
    public class RateCarViewModel
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
		public string VersionName { get; set; }
		public int VersionId { get; set; }
		public string ModelImageSmall { get; set; }
        public List<CarVersions> Versions { get; set; }
        public RateCarDetails RateCarDetails { get; set; }
		public bool IsMobile { get; set; }
	}
}