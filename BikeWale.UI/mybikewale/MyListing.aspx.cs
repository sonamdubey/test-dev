using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;

namespace Bikewale.MyBikeWale
{
    /// <summary>
    ///     Created By : Ashish G. Kamble
    ///     Class to show the customer the bike listed by him and options to edit his bike
    /// </summary>
    public class MyListing : Page
    {
        protected DataList rptListings;
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
            
            if (!IsPostBack)
            {
                GetListings();
    
            }
        }   // End of page_load

        protected void GetListings()
        {
            Database db = null;
            DataSet ds = null;

            try
            {
                db = new Database();

                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetClassifiedIndividualListings_SP";

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;

                    ds = db.SelectAdaptQry(cmd);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rptListings.DataSource = ds.Tables[0];
                        rptListings.DataBind();

                        div_SellYourBike.Visible = false;
                    }                    
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("mylistings sql ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("mylistings ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of GetListings method

        protected string GetImagePath(string imgName, string directoryPath, string hostUrl)
        {
            return MakeModelVersion.GetModelImage(hostUrl, directoryPath + imgName);
            //return Bikewale.Common.ImagingFunctions.GetPathToShowImages(directoryPath, hostUrl) + imgName;
        }

        protected string GetStatus(string statusId,bool isApproved,string inquiryId)
        {
            string retVal = string.Empty;
            if (statusId == "1" && isApproved)
            {
                retVal = "[ Approved ]";
            }
            else if (statusId == "4")
            {
                retVal = "<a target=_blank style=color:#f00; class=link-decoration href=/used/sell/default.aspx?id=" + inquiryId + " >[ Get Verified ]</a>";
            }
            else if (statusId == "1" && !isApproved)
            {
                retVal = "[ Approval pending ]";
            }
            else if (statusId == "2" && !isApproved)
            {
                retVal = "[ Fake ]";
            }
            return retVal;
        }
    
    }   // End of class
}   // End of namespace