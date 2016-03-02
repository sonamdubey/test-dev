using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Videos
{
    public class videoMakeModelNew : System.Web.UI.Page
    {
        protected uint MakeId, ModelId;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Get make and modelId from the Query string
        /// </summary>
        private void ParseQueryString()
        {
            string modelId = Request.QueryString["model"];
            string makeId = Request.QueryString["make"];
            if (!string.IsNullOrEmpty(modelId))
            {
                UInt32.TryParse(modelId, out ModelId);
            }
            if (!string.IsNullOrEmpty(makeId))
            {
                UInt32.TryParse(makeId, out MakeId);
            }
        }

    }
}