using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Entities.CMS.Articles;
using System.Configuration;
using Bikewale.Entities.CMS;

namespace Bikewale.Controls
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 6 oct 2014 to get news from api
    /// </summary>
    public partial class NewsMin : System.Web.UI.UserControl
    {
        protected Repeater rptCarNews;
        protected HtmlGenericControl divControl;
        private int posts = 5, recordCount = 0;
        private string car = "";
        protected string isExpandable = "1", basicId = string.Empty, authorName = string.Empty, description = string.Empty, displayDate = string.Empty, views = string.Empty,
            title = string.Empty, url = string.Empty, imagePathCustom = string.Empty, hostUrl = string.Empty;
        protected Label lblNotFound;

        public int Posts
        {
            get { return posts; }
            set { posts = value; }
        }

        public string Car
        {
            get { return car; }
            set { car = value; }
        }

        public string IsExpandable
        {
            get { return isExpandable; }
            set { isExpandable = value; }
        }

        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }
        public bool FetchAllRecord { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string SeriesId { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //LoadNews();
                LoadNews();
            }
        }

        public async void LoadNews()
        {
            try
            {
                List<ArticleSummary> _objArticleList = null;

                int _contentType = (int)EnumCMSContentType.News;
                string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + posts;


                if (!String.IsNullOrEmpty(MakeId) || !String.IsNullOrEmpty(ModelId))
                {
                    if (!String.IsNullOrEmpty(ModelId))
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + posts + "&makeid=" + MakeId + "&modelid=" + ModelId;
                    else
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + posts + "&makeid=" + MakeId;
                }

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objArticleList = await objClient.GetApiResponse<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objArticleList);
                }
                
                if (_objArticleList != null && _objArticleList.Count > 0)
                {
                    RecordCount = _objArticleList.Count;

                    divControl.Attributes.Remove("class");

                    ArticleSummary objFirstArticle = _objArticleList[0];

                    GetFirstNews(objFirstArticle);

                    _objArticleList.RemoveAt(0);

                    rptCarNews.DataSource = _objArticleList;
                    rptCarNews.DataBind();
                }
                else
                    divControl.Attributes.Add("class", "hide");
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            Trace.Warn("++Model ID :", ModelId);
            Trace.Warn("+++REcord count : ", recordCount.ToString());
        }

        private void GetFirstNews(ArticleSummary _objArticle)
        {
            basicId = _objArticle.BasicId.ToString();
            authorName = _objArticle.AuthorName;
            description = _objArticle.Description;
            displayDate =_objArticle.DisplayDate.ToString();
            views = _objArticle.Views.ToString();
            title = _objArticle.Title;
            url = _objArticle.ArticleUrl;
            //imagePathCustom = _objArticle.SmallPicUrl;
            imagePathCustom = _objArticle.OriginalImgUrl;
            hostUrl = _objArticle.HostUrl;
        }
        
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 27th March 2014
        /// Summary : function to retrieve news
        /// </summary>
        //private void LoadNews()
        //{
        //    DataSet ds = null;
        //    SqlCommand cmd;
        //    Database db = new Database();

        //    //string sql = string.Empty;
        //    //string selectClause = string.Empty;
        //    //string fromClause = string.Empty;
        //    //string joinClause = string.Empty;
        //    //string whereClause = string.Empty;
        //    //string orderClause = string.Empty;
        //    //bool keyPresent = false;

        //    try
        //    {
        //        //selectClause = " SELECT TOP " + Posts.ToString() + " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url ";
        //        //fromClause = " FROM Con_EditCms_Basic AS CB ";
        //        //whereClause = " WHERE CB.CategoryId = 1 AND CB.IsActive = 1 AND CB.IsPublished = 1";
        //        //orderClause = " ORDER BY CB.LastUpdatedTime DESC ";

        //        //if (Keyword != string.Empty)
        //        //{
        //        //    keyPresent = true;

        //        //    joinClause = "Left Join Con_EditCms_BasicTags BT On BT.BasicId = CB.Id "
        //        //               + "Left Join Con_EditCms_Tags T On T.Id = BT.TagId ";
        //        //    whereClause += " And T.Slug = @Keyword ";                   
        //        //}

        //        //sql = selectClause + fromClause + joinClause + whereClause + orderClause;

        //        cmd = new SqlCommand();
        //        cmd.CommandText = "NewsMin";
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = Posts;
        //        cmd.Parameters.Add("@FetchAllRecords", SqlDbType.Bit).Value = FetchAllRecord;
        //        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = MakeId;
        //        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = ModelId;
        //        cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = SeriesId;

        //        Trace.Warn("here");
        //        ds = db.SelectAdaptQry(cmd);

        //        recordCount = ds.Tables[0].Rows.Count;

        //        // to get 1st row
        //        if (ds != null && ds.Tables[0].Rows.Count>0)
        //        {
        //            divControl.Attributes.Remove("class");
        //            DataRow dr = ds.Tables[0].Rows[0];
        //            GetFirstNews(dr);

        //            //delete 1st row
        //            ds.Tables[0].Rows[0].Delete();
        //            ds.Tables[0].AcceptChanges();
        //        }
        //        else
        //            divControl.Attributes.Add("class", "hide");

        //        if (ds != null)
        //        {
        //            divControl.Attributes.Remove("class");
        //            rptCarNews.DataSource = ds;
        //            rptCarNews.DataBind();
        //        }
        //        else
        //            divControl.Attributes.Add("class", "hide");

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }

        //    Trace.Warn("++Model ID :", ModelId);
        //    Trace.Warn("+++REcord count : ", recordCount.ToString());
        //}

        protected string TruncateDesc(string _desc)
        {
            _desc = Regex.Replace(_desc, @"<[^>]+>", string.Empty);

            if (_desc.Length < 170)
                return _desc;
            else
            {
                _desc = _desc.Substring(0, 165);
                _desc = _desc.Substring(0, _desc.LastIndexOf(" "));
                return _desc + " [...]";
            }
        }

        protected string GetPubDate(string _PubDate)
        {
            if (_PubDate.ToString() == "")
                return "";
            else
                return "- " + Convert.ToDateTime(_PubDate).ToString("dd MMM yyyy");
        }

        //protected void GetFirstNews(DataRow dr)
        //{
        //    basicId = dr["BasicId"].ToString();
        //    authorName = dr["AuthorName"].ToString();
        //    description = dr["Description"].ToString();
        //    displayDate = dr["DisplayDate"].ToString();
        //    views = dr["Views"].ToString();
        //    title = dr["Title"].ToString();
        //    url = dr["Url"].ToString();
        //    imagePathCustom = dr["ImagePathCustom"].ToString();
        //    hostUrl = dr["HostUrl"].ToString();
        //}
    }//class
}//namespace