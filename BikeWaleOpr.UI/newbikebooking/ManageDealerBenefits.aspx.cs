using BikewaleOpr.common;
using BikewaleOpr.DAL;
using BikewaleOpr.Entities;
using BikewaleOpr.Interfaces;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace BikewaleOpr.NewBikeBooking
{
    /// <summary>
    /// Created by : Sangram
    /// Created on : 10-March-2016
    /// Summary    :  Web page to allow dealers to add Benefits/ USP
    /// </summary>
    public class ManageDealerBenefits : System.Web.UI.Page
    {
        #region global variables

        protected string _dealerId = string.Empty, _cityId = string.Empty, _currentUserID = string.Empty;
        ManageDealerBenefit manageDealer;
        protected Button btnAdd, btnReset, btnEditSubmit;
        protected Repeater rptBenefits;
        protected DropDownList ddlBenefitCat, ddlEditBenefit;
        protected TextBox benefitText, txtEditBenefit;
        protected HiddenField hdnBenefitId;
        protected Label greenMessage;

        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            btnAdd.Click += new EventHandler(SaveOffers);
            btnReset.Click += new EventHandler(ResetForm);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            manageDealer = new ManageDealerBenefit();
            _currentUserID = CurrentUser.Id;
            _dealerId = Request.QueryString["dealerId"];
            _cityId = Request.QueryString["cityId"];
            // First page load
            if (!IsPostBack)
            {
                BindData();
            }
            GetDealerBenefits();
        }

        private void SaveOffers(object sender, EventArgs e)
        {
            SaveDealerBenefit();
        }

        private void ResetForm(object sender, EventArgs e)
        {
            greenMessage.Text = string.Empty;
            ddlBenefitCat.SelectedIndex = 0;
            benefitText.Text = string.Empty;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Sangram Nandkhile
        /// Created on : 10-March-2016
        /// Summary    : Add new dealer benefit
        /// </summary>
        private void SaveDealerBenefit()
        {
            try
            {
                bool status = false;

                if (Convert.ToUInt32(_dealerId) > 0 && Convert.ToUInt32(_cityId) > 0 && Convert.ToUInt32(ddlBenefitCat.SelectedValue) > 0
                    && Convert.ToUInt32(CurrentUser.Id) > 0 && Convert.ToUInt32(hdnBenefitId.Value) > 0 && !String.IsNullOrEmpty(HttpUtility.UrlEncode(benefitText.Text)))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objCity = container.Resolve<DealersRepository>();
                        status = objCity.SaveDealerBenefit(Convert.ToUInt32(_dealerId), Convert.ToUInt32(_cityId),
                                                            Convert.ToUInt32(ddlBenefitCat.SelectedValue), Convert.ToString(benefitText),
                                                            Convert.ToUInt32(CurrentUser.Id), Convert.ToUInt32(hdnBenefitId.Value));
                    }

                }

                GetDealerBenefits();
                greenMessage.Text = "Benefit / USP has been added !";
                benefitText.Text = string.Empty;
                ddlBenefitCat.SelectedIndex = 0;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by : Sangram
        /// Created on : 10-March-2016
        /// Summary    : Get all dealer Benefits
        /// </summary>
        private async void GetDealerBenefits()
        {
            IEnumerable<DealerBenefitEntity> objOfferList = null;
            try
            {
                if (Convert.ToUInt32(_dealerId) > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealers, DealersRepository>();
                        IDealers objDealer = container.Resolve<DealersRepository>();
                        objOfferList = objDealer.GetDealerBenefits(Convert.ToUInt32(_dealerId));
                    }
                }

                if (objOfferList != null && objOfferList.Count() > 0)
                {
                    rptBenefits.DataSource = objOfferList;
                    rptBenefits.DataBind();
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
        /// Created by : Sangram
        /// Created on : 10-March-2016
        /// Summary    : Bind all the Dropdowns and Repeaters
        /// </summary>
        private void BindData()
        {
            // Bind Benefits for current dealer
            GetDealerBenefits();
            // Bind Benefits categories
            DataTable benefitCategories = manageDealer.GetDealerCategories(_dealerId);
            if (benefitCategories != null)
            {
                ddlBenefitCat.DataSource = benefitCategories;
                ddlBenefitCat.DataTextField = "Name";
                ddlBenefitCat.DataValueField = "Id";
                ddlBenefitCat.DataBind();
                ddlEditBenefit.DataSource = benefitCategories;
                ddlEditBenefit.DataTextField = "Name";
                ddlEditBenefit.DataValueField = "Id";
                ddlEditBenefit.DataBind();
            }
        }

        #endregion
    }
}