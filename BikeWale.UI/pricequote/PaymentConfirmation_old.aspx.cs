﻿using Bikewale.BikeBooking;
using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Mobile.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore on 11 May 2016.
    /// Summary : Template Revamped.
    /// </summary> 
    public class PaymentConfirmation_old : System.Web.UI.Page
    {
        protected Repeater rptOffers;

        protected PQ_DealerDetailEntity _objPQ = null;
        protected PQCustomerDetail objCustomer = null;
        protected double lattitude, longitude;
        protected uint totalPrice = 0;
        protected UInt32 BooingAmt = 0;
        protected string contactNo = string.Empty, organization = string.Empty, address = string.Empty, bikeName = string.Empty, MakeModel = string.Empty, VersionName = string.Empty, 
            bookingRefNum = string.Empty, WorkingTime = string.Empty;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Modified By : Lucky Rathore on 11 May 2016.
        /// Summary : paramete to call BookingEmailToCustomer() updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection();
            dd.DetectDevice();
            string bikeColor = String.Empty;
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                try
                {
                    GetDetailedQuote();
                    getCustomerDetails();
                    if (!objCustomer.IsTransactionCompleted)
                    {
                        bookingRefNum = ConfigurationManager.AppSettings["OfferUniqueTransaction"] + Carwale.BL.PaymentGateway.PGCookie.PGTransId;

                        //send sms to customer
                        SendEmailSMSToDealerCustomer.BookingSMSToCustomer(objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName, bikeName, _objPQ.objDealer.Name, _objPQ.objDealer.MobileNo, address, bookingRefNum, insuranceAmount);
                        //send sms to dealer
                        SendEmailSMSToDealerCustomer.BookingSMSToDealer(objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName, bikeName, _objPQ.objDealer.Name, _objPQ.objDealer.MobileNo, _objPQ.objDealer.Address, bookingRefNum, BooingAmt, insuranceAmount);
                        
                        //send email to dealer

                        if (objCustomer.objColor != null)
                        {
                            bikeColor = objCustomer.objColor.ColorName;
                        }
                        string imgPath = string.Empty;
                        if (_objPQ.objQuotation != null && _objPQ.objQuotation.OriginalImagePath != null && _objPQ.objQuotation.HostUrl != null)
                        {
                            imgPath = Bikewale.Utility.Image.GetPathToShowImages(_objPQ.objQuotation.OriginalImagePath, _objPQ.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                        }
                        SendEmailSMSToDealerCustomer.BookingEmailToDealer(_objPQ.objDealer.EmailId, ConfigurationManager.AppSettings["OfferClaimAlertEmail"], objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.CustomerEmail, totalPrice, _objPQ.objBookingAmt.Amount, totalPrice - _objPQ.objBookingAmt.Amount, _objPQ.objQuotation.PriceList, bookingRefNum, bikeName, bikeColor, _objPQ.objDealer.Name, _objPQ.objOffers, imgPath
                            , "", insuranceAmount);
                        //send email to customer
                        SendEmailSMSToDealerCustomer.BookingEmailToCustomer(objCustomer.objCustomerBase.CustomerEmail, objCustomer.objCustomerBase.CustomerName
                            , _objPQ.objQuotation.PriceList, _objPQ.objOffers, bookingRefNum, totalPrice, _objPQ.objBookingAmt.Amount, MakeModel, VersionName, bikeColor, imgPath,
                            _objPQ.objDealer.Organization, address, _objPQ.objDealer.MobileNo, _objPQ.objDealer.EmailId, _objPQ.objDealer.WorkingTime, _objPQ.objDealer.objArea.Latitude, _objPQ.objDealer.objArea.Longitude);
                    }
                    else
                    {
                        Response.Redirect("/pricequote/bookingsummary.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            else
            {
                Response.Redirect("/pricequote/quotation.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 15 Dec 2014
        /// Summary : To get dealer price break up and other details
        /// Modified By : Lucky Rathore on 11 May 2016.
        /// Summary : Assing Value to VersionName.
        /// </summary>
        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + PriceQuoteCookie.VersionId + "&DealerId=" + PriceQuoteCookie.DealerId + "&CityId=" + PriceQuoteCookie.CityId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objPQ = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objPQ);
                }

                if (_objPQ != null)
                {
                    if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                    {
                        rptOffers.DataSource = _objPQ.objOffers;
                        rptOffers.DataBind();
                    }

                    if (_objPQ.objDealer != null)
                    {
                        contactNo = _objPQ.objDealer.PhoneNo + (!String.IsNullOrEmpty(_objPQ.objDealer.PhoneNo) && !String.IsNullOrEmpty(_objPQ.objDealer.MobileNo) ? ", " : "") + _objPQ.objDealer.MobileNo;
                        organization = _objPQ.objDealer.Organization;
                        lattitude = _objPQ.objDealer.objArea.Latitude;
                        longitude = _objPQ.objDealer.objArea.Longitude;
                        address = _objPQ.objDealer.Address + ", " + _objPQ.objDealer.objArea.AreaName + ", " + _objPQ.objDealer.objCity.CityName;

                        if (!String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(_objPQ.objDealer.objArea.PinCode))
                        {
                            address += ", " + _objPQ.objDealer.objArea.PinCode;
                        }

                        address += ", " + _objPQ.objDealer.objState.StateName;
                        WorkingTime = Convert.ToString(_objPQ.objDealer.WorkingTime);
                    }
                    if (_objPQ.objQuotation != null)
                    {
                        bikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;
                        MakeModel = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName;
                        VersionName = _objPQ.objQuotation.objVersion.VersionName;
                        bool isShowroomPriceAvail = false, isBasicAvail = false;
                        uint exShowroomCost = 0;
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

                            totalPrice += item.Price;
                        }
                        foreach (var price in _objPQ.objQuotation.PriceList)
                        {
                            Bikewale.common.DealerOfferHelper.HasFreeInsurance(_objPQ.objDealer.DealerId.ToString(), _objPQ.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                        }
                        if (insuranceAmount > 0)
                        {
                            IsInsuranceFree = true;
                        }

                        if (isBasicAvail && isShowroomPriceAvail)
                            totalPrice = totalPrice - exShowroomCost;

                        BooingAmt = _objPQ.objBookingAmt.Amount;
                    }
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

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteCookie.PQId));

                if (objCustomer == null)
                {
                    Response.Redirect("/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }
    }
}