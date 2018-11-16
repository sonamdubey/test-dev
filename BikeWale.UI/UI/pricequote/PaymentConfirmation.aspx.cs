using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.PriceQuote
{
    public class PaymentConfirmation : System.Web.UI.Page
    {
        protected Repeater rptOffers, rptDiscount;

        protected PQ_DealerDetailEntity _objPQ = null;
        protected PQCustomerDetail objCustomer = null;
        protected double lattitude, longitude;
        protected uint totalPrice = 0;
        protected UInt32 BooingAmt = 0;
        protected string contactNo = string.Empty, organization = string.Empty, address = string.Empty, bikeName = string.Empty, MakeModel = string.Empty, bookingRefNum = string.Empty, WorkingTime = string.Empty;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;
        protected UInt32 totalDiscount = 0;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection();
            dd.DetectDevice();
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
                        Response.Redirect("/pricequote/bookingsummary_new.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
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
                Response.Redirect("/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 15 Dec 2014
        /// Summary : To get dealer price break up and other details
        /// </summary>
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
                    objParam.CityId = Convert.ToUInt32(PriceQuoteQueryString.CityId);
                    objParam.DealerId = Convert.ToUInt32(PriceQuoteQueryString.DealerId);
                    objParam.VersionId = Convert.ToUInt32((PriceQuoteQueryString.VersionId));
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
                    Response.Redirect("/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
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