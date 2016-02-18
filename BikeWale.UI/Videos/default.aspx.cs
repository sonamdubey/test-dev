using Bikewale.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    public class Default : System.Web.UI.Page
    {

        protected Bikewale.Controls.Videos ctrlVideosLanding;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            //DeviceDetection dd = new DeviceDetection();
            //dd.DetectDevice();
            ctrlVideosLanding.CategoryId = Entities.Videos.EnumVideosCategory.FeaturedAndLatest;
            ctrlVideosLanding.TotalRecords = 5;
        }
    }
}