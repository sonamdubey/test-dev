using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Memcache;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Bikewale.Interfaces.Pager;
using Bikewale.Entities.CMS.Articles;


namespace Bikewale.News
{
    /// <summary>
    /// Created By : Ashwini Todkar on 3 Oct 2014
    /// </summary>
    public class news : System.Web.UI.Page
    {
        private string _basicId = string.Empty;
        bool _isContentFount = true;
        protected ArticleDetails objArticle = null;

        protected string articleUrl = string.Empty, articleTitle = string.Empty, HostUrl = string.Empty, basicId = string.Empty, smallPicUrl = string.Empty, authorName = string.Empty, nextPageArticle = string.Empty, prevPageArticle = string.Empty, originalImgUrl = string.Empty;
        protected string displayDate = string.Empty, mainImgCaption = string.Empty, largePicUrl = string.Empty, content = string.Empty, prevPageUrl = string.Empty, nextPageUrl = string.Empty, hostUrl = string.Empty;
        protected bool isMainImageSet = false;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            ProcessQS();
            if (!String.IsNullOrEmpty(_basicId))
                GetNewsDetailsFromApi();
        }

        private void ProcessQS()
        {
            if (Request["id"] != null && Request.QueryString["id"] != string.Empty)
            {
                /** Modified By : Ashwini Todkar on 12 Aug 2014 , add when consuming carwale api
                //Check if basic id exists in mapped carwale basic id log **/
                _basicId = BasicIdMapping.GetCWBasicId(Request["id"]);

                //if id exists then redirect url to new basic id url
                if (!String.IsNullOrEmpty(_basicId))
                {
                    string newUrl = "/news/" + _basicId + "-" + Request["t"] + ".html";
                    CommonOpn.RedirectPermanent(newUrl);    //302 redirection to new basic id
                }
                else
                {
                    _basicId = Request["id"];
                }
            }
            else
            {
                Response.Redirect("/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to fetch news details from api asynchronously
        /// </summary>
        private async void GetNewsDetailsFromApi()
        {
            try
            {
                //sets the base URI for HTTP requests
                string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = "webapi/article/contentdetail/?basicid=" + _basicId;

                objArticle = await BWHttpClient.GetApiResponse<ArticleDetails>(_cwHostUrl, _requestType, _apiUrl, objArticle);

                if (objArticle == null)
                    _isContentFount = false;
                else
                    GetNewsData();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFount)
                {
                    Response.Redirect("/news/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        //PopulateWhere to set news details
        private void GetNewsData()
        {
            articleTitle = objArticle.Title;
            authorName = objArticle.AuthorName;
            displayDate = objArticle.DisplayDate.ToString();
            articleUrl = objArticle.ArticleUrl;
            HostUrl = objArticle.HostUrl;
            basicId = objArticle.BasicId.ToString();
            smallPicUrl = objArticle.SmallPicUrl;
            mainImgCaption = objArticle.MainImgCaption;
            largePicUrl = objArticle.LargePicUrl;
            content = objArticle.Content;
            prevPageUrl = "/news/" + objArticle.PrevArticle.BasicId + "-" + objArticle.PrevArticle.ArticleUrl + ".html";
            nextPageUrl = "/news/" + objArticle.NextArticle.BasicId + "-" + objArticle.NextArticle.ArticleUrl + ".html";
            isMainImageSet = objArticle.IsMainImageSet;
            originalImgUrl = objArticle.OriginalImgUrl;
        }

        /// <summary>
        /// Written By : Ashwini Todkar to get main image
        /// </summary>
        /// <returns></returns>
        protected String GetMainImagePath()
        {
            String mainImgUrl = String.Empty;
            //mainImgUrl = ImagingFunctions.GetPathToShowImages(objArticle.LargePicUrl, objArticle.HostUrl);
            mainImgUrl = ImagingFunctions.GetPathToShowImages(objArticle.OriginalImgUrl, objArticle.HostUrl,Bikewale.Utility.ImageSize._640x348);

            return mainImgUrl;
        }


        /**
        Commented By : Ashwini Todkar on 24 Sept 2014
        all details through CW api againt BasicId, so need to check article url

        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            CustomerId = CurrentUser.Id;

            CommonOpn op = new CommonOpn();
            string newsTitle = string.Empty;


            if (Request["id"] != null && Request.QueryString["id"] != string.Empty)
            {
                /** Modified By : Ashwini Todkar on 12 Aug 2014 , add when consuming carwale api
                //Check if basic id exists in mapped carwale basic id log
                string basicid = BasicIdMapping.GetCWBasicId(Request["id"]);

                //if id exists then redirect url to new basic id url
                if (!String.IsNullOrEmpty(basicid))
                {
                    string newUrl = "/news/" + basicid + "-" + Request["t"] + ".html";
                    CommonOpn.RedirectPermanent(newUrl);
                }
                else
                {
                    if (CommonOpn.CheckId(Request.QueryString["id"]) == true)
                        newsId = Request.QueryString["id"];

                    if (Request["t"] != null)
                    {
                        newsTitle = Request.QueryString["t"];

                        if (CheckTitleTamper(newsTitle, newsId))
                        {
                            Response.Redirect("/pagenotfound.aspx", true);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                }
            }
            else
            {
                Trace.Warn("newsId: " + newsId);
                Response.Redirect("/pagenotfound.aspx", true);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            GetNewsDetailsFromApi();
        }
        Commented By : Ashwini Todkar on 24 Sept 2014
        all details through CW api againt BasicId, so need to check article url

        private bool CheckTitleTamper(string newsTitle, string newsId)
        {
            string sql = string.Empty;
            Database db = new Database();
            SqlDataReader dr = null;
            bool isTampered = false;

            sql = "Select Url From Con_EditCms_Basic With(NoLock) Where Id = @NewsId";

            SqlParameter[] param = { new SqlParameter("@NewsId", newsId) };

            try
            {
                dr = db.SelectQry(sql, param);

                while (dr.Read())
                {
                    url = dr["Url"].ToString();
                }

                Trace.Warn("URL : " + url);
                Trace.Warn("NewsTitle : " + newsTitle);
                if (url != newsTitle)
                {
                    isTampered = true;
                }
                Trace.Warn("isTampered: " + isTampered.ToString());

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                db.CloseConnection();
            }

            return isTampered;
        }


        Commented By : Ashwini Todkar on 24 Sept 2014
        No need to update views as when page requests for api the views get updated on carwale

        private bool UpdateView()
        {
            bool retData = false;
            Database db = new Database();
            string sql = string.Empty;

            try
            {
                sql = " Update Con_EditCms_Basic Set Views = Views + 1 Where IsActive = 1 AND CategoryId IN(1,12) AND "
                    + " Id = @BasicId ";

                if (!IsPreview)
                {
                    sql += " AND IsPublished = 1 ";
                }

                Trace.Warn(sql);

                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = newsId;

                cmd.CommandText = sql;
                retData = db.UpdateQry(cmd);

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retData;
        }

        Commented By : Ashwini Todkar on 24 Sept 2014
        Not more require as get all details from CW api 

         <summary>
         Modified By:Prashant Vishe On 02 Auguest 2013
         Modification:Added next and prev url related details and converted inline sql query to stored procedure for retrieving news details..
         </summary>
        private void FillNewsDetails()
        {
            bool foundData = false;
            string prevNewsUrlSub = string.Empty, nextNewsUrlSub = string.Empty, prevId = string.Empty, nextId = string.Empty;

            Database db = new Database();
            string conStr = db.GetConString();

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("GetNewsPageDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Inputs
                        cmd.Parameters.Add("@BasicId", SqlDbType.Int).Value = newsId;
                        cmd.Parameters.Add("@IsPublished", SqlDbType.Bit).Value = IsPreview ? false : true;

                        // Outputs, News Details

                        cmd.Parameters.Add("@AuthorName", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@DisplayDate", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Title", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Url", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Views", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Content", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ImagePathLarge", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MainImgCaption", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MainImgSet", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CommentCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                        //Retrieve to set facebook like image
                        cmd.Parameters.Add("@ImagePathThumbnail", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

                        // Outputs, Next and Previous articles
                        cmd.Parameters.Add("@NextId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NextUrl", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NextTitle", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PrevId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PrevUrl", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PrevTitle", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;


                        con.Open();
                        cmd.ExecuteNonQuery(); //run the command

                        // Get New Details

                        authorName = cmd.Parameters["@AuthorName"].Value.ToString();
                        displayDate = cmd.Parameters["@DisplayDate"].Value.ToString();
                        _title = cmd.Parameters["@Title"].Value.ToString();
                        url = cmd.Parameters["@Url"].Value.ToString();
                        views = cmd.Parameters["@Views"].Value.ToString();
                        content = cmd.Parameters["@Content"].Value.ToString();
                        HostUrl = cmd.Parameters["@HostUrl"].Value.ToString();
                        isMainImageSet = String.IsNullOrEmpty(cmd.Parameters["@MainImgSet"].Value.ToString()) ? false : Convert.ToBoolean(cmd.Parameters["@MainImgSet"].Value.ToString());
                        largeImagePath = cmd.Parameters["@ImagePathLarge"].Value.ToString();
                        ImagePathThumbnail = cmd.Parameters["@ImagePathThumbnail"].Value.ToString();
                        MainImgCaption = cmd.Parameters["@MainImgCaption"].Value.ToString();
                        CommentCount = cmd.Parameters["@CommentCount"].Value.ToString();

                        // Next, Previous new articles
                        nextId = cmd.Parameters["@NextId"].Value.ToString();
                        prevId = cmd.Parameters["@PrevId"].Value.ToString();
                        nextNewsUrlSub = cmd.Parameters["@NextUrl"].Value.ToString();
                        prevNewsUrlSub = cmd.Parameters["@PrevUrl"].Value.ToString();
                        nextNewsTitle = cmd.Parameters["@NextTitle"].Value.ToString();
                        prevNewsTitle = cmd.Parameters["@PrevTitle"].Value.ToString();
                        prevNewsUrl = "/news/" + prevId + "-" + prevNewsUrlSub + ".html";
                        nextNewsUrl = "/news/" + nextId + "-" + nextNewsUrlSub + ".html";
                        Trace.Warn("prev news url is: " + prevNewsUrl + "  prev news title is: " + prevNewsTitle);
                        Trace.Warn("next news url is: " + nextNewsUrl + "  next news title is: " + nextNewsTitle);

                        foundData = true;
                    }
                }// db connection will be closed automatically.
                Trace.Warn("thumbnail : ", ImagePathThumbnail);
                fbLogoUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(ImagePathThumbnail, HostUrl);
                //Trace.Warn("FB logo URL :", fbLogoUrl);
            }
            catch (SqlException err)
            {
                //Trace.Warn("sql ex:" + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                //Trace.Warn("ex:" + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            if (foundData == false)
            {
                Response.Redirect("/news/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        Commented By : Ashwini Todkar on 24 Sept 2014
        Not more needed as details getting through CW api 

        /// <summary>
        /// Gets the Tags based on the News Id
        /// </summary>
        /// <param name="Id">News Id</param>
        /// <returns>DataTable</returns>
        public DataTable GetTags(string Id)
        {
            DataTable dt = null;
            string sql = string.Empty;
            SqlCommand cmd;
            Database db = new Database();
            try
            {
                sql = " Select BT.BasicId, CT.Tag, CT.Slug From Con_EditCms_Tags CT With(NoLock) "
                    + " Inner Join Con_EditCms_BasicTags BT With(NoLock) On BT.TagId = CT.Id "
                    + " Where BT.BasicId = @BasicId ";
                cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = Id;

                dt = db.SelectAdaptQry(cmd).Tables[0];
                Trace.Warn("GetTags done");
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return dt;
        }

         <summary>
         Gets the SubCategories that the Article is listed in for the Category News.
         </summary>
         <param name="Id">News Id</param>
         <returns>DataTable</returns>
        public DataTable GetSubCat(string Id)
        {
            DataTable dt = null;
            string sql = string.Empty;
            SqlCommand cmd;
            Database db = new Database();

            try
            {
                sql = " Select BSC.BasicId, SC.Id, SC.Name From Con_EditCms_SubCategories SC With(NoLock) "
                    + " Inner Join Con_EditCms_BasicSubCategories BSC With(NoLock) On BSC.SubCategoryId = SC.Id "
                    + " Where BSC.BasicId = @BasicId ";
                cmd = new SqlCommand(sql);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = Id;

                dt = db.SelectAdaptQry(cmd).Tables[0];
                Trace.Warn("GetSubCat done");
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return dt;
        }**/
    }
}