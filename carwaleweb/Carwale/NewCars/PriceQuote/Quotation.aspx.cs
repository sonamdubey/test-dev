using Carwale.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Carwale.UI.NewCars.PriceQuote
{
    public partial class Quotation : Page
    {
        protected string CompleteQS { get; set; }

        void Page_Load(object o, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["pqid"]) && !string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                DeviceDetection dd = new DeviceDetection(string.Format("/research/quotation.aspx?pqid={0}&t={1}", Request.QueryString["pqid"], Request.QueryString["t"]));
                dd.DetectDevice();

                CompleteQS = Request.QueryString.ToString();
            }
        }
    }
}