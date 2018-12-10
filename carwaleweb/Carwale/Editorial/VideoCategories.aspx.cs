using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.UI.Editorial;
using System.Collections.Specialized;
using Carwale.Notifications;



namespace Carwale.UI.Editorial
{
    public class VideoCategories : Page
    {

        protected VideosPager rpgVideos;
        protected Repeater rptVideos;
        protected HtmlGenericControl emptyVid;

        protected override void OnInit(EventArgs e)
        {
            rptVideos = (Repeater)rpgVideos.FindControl("rptVideos");
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
                try
                {
                    string query_string = Request.ServerVariables["QUERY_STRING"];
                    NameValueCollection qsCollection = Request.QueryString;

                    if (!String.IsNullOrEmpty(qsCollection.Get("catId")))
                    {
                        string makeId = qsCollection.Get("mId");
                        string catId = qsCollection.Get("catId");
                        string modelId = qsCollection.Get("moId");

                        rpgVideos.BaseUrl = "/videos/videocategories.aspx?" + query_string;
                        rpgVideos.CatId = catId;
                        rpgVideos.MakeId = makeId;
                        rpgVideos.ModelId = modelId;

                        string pageNumber = qsCollection.Get("pn");
                        if (!String.IsNullOrEmpty(pageNumber) && CommonOpn.IsNumeric(pageNumber))
                            rpgVideos.CurrentPageIndex = int.Parse(pageNumber);
                        else
                            rpgVideos.CurrentPageIndex = 1;


                        rpgVideos.InitializeGrid();//initialize the grid, and this will also bind the repeater

                        if (rpgVideos.RecordCount == 0)
                        {
                            rpgVideos.Visible = false;
                            emptyVid.Visible = true;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
        }

        protected string FindSubString(string str, string strFrom, string strTo)
        {
            int pFrom = str.IndexOf(strFrom) + strFrom.Length; ;
            int pTo = str.IndexOf(strTo);
            return str.Substring(pFrom, pTo - pFrom);
        }

        protected string FormatSubCat(string subCat)
        {
            return subCat.Trim().ToLower().Replace(" ", "-");
        }

    } //class
}//namespace