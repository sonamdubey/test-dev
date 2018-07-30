using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MySql.CoreDAL;


namespace Bikewale.MyBikeWale
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 9/9/2012
    ///     Class to show intersted buyers list for the current inquiry
    /// </summary>
    public class BuyerDetails : System.Web.UI.Page
    {
        protected Repeater rptBuyersList;


        protected string inquiryId = string.Empty, _bikeName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx?returnurl=/mybikewale/mylisting.aspx");    
            }

            if(!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                inquiryId = Request.QueryString["id"];
            }

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(inquiryId))
                {
                    FillBuyersList();
                }                
            }
        }   // End of page load

        protected void FillBuyersList()
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getclassifiedindividualbuyerdetails";  
                    //cmd.Parameters.Add("@inquiryid", SqlDbType.BigInt).Value = inquiryId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, inquiryId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            rptBuyersList.DataSource = ds.Tables[0];
                            rptBuyersList.DataBind();
                        } 
                    }
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("FillBuyersList sql ex : ", ex.Message);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                Trace.Warn("FillBuyersList ex : ", ex.Message);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        } // End of FillBuyersList method

        protected string GetBikeDetails(string bikeName)
        {
            if (string.IsNullOrEmpty(_bikeName))
            {
                _bikeName = bikeName;
                Trace.Warn(_bikeName);    
            }

            return "";
        }
    }   // End of class
}   // End of namespace