using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.TrackDay
{
    public partial class Details : System.Web.UI.Page
    {
        protected bool androidApp;
        protected ushort articleid;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            string isApp = Request["isapp"];
            if (!string.IsNullOrEmpty(Request["isapp"]))
            {
                androidApp = true;
            }

            if (!string.IsNullOrEmpty(Request["articleid"]))
            {
                articleid = Convert.ToUInt16(Request["articleid"]);
            }

        }
    }
}