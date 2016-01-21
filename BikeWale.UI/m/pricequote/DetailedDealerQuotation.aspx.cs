using Bikewale.BAL.BikeData;
using Bikewale.BikeBooking;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Mobile.PriceQuote;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.BikeBooking
{
    public class DetailedDealerQuotation : System.Web.UI.Page
    {
        protected PQ_DealerDetailEntity _objPQ = null;
        protected HtmlGenericControl divBookBike, divBikeBooked;
        protected Repeater rptQuote, rptOffers, rptFacility, rptColors, rptDisclaimer, rptPopupColors;
        protected string ImgPath = string.Empty, BikeName = string.Empty, cityId = string.Empty, contactNo = string.Empty, dealerId = string.Empty, organization = string.Empty, address = string.Empty, versionId = string.Empty, MakeModel = string.Empty;
        protected UInt32 TotalPrice = 0, exShowroomCost = 0;
        protected CalculatedEMI objCEMI = null;
        protected double lattitude = 0, longitude = 0;
        protected PQCustomerDetail objCustomer;
        protected Button btnSavePdf;
        protected uint pqId = 0;
        protected PQPdfTemplate PQPdfTemplate;
        protected uint noOfDays = 0;
        protected List<VersionColor> objColors = null;
        protected bool isDealerNotified = false;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;
        protected uint bookingAmount = 0;
        protected bool hasBumperDealerOffer = false;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            this.btnSavePdf.Click += new EventHandler(SaveAsPdf);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                pqId = Convert.ToUInt32(PriceQuoteQueryString.PQId);
                cityId = PriceQuoteQueryString.CityId;
                if (!String.IsNullOrEmpty(PriceQuoteQueryString.DealerId))
                    dealerId = PriceQuoteQueryString.DealerId;
                versionId = PriceQuoteQueryString.VersionId;

                if (!String.IsNullOrEmpty(dealerId) && pqId > 0)
                {
                    getCustomerDetails();
                    GetDetailedQuote();
                    GetBikeAvailability(dealerId, PriceQuoteQueryString.VersionId);
                    GetVersionColors(Convert.ToUInt32(PriceQuoteQueryString.VersionId));
                }
                else
                {
                    Response.Redirect("/m/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/m/pricequote/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        private void DoProcessing()
        {
            var bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            var url = bwHostUrl + "/PQCertificate.aspx?versionId=" + versionId + "&dealerId=" + dealerId + "&CityId=" + cityId + "&availability=" + noOfDays + "&exshowroom=" + exShowroomCost + "&totalPrice=" + TotalPrice;
            var file = WKHtmlToPdf(url);

            if (file != null)
            {
                Response.ContentType = "Application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=DetailedDealerQuotation.pdf");
                Response.BinaryWrite(file);
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
        //private void SaveAsPdf(object sender, EventArgs e)
        //{
        //    string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
        //    string _requestType = "application/json";
        //    string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + PriceQuoteCookie.VersionId + "&DealerId=" + Request.QueryString["dealerId"] + "&CityId=" + cityId;
        //    // Send HTTP GET requests 

        //    PQ_DealerDetailEntity objPQR = BWHttpClient.GetApiResponseSync<PQ_DealerDetailEntity>(_abHostUrl, _requestType, _apiUrl, _objPQ);

        //    if (objPQR != null)
        //    {
        //        _objPQ = objPQR;
        //        string attachment = "attachment; filename=" + "DealerQuotation" + ".pdf";
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", attachment);
        //        Response.ContentType = "application/pdf";

        //        StringWriter s_tw = new StringWriter();
        //        HtmlTextWriter h_textw = new HtmlTextWriter(s_tw);

        //        //PQPdfTemplate._objPQ = objPQR;
        //        //PQPdfTemplate.BikeName = BikeName;
        //        //PQPdfTemplate.ImgPath = ImgPath;
        //        PQPdfTemplate.VersionId = Convert.ToUInt32(PriceQuoteCookie.VersionId);
        //        PQPdfTemplate.DealerId = Convert.ToUInt32(Request.QueryString["dealerId"]);
        //        PQPdfTemplate.BindDetails();
        //        PQPdfTemplate.RenderControl(h_textw);//Name of the Panel

        //        Document doc = new Document(PageSize.A4, 10, 10, 10, 10);
        //        FontFactory.GetFont("Verdana", 40);
        //        PdfWriter.GetInstance(doc, Response.OutputStream);

        //        doc.Open();

        //        StringReader s_tr = new StringReader(s_tw.ToString());
        //        HTMLWorker html_worker = new HTMLWorker(doc);

        //        html_worker.Parse(s_tr);
        //        doc.Close();
        //        Response.Write(doc);
        //    }
        //}

        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// Summary    : PopulateWhere to get availability of bike with dealer e.g. in stock or waiting
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        private void GetBikeAvailability(string dealerId, string versionId)
        {            
            string _apiUrl = "/api/Dealers/GetAvailabilityDays/?dealerId=" + dealerId + "&versionId=" + versionId;
         
            using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
            {
                noOfDays = objClient.GetApiResponseSync<uint>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, noOfDays);
            }            
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// Summary    : PopulateWhere to get dealer price quote, offers, facilities, contact details 
        /// </summary>
        private void GetDetailedQuote()
        {
            try
            {
                string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + PriceQuoteQueryString.VersionId + "&DealerId=" + PriceQuoteQueryString.DealerId + "&CityId=" + cityId;
                
                Trace.Warn("_apiUrl: ", _apiUrl);

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objPQ = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objPQ);
                }                

                if (_objPQ != null && _objPQ.objQuotation != null)
                {

                    ImgPath = Bikewale.Utility.Image.GetPathToShowImages(_objPQ.objQuotation.OriginalImagePath, _objPQ.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                    BikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;
                    MakeModel = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName;
                    //hide book a bike button from page when booking amount is 0
                    if (_objPQ.objBookingAmt != null && _objPQ.objBookingAmt.Amount > 0)
                    {
                        bookingAmount = _objPQ.objBookingAmt.Amount;
                        divBookBike.Visible = true;
                        if (objCustomer!=null && objCustomer.IsTransactionCompleted)
                        {
                            divBikeBooked.Visible = true;
                            divBookBike.Visible = false;
                        }
                    }

                    if (_objPQ.objQuotation.PriceList != null && _objPQ.objQuotation.PriceList.Count > 0)
                    {
                        rptQuote.DataSource = _objPQ.objQuotation.PriceList;
                        rptQuote.DataBind();



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
                            {
                                exShowroomCost += item.Price;
                            }

                            TotalPrice += item.Price;
                        }

                        if (isBasicAvail && isShowroomPriceAvail)
                            TotalPrice = TotalPrice - exShowroomCost;

                        if (_objPQ.objEmi != null && exShowroomCost > 0)
                            objCEMI = CommonOpn.GetCalculatedFlatEmi(_objPQ.objEmi, TotalPrice);

                        foreach (var price in _objPQ.objQuotation.PriceList)
                        {
                            Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), _objPQ.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                        }
                        if (insuranceAmount > 0)
                        {
                            IsInsuranceFree = true;
                        }
                    }

                    if (_objPQ.objQuotation.Disclaimer != null && _objPQ.objQuotation.Disclaimer.Count > 0)
                    {
                        rptDisclaimer.DataSource = _objPQ.objQuotation.Disclaimer;
                        rptDisclaimer.DataBind();
                    }

                    if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                    {

                        rptOffers.DataSource = _objPQ.objOffers;
                        rptOffers.DataBind();
                    }

                    if (_objPQ.objFacilities != null && _objPQ.objFacilities.Count > 0)
                    {

                        rptFacility.DataSource = _objPQ.objFacilities;
                        rptFacility.DataBind();
                    }

                    if (_objPQ.objDealer != null  && objCustomer!=null)
                    {
                        contactNo = _objPQ.objDealer.PhoneNo + (!String.IsNullOrEmpty(_objPQ.objDealer.PhoneNo) && !String.IsNullOrEmpty(_objPQ.objDealer.MobileNo) ? ", " : "") + _objPQ.objDealer.MobileNo;
                        organization = _objPQ.objDealer.Organization;
                        lattitude = _objPQ.objDealer.objArea.Latitude;
                        longitude = _objPQ.objDealer.objArea.Longitude;
                        address = _objPQ.objDealer.objArea.AreaName + ", " + _objPQ.objDealer.objCity.CityName;// +"," + _objPQ.objDealer.objArea.PinCode + ", " + _objPQ.objDealer.objState.StateName;

                        if (!String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(_objPQ.objDealer.objArea.PinCode))
                        {
                            address += ", " + _objPQ.objDealer.objArea.PinCode;
                        }

                        address += ", " + _objPQ.objDealer.objState.StateName;
                        
                        if (!DealerPriceQuoteCookie.IsSMSSend && !DealerPriceQuoteCookie.IsMailSend)
                        {
                            SendEmailSMSToDealerCustomer.SendEmailToCustomer(BikeName, ImgPath, _objPQ.objDealer.Name, _objPQ.objDealer.EmailId, _objPQ.objDealer.MobileNo, _objPQ.objDealer.Organization, _objPQ.objDealer.Address, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail, _objPQ.objQuotation.PriceList, _objPQ.objOffers, _objPQ.objDealer.objArea.PinCode, _objPQ.objDealer.objState.StateName, _objPQ.objDealer.objCity.CityName, TotalPrice, insuranceAmount);
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
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
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

                    if (objColors != null && objColors.Count > 0)
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
    }   //End of class
}   //End of namespace