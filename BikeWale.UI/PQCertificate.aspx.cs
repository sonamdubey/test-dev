﻿using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    public class PQCertificate : System.Web.UI.Page
    {
        protected string ImgPath = string.Empty, contactNo =string.Empty, address = string.Empty, Organization = string.Empty;
        protected string BikeName = string.Empty;
        protected Repeater rptQuote, rptOffers, rptFacility, rptDisclaimer;
        protected PQ_DealerDetailEntity objPQ = null;
        protected uint noOfDays = 0, exShowroomCost = 0, TotalPrice = 0;
        protected CalculatedEMI objCEMI = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + Request.QueryString["versionId"]+ "&DealerId=" + Request.QueryString["dealerId"] + "&CityId=" + Request.QueryString["cityId"];
        
            using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
            {
                objPQ = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objPQ);
            }            

            if (objPQ != null)
            {
                ImgPath = Bikewale.Utility.Image.GetPathToShowImages(objPQ.objQuotation.OriginalImagePath, objPQ.objQuotation.HostUrl,Bikewale.Utility.ImageSize._210x118);
                BikeName = objPQ.objQuotation.objMake.MakeName + " " + objPQ.objQuotation.objModel.ModelName + " " + objPQ.objQuotation.objVersion.VersionName;
                noOfDays = Convert.ToUInt32(Request.QueryString["availability"]);
                TotalPrice = Convert.ToUInt32(Request.QueryString["totalPrice"]);
                Trace.Warn("total price", TotalPrice.ToString());
                Organization = objPQ.objDealer.Organization;
                contactNo = objPQ.objDealer.PhoneNo + (!String.IsNullOrEmpty(objPQ.objDealer.PhoneNo) && !String.IsNullOrEmpty(objPQ.objDealer.MobileNo) ? ", " : "") + objPQ.objDealer.MobileNo;
                address = objPQ.objDealer.objArea.AreaName + ", " + objPQ.objDealer.objCity.CityName;

                if (!String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(objPQ.objDealer.objArea.PinCode))
                {
                    address += ", " + objPQ.objDealer.objArea.PinCode;
                }

                address += ", " + objPQ.objDealer.objState.StateName;

                if (objPQ.objQuotation.PriceList != null && objPQ.objQuotation.PriceList.Count > 0)
                {
                    rptQuote.DataSource = objPQ.objQuotation.PriceList;
                    rptQuote.DataBind();
                }

                if (objPQ.objOffers != null && objPQ.objOffers.Count > 0)
                {
                    rptOffers.DataSource = objPQ.objOffers;
                    rptOffers.DataBind();
                }

                if (objPQ.objFacilities != null && objPQ.objFacilities.Count > 0)
                {
                    rptFacility.DataSource = objPQ.objFacilities;
                    rptFacility.DataBind();
                }
                if (objPQ.objQuotation.Disclaimer != null && objPQ.objQuotation.Disclaimer.Count > 0)
                {
                    rptDisclaimer.DataSource = objPQ.objQuotation.Disclaimer;
                    rptDisclaimer.DataBind();
                }
            }
        }
    }
}