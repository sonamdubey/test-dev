using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using AjaxPro;
using Newtonsoft.Json;
using Carwale.Entity;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces;
using AEPLCore.Cache;
using Carwale.Cache.CMS;


namespace Carwale.UI.Editorial
{
    public class VideoDefault : System.Web.UI.Page
    {
        protected int pageId = 55;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
        }

    }//class
}//namespace