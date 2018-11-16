using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.PriceQuote
{
    public partial class PaymentConfirmation : System.Web.UI.Page
    {
        protected Repeater rptOffers;

        protected PQ_DealerDetailEntity _objPQ;
        protected PQCustomerDetail objCustomer;
        protected double lattitude, longitude;
        protected UInt32 BooingAmt = 0;
        protected string contactNo = string.Empty, organization = string.Empty, address = string.Empty, bikeName = string.Empty, MakeModel = string.Empty, bookingRefNum = string.Empty;
        protected uint totalPrice = 0;
        protected UInt32 insuranceAmount = 0, dealerId = 0;
        protected bool IsInsuranceFree = false;
        protected IList<PQ_Price> discountsList { get; set; }
        protected Repeater rptDiscount;
        protected UInt32 totalDiscount = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string bikeColor = String.Empty;
            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                try
                {
                    GetDetailedQuote();
                    getCustomerDetails();
                    if (objCustomer.IsTransactionCompleted)
                    {
                        bookingRefNum = ConfigurationManager.AppSettings["OfferUniqueTransaction"] + Carwale.BL.PaymentGateway.PGCookie.PGTransId;
                        if (objCustomer.objColor != null)
                        {
                            bikeColor = objCustomer.objColor.ColorName;
                        }
                    }
                    else
                    {
                        Response.Redirect("/m/pricequote/bookingsummary.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, Request.ServerVariables["URL"]);

                }
            }
            else
            {
                Response.Redirect("/m/pricequote/dealer/?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = Convert.ToUInt32(PriceQuoteQueryString.CityId); ;
                    objParam.DealerId = Convert.ToUInt32(PriceQuoteQueryString.DealerId);
                    objParam.VersionId = Convert.ToUInt32(PriceQuoteQueryString.VersionId); ;
                    _objPQ = objDealer.GetDealerDetailsPQ(objParam);
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
                        dealerId = _objPQ.objDealer.DealerId;
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
                    }
                    if (_objPQ.objQuotation != null)
                    {
                        bikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;
                        MakeModel = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName;
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
                            Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), _objPQ.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                        }
                        if (insuranceAmount > 0)
                        {
                            IsInsuranceFree = true;
                        }
                        _objPQ.objQuotation.discountedPriceList = OfferHelper.ReturnDiscountPriceList(_objPQ.objOffers, _objPQ.objQuotation.PriceList);
                        if (_objPQ.objQuotation.discountedPriceList != null)
                        {
                            rptDiscount.DataSource = _objPQ.objQuotation.discountedPriceList;
                            rptDiscount.DataBind();
                            totalDiscount = TotalDiscountedPrice();
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
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
            finally
            {
                if (!_isContentFound)
                {
                    UrlRewrite.Return404();

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

                objCustomer = objDealer.GetCustomerDetailsByLeadId(Convert.ToUInt32(PriceQuoteQueryString.LeadId));

                if (objCustomer == null)
                {
                    Response.Redirect("/m/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// created By : Sangram Nandkhile 8 Oct 2015
        /// Modified By : Sushil Kumar on 9th Oct 2015
        /// Function used to Push Booking Request in AutoBiz
        /// </summary>
        private void PushBikeBookingSuccess()
        {
            try
            {
                BookingRequest request = new BookingRequest();
                request.BookingDate = DateTime.Now;
                request.BranchId = _objPQ.objDealer.DealerId;
                request.InquiryId = Convert.ToUInt32(objCustomer.AbInquiryId);
                request.PaymentAmount = BooingAmt;
                request.Price = totalPrice;

                string _apiUrl = "/webapi/booking/";
                uint bookingId = default(uint);

                using (Bikewale.Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    bookingId = objClient.PostSync<BookingRequest, uint>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, request);
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
        }

        /// <summary>
        /// Creted By : Lucky Rathore
        /// Created on : 08 January 2016
        /// </summary>
        /// <returns>Total dicount on specific Version.</returns>
        protected UInt32 TotalDiscountedPrice()
        {
            UInt32 totalPrice = 0;
            foreach (var priceListObj in _objPQ.objQuotation.discountedPriceList)
            {
                totalPrice += priceListObj.Price;
            }
            return totalPrice;
        }
    }
}