using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Ajax;

namespace BikeWaleOpr.EditCms
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 9th Dec 2013
    /// summary : To manage update image tagging.
    /// </summary>
    public class ManageImageTagging : Page
    {
        // Variables and Controls
        protected string ImageId = string.Empty;
        protected CheckBox chkMainImage;
        protected TextBox txtCaption;
        protected DropDownList ddlMakes, ddlModels;
        protected Button btnUpdateTagging;
        protected HiddenField hdnModels;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdateTagging.Click += new EventHandler(UpdateImageTagging);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageId = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                ViewState["PreviousPage"] = Request.UrlReferrer;
                try
                {                    
                    LoadImageDetails();
                }
                catch (Exception err)
                {
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
        }

        #region GetMakes
        /// <summary>
        /// Created By : Sadhana Upadhyay on 4th Dec 2013
        /// summary : Gets the list of makes that we need to tag the images with
        /// </summary>
        private void GetMakes()
        {
            DataTable dt;
            MakeModelVersion mmv = new MakeModelVersion();
            dt = mmv.GetMakes("All");

            ddlMakes.DataSource = dt;
            ddlMakes.DataValueField = "value";
            ddlMakes.DataTextField = "text";
            ddlMakes.DataBind();

            ddlMakes.Items.Insert(0, new ListItem("--Select Make--", "0"));
        }   // End of LoadImageDetails
        #endregion


        #region LoadImageDetails
        /// <summary>
        /// Created By : Sadhana Upadhyay on 9th Dec 2013
        /// Summary : To retrieve value of caption, Make, Model, IsMainImage 
        /// </summary>
        private void LoadImageDetails()
        {
            throw new Exception("Method not used/commented");

            //string makeId = string.Empty, modelId = string.Empty;

            //SqlCommand cmd;
            //Database db = new Database();
            //SqlDataReader dr = null;

            //try
            //{
            //    using (SqlConnection con = new SqlConnection(db.GetConString()))
            //    {
            //        cmd = new SqlCommand();
            //        cmd.CommandText = "Con_EditCms_GetImageDetails";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Connection = con;

            //        cmd.Parameters.Add("@ImageId", SqlDbType.BigInt).Value = ImageId;

            //        cmd.Parameters.Add("@Caption", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
            //        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        cmd.Parameters.Add("@IsMainImage", SqlDbType.Bit).Direction = ParameterDirection.Output;

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    txtCaption.Text = cmd.Parameters["@Caption"].Value.ToString();

            //    GetMakes();
            //    makeId = cmd.Parameters["@MakeId"].Value.ToString();
            //    ddlMakes.SelectedValue = makeId;

            //    GetModels(makeId);
            //    ddlModels.SelectedValue = cmd.Parameters["@ModelId"].Value.ToString();
                
            //    chkMainImage.Checked = Convert.ToBoolean(cmd.Parameters["@IsMainImage"].Value);
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
        }   // End of LoadImageDetails
        #endregion

        #region GetModels
        /// <summary>
        /// Created By : Sadhana Upadhyay on 4th Dec
        /// Summary : Get the models with respect to a certain make and select the desired model from that list.
        /// </summary>
        /// <param name="makeId">Make Id for which the models need to be fetched</param>
        /// <param name="modelId">Model Id which needs to be selected from the list of models retrieved</param>
        private void GetModels(string makeId)
        {
            DataTable dt;
            MakeModelVersion mmv = new MakeModelVersion();
            dt = mmv.GetModels(makeId, "All");

            ddlModels.DataSource = dt;
            ddlModels.DataValueField = "value";
            ddlModels.DataTextField = "text";
            ddlModels.DataBind();

            ddlModels.Items.Insert(0, new ListItem("--Select Model--", "0"));
        }   // End of GetModels
        #endregion
        
        /// <summary>
        /// Created By : Sadhana Upadhyay on 9th Dec 2013
        /// Summary : To update Caption, Make, Model, IsMainImage 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateImageTagging(object sender, EventArgs e)
        {
            throw new Exception("Method not used/commented");
            //SqlCommand cmd = null;
            //Database db = null;

            //try
            //{
            //    db = new Database();
            //    cmd = new SqlCommand();
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "Con_EditCms_UpdateImageTagging";
                
            //    cmd.Parameters.Add("@ImageId", SqlDbType.BigInt).Value = ImageId;
            //    cmd.Parameters.Add("@Caption", SqlDbType.VarChar, 250).Value = txtCaption.Text.Trim();
            //    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = ddlMakes.Text;
            //    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = hdnModels.Value;                
            //    cmd.Parameters.Add("@IsMainImage", SqlDbType.Bit).Value = chkMainImage.Checked;

            //    db.UpdateQry(cmd);
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (ViewState["PreviousPage"] != null)
            //    {
            //        Response.Redirect(ViewState["PreviousPage"].ToString());
            //    }
            //}
        }   // End of UpdateImageTagging
    }
}