using BikewaleOpr.common;
using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikewaleOpr.newbikebooking
{
    public class ManageDealerBenefits : System.Web.UI.Page
    {
        private string _dealerId = string.Empty, _cityId = string.Empty;
        ManageDealerBenefit manageDealer;
        protected Repeater rptBenefits;
        protected DropDownList ddlBenefitCat;
        
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            //btnAdd.Click += new EventHandler(SaveOffers);
            //btnCopyOffers.Click += new EventHandler(btnCopyOffers_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            manageDealer = new ManageDealerBenefit();
            _dealerId = Request.QueryString["dealerId"];
            _cityId = Request.QueryString["cityId"];
            // First page load
            if(!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            List<DealerBenefitEntity> benefitList = manageDealer.GetDealerBenefits(_dealerId);
            if (benefitList != null && benefitList.Count > 0)
            {
                rptBenefits.DataSource = benefitList;
                rptBenefits.DataBind();
            }


            Dictionary<int, string> benefitCategories = manageDealer.GetDealerCategories(_dealerId);

            if (benefitCategories != null)
            {
                ddlBenefitCat.DataSource = benefitCategories;
                ddlBenefitCat.DataTextField = "Value";
                ddlBenefitCat.DataValueField = "Key";
                ddlBenefitCat.DataBind();
            }
        }
    }
}