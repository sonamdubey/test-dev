using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity
{
  public  class BannerDetails
    {
        public string HorizontalPosition { get; set; }
        public string VerticalPosition { get; set; }
        public string BackgroundColor { get; set; }

        public string ImageHref { get; set; }

        public string BannerTitle { get; set; }

        public string ButtonPosition { get; set; }

        public string ButtonText { get; set; }

        public string ButtonColor { get; set; }

        public ushort ButtonType { get; set; }

        public ushort TargetHref { get; set; }

        public string JumbotronDepth { get; set; }

        public string HTML { get; set; }

        public string JS { get; set; }

        public string CSS { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
