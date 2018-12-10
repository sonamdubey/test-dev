using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using MobileWeb.Controls;

namespace MobileWeb.Forums
{
    public class ViewThreads : Page
    {
        protected string forumId, pageNo="1", threadCount = "0", subCatName, subCatDesc,url="";
        protected int pageSize = 10, totalPages = 0;
        protected PageThreads ucPageThreads;
        protected Repeater rptStickyThreads;
        protected string pageUrl = "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["forum"] != null && Request.QueryString["forum"] != "")
                {
                    forumId = Request.QueryString["forum"];
                    Trace.Warn("forum " +  Request.QueryString["forum"]);
                    //verify the id as passed in the url
                    if (CommonOpn.CheckId(forumId) == false)
                    {
                        //redirect to the default page
                        Response.Redirect("~/m/forums/");
                        return;
                    }
                }
                else
                {
                    //redirect to the default page
                    Response.Redirect("~/m/forums/");
                    return;
                }

                if (Request.QueryString["pn"] != null && Request.QueryString["pn"] != "" && CommonOpn.CheckId(Request.QueryString["pn"].ToString()) == true)
                {
                    Trace.Warn(Request.QueryString["pn"].ToString());
                    pageNo = Request.QueryString["pn"].ToString();
                }
                GetSubCategoryDetails();
                GetThreadCount();
                BindFirstPageThreads();
                BindStickyThreads();

                pageUrl = url;
            }
        }

        private void BindStickyThreads()
        {
            Forum obj = new Forum();
            obj.GetRepeater = true;
            obj.Rpt = rptStickyThreads;
            obj.GetStickyThreads(forumId);
        }

        private void GetSubCategoryDetails()
        {
            IDataReader dr = null;
            Forum obj = new Forum();
            try
            {
                obj.GetReader = true;
                obj.GetSubCategoryDetails(forumId);
                dr = obj.drReader;
                if (dr.Read())
                {
                    subCatName = dr["Name"].ToString();
                    subCatDesc = dr["Description"].ToString();
                    url = dr["Url"].ToString();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                obj.drReader.Close();
            }
        }

        private void BindFirstPageThreads()
        {
            ucPageThreads.ForumSubCategoryId = Convert.ToInt32(forumId);
            ucPageThreads.PageSize = 10;
            ucPageThreads.PageNo = Convert.ToInt32(pageNo);
            ucPageThreads.BindPage();
        }

        private void GetThreadCount()
        {
            IDataReader dr = null;
            Forum obj = new Forum();
            try
            {
                obj.GetReader = true;
                obj.GetSubCategoryThreadCount(forumId);
                dr = obj.drReader;
                if (dr.Read())
                {
                    int totalThreads = 0;
                    totalThreads = Convert.ToInt32(dr[0].ToString());
                    totalPages = totalThreads / pageSize;
                    if (totalThreads % pageSize != 0)
                        totalPages++;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                obj.drReader.Close();
            }
        }
    }
}