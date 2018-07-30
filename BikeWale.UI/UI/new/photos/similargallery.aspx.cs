using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Controls;
using Bikewale.Common;

namespace Bikewale.New.PhotoGallery
{
    public class SimilarGallery : System.Web.UI.Page
    {
        protected RepeaterPagerPhotoGallery rpgListings;
        protected Repeater rptListings;
        protected HtmlGenericControl smAlertMsg; // In the case that there are no images for similar Bikes to the model, the message to be displayed.

        // html controls
        //protected HtmlGenericControl res_msg;

        // protected variables
        protected string PageNumber = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            rptListings = (Repeater)rpgListings.FindControl("rptListings");
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                smAlertMsg.Visible = false;
                string query_string = Request.ServerVariables["QUERY_STRING"];
                NameValueCollection qsCollection = Request.QueryString;
                if (!String.IsNullOrEmpty(qsCollection.Get("moId")))
                {
                    string modelId = qsCollection.Get("moId");
                    Trace.Warn(modelId);

                    if (CommonOpn.CheckId(modelId))
                    {
                        string modelIds = GetSimilarBikes(modelId);
                        Trace.Warn("modelIds: " + modelIds);
                        SqlCommand sqlCmdParams = new SqlCommand();
                        sqlCmdParams.Parameters.Add("@ModelIds", SqlDbType.VarChar, -1).Value = modelIds;

                        rpgListings.BaseUrl = "/research/photos/similargallery.aspx?" + query_string;
                        rpgListings.SelectClause = GetSelectClause();
                        rpgListings.FromClause = GetFromClause();
                        rpgListings.WhereClause = GetWhereClause();
                        rpgListings.OrderByClause = GetOrderByClause();
                        rpgListings.RecordCountQuery = GetRecordCountQry();
                        rpgListings.CmdParamQry = sqlCmdParams;
                        rpgListings.CmdParamCountQry = sqlCmdParams.Clone();                       

                        string pageNumber = qsCollection.Get("pn");
                        if (!String.IsNullOrEmpty(pageNumber) && CommonOpn.IsNumeric(pageNumber))
                            rpgListings.CurrentPageIndex = int.Parse(pageNumber);
                        else
                            rpgListings.CurrentPageIndex = 1;
                        
                        rpgListings.InitializeGrid();//initialize the grid, and this will also bind the repeater

                        if (rpgListings.RecordCount == 0)
                        {
                            rpgListings.Visible = false;
                            smAlertMsg.Visible = true;
                        }
                    }
                }
            }
        }

        private string GetSimilarBikes(string modelId)
        {
            throw new Exception("Method not used/commented");

            //string retVal = string.Empty;
            //string sql = " SELECT SCM.SimilarModels FROM SimilarBikeModels SCM With(NoLock) WHERE SCM.ModelId = @ModelId";

            //Trace.Warn(sql);

            //SqlCommand cmd = new SqlCommand(sql);
            //cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;

            //SqlDataReader dr = null;
            //Database db = new Database();

            //try
            //{
            //    dr = db.SelectQry(cmd);
            //    while (dr.Read())
            //        retVal = dr["SimilarModels"].ToString();
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
            //    //Dispose of all the objects and Close connection objects.
            //    if (dr != null)
            //    {
            //        dr.Close();
            //        dr.Dispose();
            //    }                
            //    db.CloseConnection();
            //}

            //return retVal;
        }

        private string GetSelectClause()
        {
            //return " CB.Id, Case CB.CategoryId When 8 Then ('Road Test: ' + CMa.Name + ' ' + CMo.Name) When 1 Then CB.Title When 3 Then CB.Title Else CB.Title End As ArticleTitle, ('http://' + CB.HostURL + '/ec/' + CONVERT(VarChar, CB.Id) + '/img/m/' + CONVERT(VarChar, CB.Id) + '_m.jpg') As MainImage, CMa.Name as MakeName,CMa.MaskingName AS MakeMaskingName, CMo.Name As ModelName,CMo.MaskingName AS ModelMaskingName,  CB.EntryDate ";
            return " CB.Id, Case CB.CategoryId When 8 Then ('Road Test: ' + CMa.Name + ' ' + CMo.Name) When 1 Then CB.Title When 3 Then CB.Title Else CB.Title End As ArticleTitle, ('http://' + CB.HostURL + '/800x600/' + CONVERT(VarChar, CB.Id) + '/img/m/' + CONVERT(VarChar, CB.Id) + '.jpg') As MainImage, CMa.Name as MakeName,CMa.MaskingName AS MakeMaskingName, CMo.Name As ModelName,CMo.MaskingName AS ModelMaskingName,  CB.EntryDate ";
        }

        private string GetFromClause()
        {
            return " Con_EditCms_Basic CB With(NoLock)"
                + " Inner Join Con_EditCms_Bikes CEC With(NoLock) on CEC.BasicId = CB.Id"
                + " Inner Join Con_EditCms_Images CEI With(NoLock) On CEI.BasicId = CB.Id"
                + " Inner Join [dbo].[fnSplitCSV](@ModelIds) as f on  CEC.ModelId =f.ListMember"
                + " Inner Join BikeModels as CMo With(NoLock) on CMo.ID= CEC.ModelId"
                + " Inner Join BikeMakes as CMa With(NoLock) on CMa.ID= CEC.MakeId";
        }

        private string GetWhereClause()
        {
            string whereClause = " CEI.IsActive = 1 And CEC.IsActive = 1 And CB.IsPublished=1 And CMa.IsDeleted = 0 And CMo.IsDeleted = 0 And CB.CategoryId Not In (1,2) " // Exclude CategoryId = 1 & 2 (News & Comparison Tests) for Similar Bikes
                            + " Group By CB.Id, CB.Title, CB.HostURL, CMa.Name, CMo.Name, CB.EntryDate, CB.CategoryId , CMa.MaskingName,CMo.MaskingName";
            return whereClause;
        }

        private string GetOrderByClause()
        {
            string retVal = string.Empty;

            retVal = " EntryDate DESC ";

            return retVal;
        }

        private string GetRecordCountQry()
        {
            return " Select Count(Id) From (Select " + GetSelectClause() + "From " + GetFromClause() + " Where " + GetWhereClause() + " )tbl ";
        }        
    } // class
} // namespace