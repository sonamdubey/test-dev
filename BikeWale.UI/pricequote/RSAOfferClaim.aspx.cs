using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Controls;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Common;

namespace Bikewale.PriceQuote
{
    public class RSAOfferClaim : System.Web.UI.Page
    {
        protected TextBox txtBookingNum, txtName, txtMobile, txtEmail, txtVehicle, txtAddress, txtPincode, txtdealerName, txtDealerAddress, txtComments, txtPreferredDate;
        protected HtmlInputHidden hdnVersion, hdnBikeName;
        protected HtmlSelect ddlMake;
        protected Button btnSubmit;
        protected DateControl calDeliveryDate;
        protected HtmlGenericControl RSAMessage;
        protected HiddenField hdnSelHelmet;
        public bool isOfferClaimed = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSubmit.Click += new EventHandler(SaveRSAPriceClaim);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Pivotal# 99282346
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            #endregion
            RSAMessage.Visible = false;
            BindMakes();
        }

        /// <summary>
        /// Modified by : Ashish G. Kamble on 28 May 2015
        /// Modified : Added helmet offer id to claim the helmet offer also.
        /// Modified By  : Sushil Kumar on 30th Nov 2015
        /// Modified : Remove value for helmet as helmet offer section hidden from UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveRSAPriceClaim(object sender, EventArgs e)
        {
            RSAOfferClaimEntity objOffer = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                string date = txtPreferredDate.Text;
                objOffer = new RSAOfferClaimEntity()
                {   
                    BookingNum = txtBookingNum.Text,
                    CustomerName = txtName.Text,
                    CustomerEmail = txtEmail.Text,
                    CustomerMobile = txtMobile.Text,
                    CustomerAddress = txtAddress.Text,
                    CustomerPincode = txtPincode.Text,
                    //DeliveryDate = calDeliveryDate.Value,
                    DeliveryDate = Convert.ToDateTime(date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat),
                    DealerAddress = txtDealerAddress.Text,
                    DealerName = txtdealerName.Text,
                    BikeRegistrationNo = txtVehicle.Text,
                    Comments = txtComments.Text,
                    VersionId = Convert.ToUInt32(hdnVersion.Value),
                    //HelmetId = Convert.ToUInt16(hdnSelHelmet.Value)
                };
                objDealer.SaveRSAOfferClaim(objOffer, hdnBikeName.Value);
                isOfferClaimed = true;
                RSAMessage.Visible = true;
                RSAMessage.InnerText = "Thank you for submitting your bike purchase information in order to claim the offers. Please give us 30 days to ship the gifts to your given address after verifying your purchase with the dealership. You can also write to us at contact@bikewale.com in case of any concerns.";
            }
            Bikewale.Notifications.SMSTypes smsTypes = new Bikewale.Notifications.SMSTypes();
            smsTypes.ClaimedOfferSMSToCustomer(txtMobile.Text, HttpContext.Current.Request.ServerVariables["URL"].ToString()); 
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Dec 2014
        /// Summary  : To bind dropdown with bike make
        /// </summary>
        private void BindMakes()
        {
            List<BikeMakeEntityBase> objMakes = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>();
                IBikeMakes<BikeMakeEntity, int> objMake = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                objMakes = objMake.GetMakesByType(EnumBikeType.New);

                if (objMakes != null && objMakes.Count > 0)
                {
                    ddlMake.DataSource = objMakes;
                    ddlMake.DataTextField = "MakeName";
                    ddlMake.DataValueField = "MakeId";
                    ddlMake.DataBind();
                }

                ddlMake.Items.Insert(0, new ListItem("--Select Make--", "0"));

            }
        }
    }   //End of class
}   //End of namespace