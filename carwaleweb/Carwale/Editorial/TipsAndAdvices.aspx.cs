using System;
using System.Collections.Generic;
using System.Web;
using Carwale.UI.Common;

namespace Carwale.UI.Editorial
{
    /// <summary>
    /// Summary description for TipsAndAdvicesCategories
    /// </summary>

    public class TipsAndAdvices : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             Code created By : Supriya Khartode
             Created Date : 11/12/2013
             Note : This is the code used for device detection to integrate mobile website with desktop website
            */

            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            /*	Code added by Supriya Khartode ends here */
        }
    }
}