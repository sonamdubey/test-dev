﻿using Bikewale.Common;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale
{
    public class OfferTerms : System.Web.UI.Page
    {
        public string htmlContent = string.Empty;
        public bool isExpired = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                string MaskingName = Convert.ToString(Request.QueryString["id"]);
                MaskingName = MaskingName.Replace("/", string.Empty);
                if(!string.IsNullOrEmpty(MaskingName))
                {
                    htmlContent = GetHtml(MaskingName,out isExpired);
                    if (string.IsNullOrEmpty(htmlContent))
                        Server.Transfer("default.aspx");
                }
            }
        }
        /// <summary>
        /// Written by: Sangram Nandkhile on 8 Oct 2015
        /// Function to get Terms and condition for Masking Name
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        protected string GetHtml(string maskingName, out bool isExpired)
        {
            isExpired = default(bool);
            try
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                OfferHtmlEntity objTerms = null;
                string _apiUrl = "/api/DealerPriceQuote/GetOfferTerms?offerMaskingName=" + maskingName + "&offerId=0";
                // Send HTTP GET requests 
                objTerms = BWHttpClient.GetApiResponseSync<OfferHtmlEntity>(_abHostUrl, _requestType, _apiUrl, objTerms);
                if (objTerms != null)
                {
                    isExpired = objTerms.IsExpired;
                    return objTerms.Html;
                }
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}