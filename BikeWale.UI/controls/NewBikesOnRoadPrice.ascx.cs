﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public  class NewBikesOnRoadPrice : System.Web.UI.UserControl
    {
        public string PageId { get; set; }
        public int PQSourceId { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}