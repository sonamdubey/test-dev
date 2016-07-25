using Bikewale.Entities.BikeBooking;
using System;
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

        protected CalculatedEMI objCEmi = null;
        protected PQ_DealerDetailEntity objPQ = null;

        public CalculatedEMI CEmi { get; set; }
        public PQ_DealerDetailEntity PQDetails { get; set; }
        public uint VersionId { get; set; }
        public uint DealerId { get; set; }
        public uint Availability { get; set; }
        public uint CityId { get; set; }
        public uint TotalPrice { get; set; }

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
            }
        }
    }
}