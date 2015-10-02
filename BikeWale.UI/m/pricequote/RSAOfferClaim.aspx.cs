﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.BAL.BikeData;
using Bikewale.Controls;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Microsoft.Practices.Unity;

namespace Bikewale.Mobile.BikeBooking
{
    public class RSAOfferClaim : System.Web.UI.Page
    {
        protected TextBox txtBookingNum, txtName, txtMobile, txtEmail, txtVehicle, txtAddress, txtPincode, txtdealerName, txtDealerAddress, txtComments;
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
            RSAMessage.Visible = false;
            BindMakes();
        }

        /// <summary>
        /// Modified by : Ashish G. Kamble on 28 May 2015
        /// Modified : Added helmet offer id to claim the helmet offer also.
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

                objOffer = new RSAOfferClaimEntity()
                {
                    BookingNum = txtBookingNum.Text,
                    CustomerName = txtName.Text,
                    CustomerEmail = txtEmail.Text,
                    CustomerMobile = txtMobile.Text,
                    CustomerAddress = txtAddress.Text,
                    CustomerPincode = txtPincode.Text,
                    DeliveryDate = calDeliveryDate.Value,
                    DealerAddress = txtDealerAddress.Text,
                    DealerName = txtdealerName.Text,
                    BikeRegistrationNo = txtVehicle.Text,
                    Comments = txtComments.Text,
                    VersionId = Convert.ToUInt32(hdnVersion.Value),
                    HelmetId = Convert.ToUInt16(hdnSelHelmet.Value)
                };

                objDealer.SaveRSAOfferClaim(objOffer, hdnBikeName.Value);
                isOfferClaimed = true;
                RSAMessage.Visible = true;
                RSAMessage.InnerText = "Thank you for submitting your bike purchase information in order to claim the offers. Please give us 30 days to ship the gifts to your given address after verifying your purchase with the dealership. You can also write to us at contact@bikewale.com in case of any concerns.";
            }
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
    }   // class
}   // namespace