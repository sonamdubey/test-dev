﻿using Bikewale.BAL.BikeData;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    /// <summary>
    /// Created By : Ashwini Todkar 6 Nov 2014
    /// Modified by : Vivek Gupta on 13 May 2016
    /// Description :  Field versionName added, function signature updated.
    /// </summary>
    public class DetailedDealerQuotation : Page
    {
        protected PQ_DealerDetailEntity _objPQ = null;
        protected Repeater rptQuote, rptOffers, rptFacility, rptColors, rptDisclaimer, rptPopupColors;
        protected SimilarBikes ctrl_similarBikes;
        protected string ImgPath = string.Empty, BikeName = string.Empty, cityId = string.Empty, versionId = string.Empty, dealerId = string.Empty, contactNo = string.Empty, organization = string.Empty, address = string.Empty, MakeModel = string.Empty, VersionName = string.Empty;
        protected double lattitude = 0, longitude = 0;
        protected UInt32 TotalPrice = 0;
        protected CalculatedEMI objCEMI = null;
        protected uint PqId = 0;
        protected DateTime validDate;
        protected PQCustomerDetail objCustomer = null;
        protected bool isMailSend = false, isSMSSend = false;
        protected PQPdfTemplate PQPdfTemplate;
        //protected Button btnSavePdf;//, btnSavePdfControl, btnSaveTest;
        protected HtmlGenericControl divBookBike, divBikeBooked;
        protected uint numOfDays = 0;
        protected List<VersionColor> objColors = null;
        uint exShowroomCost = 0;
        bool isDealerNotified = false;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;
        protected uint bookingAmount = 0;
        protected bool hasBumperDealerOffer = false;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            //this.btnSavePdf.Click += new EventHandler(SaveAsPdf); //Commented By : Sadhana Upadhyay on 27 Jan 2014
            //this.btnSavePdfControl.Click += new EventHandler(SavePdfControl);
            //this.btnSaveTest.Click += new EventHandler(SaveAsPdfTest);

        }

        //private void SaveAsPdfTest(object sender, EventArgs e)
        //{
        //    Test.PQDetails = _objPQ;
        //    Test.BindData();
        //}

        //private void SavePdfControl(object sender, EventArgs e)
        //{

        //    string attachment = "attachment; filename=" + "DetailedDealerQuotation" + ".pdf";
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", attachment);
        //    Response.ContentType = "application/pdf";

        //    StringWriter s_tw = new StringWriter();
        //    HtmlTextWriter h_textw = new HtmlTextWriter(s_tw);

        //    PQPdfTemplate.TotalPrice = TotalPrice;
        //    PQPdfTemplate.VersionId = Convert.ToUInt32(versionId);
        //    PQPdfTemplate.DealerId = Convert.ToUInt32(dealerId);
        //    PQPdfTemplate.CityId = Convert.ToUInt32(cityId);
        //    PQPdfTemplate.PQDetails = _objPQ;
        //    PQPdfTemplate.Availability = numOfDays;
        //    PQPdfTemplate.CEmi = objCEMI;
        //    PQPdfTemplate.BindDetails();
        //    PQPdfTemplate.RenderControl(h_textw);//Name of the Panel

        //    Trace.Warn("html : " + h_textw.ToString());

        //    Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
        //    FontFactory.GetFont("Verdana", 20);
        //    PdfWriter.GetInstance(doc, Response.OutputStream);

        //    doc.Open();

        //    StringReader s_tr = new StringReader(s_tw.ToString());
        //    HTMLWorker html_worker = new HTMLWorker(doc);

        //    html_worker.Parse(s_tr);
        //    doc.Close();
        //    Response.Write(doc);

        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                PqId = Convert.ToUInt32(PriceQuoteQueryString.PQId);

                if (!String.IsNullOrEmpty(PriceQuoteQueryString.DealerId))
                    dealerId = PriceQuoteQueryString.DealerId;
                versionId = PriceQuoteQueryString.VersionId;
                ctrl_similarBikes.VersionId = versionId;
                cityId = PriceQuoteQueryString.CityId;

                if (!String.IsNullOrEmpty(dealerId) && PqId > 0)
                {
                    getCustomerDetails();
                    GetDetailedQuote();
                    GetBikeAvailability();
                    GetVersionColors(Convert.ToUInt32(versionId));
                }
                else
                {
                    Response.Redirect("/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/pricequote/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            //SendSMSnEmail();
        }

        private void DoProcessing()
        {
            //var bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            //var url = bwHostUrl + "/PQCertificate.aspx?versionId=" + versionId + "&dealerId=" + dealerId + "&CityId=" + cityId + "&availability=" + numOfDays + "&exshowroom=" + exShowroomCost + "&totalPrice=" + TotalPrice;
            //var file = WKHtmlToPdf(url);

            //if (file != null)
            //{
            //    Response.ContentType = "Application/pdf";
            //    Response.AddHeader("content-disposition", "attachment;filename=DetailedDealerQuotation.pdf");
            //    Response.BinaryWrite(file);
            //    Response.End();
            //}

            PQPdfTemplate PQPdfTemplate = (PQPdfTemplate)LoadControl("/controls/PQPdfTemplate.ascx");

            PQPdfTemplate.TotalPrice = TotalPrice;
            PQPdfTemplate.VersionId = Convert.ToUInt32(versionId);
            PQPdfTemplate.DealerId = Convert.ToUInt32(dealerId);
            PQPdfTemplate.CityId = Convert.ToUInt32(cityId);
            PQPdfTemplate.PQDetails = _objPQ;
            PQPdfTemplate.Availability = numOfDays;
            PQPdfTemplate.CEmi = objCEMI;
            PQPdfTemplate.BindDetails();

            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            PQPdfTemplate.RenderControl(hw);
            var htmlContent = sb.ToString();

            var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent);

            if (pdfBytes != null)
            {
                Response.ContentType = "Application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=DetailedDealerQuotation.pdf");
                Response.BinaryWrite(pdfBytes);
                Response.End();
            }
        }

        public byte[] WKHtmlToPdf(string url)
        {
            var fileName = " - ";
            var p = new Process();

            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = Path.Combine(Server.MapPath("~/wkhtmltopdf/bin"), "wkhtmltopdf.exe");

            string switches = "";
            switches += "--margin-top 5mm --margin-bottom 5mm --margin-right 0mm --margin-left 0mm ";
            switches += "--page-size Letter ";
            p.StartInfo.Arguments = switches + " " + url + " " + fileName;
            p.Start();

            //read output
            byte[] buffer = new byte[32768];
            byte[] file;
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);

                    if (read <= 0)
                    {
                        break;
                    }
                    ms.Write(buffer, 0, read);
                }
                file = ms.ToArray();
            }

            // wait or exit
            p.WaitForExit(0);

            // read the exit code, close process
            int returnCode = p.ExitCode;
            p.Close();

            return returnCode == 0 ? file : null;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// PopulateWhere save quote certificate as pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsPdf(object sender, EventArgs e)
        {
            DoProcessing();
        }



        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// Summary    : PopulateWhere to get availability of bike with dealer e.g. in stock or waiting
        /// Modified By : Lucky Rathore on 13 May 2016
        /// Description : Signature of SendEmailToCustomer() changed.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        private void GetBikeAvailability()
        {
            string _apiUrl = "/api/Dealers/GetAvailabilityDays/?dealerId=" + dealerId + "&versionId=" + versionId;

            using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
            {
                numOfDays = objClient.GetApiResponseSync<uint>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, numOfDays);
            }

            Trace.Warn("noOfDays : " + numOfDays);
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// Summary    : PopulateWhere to get dealer price quote, offers, facilities, contact details 
        /// </summary>
        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + versionId + "&DealerId=" + dealerId + "&CityId=" + cityId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objPQ = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objPQ);
                }

                if (_objPQ != null && _objPQ.objQuotation != null && objCustomer != null)
                {
                    //_objPQ.objQuotation.HostUrl + _objPQ.objQuotation.LargePicUrl + _objPQ.objQuotation.objMake.MakeName + _objPQ.objQuotation.objModel.ModelName + _objPQ.objQuotation.objVersion.VersionName +
                    ImgPath = Bikewale.Utility.Image.GetPathToShowImages(_objPQ.objQuotation.OriginalImagePath, _objPQ.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                    BikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;
                    MakeModel = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName;
                    VersionName = _objPQ.objQuotation.objVersion.VersionName;
                    //hide book a bike button if booking amount is not available for a bike
                    //Check if the customer already paid for this bike

                    if (_objPQ.objBookingAmt != null && _objPQ.objBookingAmt.Amount > 0)
                    {
                        bookingAmount = _objPQ.objBookingAmt.Amount;
                        divBookBike.Visible = true;
                        if (objCustomer.IsTransactionCompleted)
                        {
                            divBikeBooked.Visible = true;
                            divBookBike.Visible = false;
                        }

                    }

                    if (_objPQ.objQuotation.PriceList != null && _objPQ.objQuotation.PriceList.Count > 0)
                    {
                        rptQuote.DataSource = _objPQ.objQuotation.PriceList;
                        rptQuote.DataBind();

                        foreach (var price in _objPQ.objQuotation.PriceList)
                        {
                            Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), _objPQ.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                        }
                        if (insuranceAmount > 0)
                        {
                            IsInsuranceFree = true;
                        }

                        bool isShowroomPriceAvail = false, isBasicAvail = false;

                        foreach (var item in _objPQ.objQuotation.PriceList)
                        {
                            //Check if Ex showroom price for a bike is available CategoryId = 3 (exshowrrom)
                            if (item.CategoryId == 3)
                            {
                                isShowroomPriceAvail = true;
                                exShowroomCost = item.Price;
                            }

                            //if Ex showroom price for a bike is not available  then set basic cost for bike price CategoryId = 1 (basic bike cost)
                            if (!isShowroomPriceAvail && item.CategoryId == 1)
                            {
                                exShowroomCost += item.Price;
                                isBasicAvail = true;
                            }

                            if (item.CategoryId == 2 && !isShowroomPriceAvail)
                                exShowroomCost += item.Price;

                            TotalPrice += item.Price;
                        }

                        if (isBasicAvail && isShowroomPriceAvail)
                            TotalPrice = TotalPrice - exShowroomCost;

                        //if (_objPQ.objEmi != null && TotalPrice > 0)
                        //{
                        //    objCEMI = CommonOpn.GetCalculatedFlatEmi(_objPQ.objEmi, TotalPrice);
                        //}

                    }

                    if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                    {

                        rptOffers.DataSource = _objPQ.objOffers;
                        rptOffers.DataBind();
                    }


                    if (_objPQ.objQuotation.Disclaimer != null && _objPQ.objQuotation.Disclaimer.Count > 0)
                    {
                        rptDisclaimer.DataSource = _objPQ.objQuotation.Disclaimer;
                        rptDisclaimer.DataBind();
                    }

                    if (_objPQ.objFacilities != null && _objPQ.objFacilities.Count > 0)
                    {

                        rptFacility.DataSource = _objPQ.objFacilities;
                        rptFacility.DataBind();
                    }

                    if (_objPQ.objDealer != null)
                    {
                        contactNo = _objPQ.objDealer.PhoneNo + (!String.IsNullOrEmpty(_objPQ.objDealer.PhoneNo) && !String.IsNullOrEmpty(_objPQ.objDealer.MobileNo) ? ", " : "") + _objPQ.objDealer.MobileNo;
                        organization = _objPQ.objDealer.Organization;
                        lattitude = _objPQ.objDealer.objArea.Latitude;
                        longitude = _objPQ.objDealer.objArea.Longitude;
                        address = _objPQ.objDealer.objArea.AreaName + ", " + _objPQ.objDealer.objCity.CityName;

                        if (!String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(_objPQ.objDealer.objArea.PinCode))
                        {
                            address += ", " + _objPQ.objDealer.objArea.PinCode;
                        }

                        address += ", " + _objPQ.objDealer.objState.StateName;

                        if (!DealerPriceQuoteCookie.IsSMSSend && !DealerPriceQuoteCookie.IsMailSend)
                        {
                            SendEmailSMSToDealerCustomer.SendEmailToCustomer(BikeName, ImgPath, _objPQ.objDealer.Name, _objPQ.objDealer.EmailId,
                               _objPQ.objDealer.MobileNo, _objPQ.objDealer.Organization, _objPQ.objDealer.Address, objCustomer.objCustomerBase.CustomerName,
                               objCustomer.objCustomerBase.CustomerEmail, _objPQ.objQuotation.PriceList, _objPQ.objOffers, _objPQ.objDealer.objArea.PinCode,
                               _objPQ.objDealer.objState.StateName, _objPQ.objDealer.objCity.CityName, TotalPrice, VersionName, _objPQ.objDealer.objArea.Latitude, _objPQ.objDealer.objArea.Longitude, _objPQ.objDealer.WorkingTime);

                            hasBumperDealerOffer = Bikewale.Utility.OfferHelper.HasBumperDealerOffer(_objPQ.objDealer.DealerId.ToString(), "");
                            if (bookingAmount > 0)
                            {
                                SendEmailSMSToDealerCustomer.SMSToCustomer(objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName, BikeName, _objPQ.objDealer.Name, _objPQ.objDealer.MobileNo, _objPQ.objDealer.Address + "" + address, bookingAmount, insuranceAmount, hasBumperDealerOffer);
                            }

                            if (!IsDealerNotified())
                            {
                                SendEmailSMSToDealerCustomer.SendEmailToDealer(_objPQ.objQuotation.objMake.MakeName, _objPQ.objQuotation.objModel.ModelName, _objPQ.objQuotation.objVersion.VersionName, _objPQ.objDealer.Name, _objPQ.objDealer.EmailId, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.cityDetails.CityName, _objPQ.objQuotation.PriceList, Convert.ToInt32(TotalPrice), _objPQ.objOffers, ImgPath, insuranceAmount);
                                SendEmailSMSToDealerCustomer.SMSToDealer(_objPQ.objDealer.MobileNo, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, BikeName, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.cityDetails.CityName);
                            }
                            DealerPriceQuoteCookie.CreateDealerPriceQuoteCookie(PriceQuoteQueryString.PQId, true, true);
                        }
                    }
                }
                else
                {
                    _isContentFound = false;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        // <summary>
        /// created By : Sadhana Upadhyay on 11 Nov 2014
        /// Summary : Get Customer Details 
        /// </summary>
        protected void getCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteQueryString.PQId));
            }

        }
        public void GetVersionColors(uint versionId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
                    IBikeVersions<BikeVersionEntity, uint> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();

                    objColors = objVersion.GetColorByVersion(versionId);

                    if (objColors.Count > 0 && objColors != null)
                    {
                        rptColors.DataSource = objColors;
                        rptColors.DataBind();
                        rptPopupColors.DataSource = objColors;
                        rptPopupColors.DataBind();
                    }
                    else
                        divBookBike.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 Jan 2015
        /// Summary : To check dealer notified or not
        /// </summary>
        /// <returns></returns>
        protected bool IsDealerNotified()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isDealerNotified = objDealer.IsDealerNotified(Convert.ToUInt32(dealerId), objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isDealerNotified;
        }
    }// End of class
}//End of namespace