using Bikewale.DTO.PriceQuote.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.PriceQuote
{
    public class BookingSummary_New : System.Web.UI.Page
    {
        protected string pageName, description, bikeName, keywords;
        protected Button btnMakePayment;
        protected string dealerId, versionId, cityId, pqId, clientIP, pageUrl;
        protected BookingPageDetailsDTO objBookingPageDetailsDTO = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnMakePayment.Click += new EventHandler(btnMakePayment_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessCookie();
        }

        void btnMakePayment_click(object Sender, EventArgs e)
        {

        }

        #region Private Method
        private void ProcessCookie()
        {
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                dealerId = !String.IsNullOrEmpty(PriceQuoteCookie.DealerId) ? PriceQuoteCookie.DealerId : "0";
                versionId = PriceQuoteCookie.VersionId;
                cityId = PriceQuoteCookie.CityId;
                dealerId = PriceQuoteCookie.PQId;
                if (Convert.ToUInt32(dealerId) > 0)
                {
                    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                    pageUrl = Request.ServerVariables["URL"];
                }
                else
                {
                    Response.Redirect("/", true);
                }
            }
            else
            {
                Response.Redirect("/", true);
            }
        }
        #endregion
    }
}