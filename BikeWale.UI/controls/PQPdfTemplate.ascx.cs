using Bikewale.Common;
using Bikewale.Entity.BikeBooking;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Ashwini Todkar on 7 Nov 2014
    /// </summary>
    public class PQPdfTemplate : System.Web.UI.UserControl
    {
        protected Repeater rptQuote, rptOffers, rptFacility;
        protected HtmlGenericControl divOffers, divFacility;

        protected string ImgPath = string.Empty, BikeName = string.Empty, Organization = string.Empty, contactNo = string.Empty, address = string.Empty,
            Oragnization = string.Empty, ContactNo = string.Empty, workingHours = string.Empty,
            ltv = string.Empty, tenure = string.Empty, roi = string.Empty, downPayment = string.Empty, emi = string.Empty, loanAmount = string.Empty;

        protected CalculatedEMI objCEmi  = null;
        protected PQ_DealerDetailEntity objPQ = null;

        public CalculatedEMI CEmi { get; set; }
        public PQ_DealerDetailEntity PQDetails { get; set; }
        public uint VersionId { get; set; }
        public uint DealerId { get; set; }
        public uint Availability { get; set; }
        public uint CityId { get; set; }
        public uint TotalPrice {get;set;}

        public void BindDetails()
        {
            if (PQDetails != null)
            {
                if (!String.IsNullOrEmpty(PQDetails.objQuotation.LargePicUrl))
                {
                    //ImgPath = ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + PQDetails.objQuotation.LargePicUrl, PQDetails.objQuotation.HostUrl);
                    ImgPath = Bikewale.Utility.Image.GetPathToShowImages(PQDetails.objQuotation.OriginalImagePath, PQDetails.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);

                    BikeName = PQDetails.objQuotation.objMake.MakeName + " " + PQDetails.objQuotation.objModel.ModelName + " " + PQDetails.objQuotation.objVersion.VersionName;

                    workingHours = PQDetails.objDealer.WorkingTime;

                    Oragnization = PQDetails.objDealer.Organization;
                    contactNo = PQDetails.objDealer.PhoneNo + (!String.IsNullOrEmpty(PQDetails.objDealer.PhoneNo) && !String.IsNullOrEmpty(PQDetails.objDealer.MobileNo) ? ", " : "") + PQDetails.objDealer.MobileNo;

                    address = PQDetails.objDealer.Address + " ";
                    address += PQDetails.objDealer.objArea.AreaName + ", " + PQDetails.objDealer.objCity.CityName;

                    if (!String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(PQDetails.objDealer.objArea.PinCode))
                    {
                        address += ", " + PQDetails.objDealer.objArea.PinCode;
                    }

                    address += ", " + PQDetails.objDealer.objState.StateName;
                }

                if (PQDetails.objQuotation.PriceList != null && PQDetails.objQuotation.PriceList.Count > 0)
                {
                    rptQuote.DataSource = PQDetails.objQuotation.PriceList;
                    rptQuote.DataBind();
                }

                if (PQDetails.objOffers != null && PQDetails.objOffers.Count > 0)
                {
                    rptOffers.DataSource = PQDetails.objOffers;
                    rptOffers.DataBind();
                }

                if (PQDetails.objFacilities != null && PQDetails.objFacilities.Count > 0)
                {
                    rptFacility.DataSource = PQDetails.objFacilities;
                    rptFacility.DataBind();
                }

                //if (PQDetails.objEmi != null && TotalPrice > 0)
                //{
                //    objCEmi = CommonOpn.GetCalculatedFlatEmi(PQDetails.objEmi, TotalPrice);
                //}
                
                //if (!String.IsNullOrEmpty(_objPQ.objQuotation.LargePicUrl))
                //    ImgPath = ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + _objPQ.objQuotation.LargePicUrl, _objPQ.objQuotation.HostUrl);
                //else
                //    ImgPath = "";
                //BikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;

                //Oragnization = _objPQ.objDealer.Organization;
                //Address =  _objPQ.objDealer.Address ;
                //Area = _objPQ.objDealer.objArea.AreaName + ", " + _objPQ.objDealer.objCity.CityName + "," + _objPQ.objDealer.objArea.PinCode + ", " + _objPQ.objDealer.objState.StateName;

                //ContactNo = _objPQ.objDealer.PhoneNo + (!String.IsNullOrEmpty(_objPQ.objDealer.PhoneNo) && !String.IsNullOrEmpty(_objPQ.objDealer.MobileNo) ? ", " : "") + _objPQ.objDealer.MobileNo;

                //workingHours = _objPQ.objDealer.WorkingTime;

                //if (_objPQ.objQuotation.PriceList != null && _objPQ.objQuotation.PriceList.Count > 0)
                //{
                //    rptQuote.DataSource = _objPQ.objQuotation.PriceList;
                //    rptQuote.DataBind();

                //    uint ex_showrrom = 0;

                //    foreach (var item in _objPQ.objQuotation.PriceList)
                //    {
                //        if (item.CategoryId == 3)
                //            ex_showrrom = item.Price;

                //        TotalPrice = TotalPrice + item.Price;
                //    }

                //    if (_objPQ.objEmi != null)
                //    {
                //        CalculatedEMI _objCEMI = CommonOpn.GetCalculatedReducingEmi(_objPQ.objEmi, ex_showrrom, TotalPrice);


                //        ltv = _objCEMI.objEMI.LoanToValue.ToString();
                //        tenure = _objCEMI.objEMI.Tenure.ToString();
                //        roi = _objCEMI.objEMI.RateOfInterest.ToString();
                //        downPayment = Bikewale.Common.CommonOpn.FormatPrice((_objCEMI.DownPayment.ToString()));
                //        loanAmount = Bikewale.Common.CommonOpn.FormatPrice((_objCEMI.LoanAmount.ToString()));
                //        emi = Bikewale.Common.CommonOpn.FormatPrice(Math.Ceiling(_objCEMI.EMI).ToString());
                //    }

                //}


                //if(_objPQ.objFacilities != null && _objPQ.objFacilities.Count > 0)
                //{
                //    rptFacility.DataSource = _objPQ.objFacilities;
                //    rptFacility.DataBind();
                //    divFacility.Visible = true;
                //}

                //if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                //{
                //    rptOffers.DataSource = _objPQ.objOffers;
                //    rptOffers.DataBind();
                //    divOffers.Visible = true;
                //}

                //string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                //string _requestType = "application/json";
                //string _apiUrl = "/api/Dealers/GetAvailabilityDays/?dealerId=" + DealerId + "&versionId=" + VersionId;
                //// Send HTTP GET requests 

                //numOfDays = BWHttpClient.GetApiResponseSync<uint>(_abHostUrl, _requestType, _apiUrl, numOfDays);
                //Trace.Warn("noOfDays : " + numOfDays);
            }
        }
    }
}