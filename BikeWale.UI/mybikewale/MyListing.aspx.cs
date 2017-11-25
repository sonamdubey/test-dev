using Bikewale.Common;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Bikewale.MyBikeWale
{
    /// <summary>
    ///     Created By : Ashish G. Kamble
    ///     Class to show the customer the bike listed by him and options to edit his bike
    /// </summary>
    public class MyListing : Page
    {
        protected IEnumerable<CustomerListingDetails> listingDetailsList;
        protected HtmlGenericControl div_SellYourBike, div_FakeCustomer;
        protected string customerId = string.Empty, inquiryId = string.Empty;
        protected bool isFake = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            div_FakeCustomer.Visible = false;
            // check for login.
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx?returnUrl=/mybikewale/mylisting.aspx?bike=" + Request.QueryString["bike"]);
            }

            customerId = CurrentUser.Id;

            RegisterCustomer objCust = new RegisterCustomer();
            isFake = objCust.IsFakeCustomer(Convert.ToInt32(customerId));

            if (isFake)
                div_FakeCustomer.Visible = true;


            GetListings();
        }   // End of page_load

        /// <summary>
        /// Created by Sajal Gupta on 25-11
        /// Desc : Method to get listing details
        /// </summary>
        protected void GetListings()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeSellerRepository, UsedBikeSellerRepository>();

                    var objSellerRepo = container.Resolve<IUsedBikeSellerRepository>();

                    listingDetailsList = objSellerRepo.GetCustomerListingDetails(Convert.ToUInt32(customerId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MyListing.GetListings()");
                
            }
        }   // End of GetListings method

        protected string GetImagePath(string imgName, string directoryPath, string hostUrl)
        {
            return MakeModelVersion.GetModelImage(hostUrl, directoryPath + imgName);
            //return Bikewale.Common.ImagingFunctions.GetPathToShowImages(directoryPath, hostUrl) + imgName;
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 23-02-2017
        /// Description : Setting approval pending status for status id = 5.
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="isApproved"></param>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        protected string GetStatus(int statusId, bool isApproved, int inquiryId)
        {
            string retVal = string.Empty;
            if (statusId == (int)SellAdStatus.Approved && isApproved)
            {
                retVal = "[ Approved ]";
            }
            else if (statusId == (int)SellAdStatus.MobileUnverified)
            {
                retVal = "<a target=_blank style=color:#f00; class=link-decoration href=/used/sell/default.aspx?id=" + inquiryId + " >[ Get Verified ]</a>";
            }
            else if (statusId == (int)SellAdStatus.MobileVerified || (statusId == (int)SellAdStatus.Approved && !isApproved))
            {
                retVal = "[ Approval pending ]";
            }
            else if (statusId == (int)SellAdStatus.Fake && !isApproved)
            {
                retVal = "[ Fake ]";
            }
            return retVal;
        }

    }   // End of class
}   // End of namespace

