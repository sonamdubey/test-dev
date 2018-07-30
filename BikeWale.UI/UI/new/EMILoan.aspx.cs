using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data;

namespace Bikewale.New
{
	public class EMILoan : System.Web.UI.Page
	{
        protected DropDownList ddlState, ddlCity;
        protected HtmlInputText txtMobile, txtEmail, txtName;
        protected HiddenField hdnCityId;

        protected string modelId = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                try
                {
                    BindState();
                }
                catch (Exception err)
                {
                    ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                    
                }
            }
            modelId = Request.QueryString["modelid"];
		}

        /// <summary>
        /// Created By : Sadhana on 17 Oct 2014
        /// Summary : To bind state
        /// </summary>
        protected void BindState()
        {
            DataTable dt = null;
            try
            {
                StateCity objState = new StateCity();

                dt = objState.GetStates();

                ddlState.DataSource = dt;
                ddlState.DataValueField = "Value";
                ddlState.DataTextField = "Text";
                ddlState.DataBind();

                ddlState.Items.Insert(0,new ListItem("--Select State--","0"));
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "EMILoan.BindState");
                
                Trace.Warn("err makes : ", err.Message);
            }
        }   //End of BindState
	}   // End of class
}   // End of namespace