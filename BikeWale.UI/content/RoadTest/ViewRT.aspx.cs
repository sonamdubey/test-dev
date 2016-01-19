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
using Bikewale.Entities.CMS.Articles;
using System.Text;
using Bikewale.Controls;
using Bikewale.Entities.CMS.Photos;
using System.Linq;

namespace Bikewale.Content
{
    public class ViewRT : System.Web.UI.Page
    {
        protected Repeater rptPages, rptPageContent;
        private string _basicId = string.Empty;
        protected ArticlePageDetails objRoadtest;
        protected StringBuilder _bikeTested;
        protected ArticlePhotoGallery ctrPhotoGallery;
        private bool _isContentFount = true;

        protected string articleUrl = string.Empty, articleTitle = string.Empty, basicId = string.Empty, authorName = string.Empty, displayDate = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection deviceDetection = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            deviceDetection.DetectDevice();


            ProcessQS();
            GetRoadtestDetails();
            GetArticlePhotos();
        }

        

        /// <summary>
        /// 
        /// </summary>
        private void ProcessQS()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                //id = Request.QueryString["id"].ToString();
                _basicId = Request.QueryString["id"];

                string basicId = BasicIdMapping.GetCWBasicId(_basicId);

                Trace.Warn("Carwale basic id : " + basicId);

                if (!String.IsNullOrEmpty(basicId))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_REWRITE_URL"];
                    var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = "/road-tests/" + _newUrlTitle + basicId + ".html";
                    CommonOpn.RedirectPermanent(_newUrl);
                }
            }
            else
            {
                Response.Redirect("/road-tests/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to fetch roadtest details from api asynchronously
        /// </summary>
       
        private async void GetRoadtestDetails()
        {
            try
            {
                string _apiUrl = "webapi/article/contentpagedetail/?basicid=" + _basicId;
              
                // Send HTTP GET requests 
                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objRoadtest = await objClient.GetApiResponse<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objRoadtest);
                }
                
                if (objRoadtest != null)
                {
                    BindPages();
                    GetRoadtestData();
                    if(objRoadtest.VehiclTagsList.Count > 0)
                        GetTaggedBikeList();
                }
                else
                {
                    _isContentFount = false;
                }
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
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private async void GetArticlePhotos()
        {
            try
            {                
                string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + _basicId;
                
                List<ModelImage> objImg = null;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objImg = await objClient.GetApiResponse<List<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objImg);
                }
                
                if (objImg != null && objImg.Count > 0)
                {
                    ctrPhotoGallery.BasicId = Convert.ToInt32(_basicId);
                    ctrPhotoGallery.ModelImageList = objImg;
                    ctrPhotoGallery.BindPhotos();
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetTaggedBikeList()
        {
            _bikeTested =  new StringBuilder();

            _bikeTested.Append("Bike Tested: ");

            IEnumerable<int> ids = objRoadtest.VehiclTagsList
                   .Select(e => e.ModelBase.ModelId)
                   .Distinct();

            foreach (var i in ids)
            {
                VehicleTag item = objRoadtest.VehiclTagsList.Where(e => e.ModelBase.ModelId == i).First();
                    _bikeTested.Append("<a title='" + item.MakeBase.MakeName + " " + item.ModelBase.ModelName + " Bikes' href='/" + item.MakeBase.MakeName.ToLower() + "-bikes/" + item.ModelBase.MaskingName + "/'>" + item.ModelBase.ModelName + "</a>   ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindPages()
        {
            rptPages.DataSource = objRoadtest.PageList;
            rptPages.DataBind();

            rptPageContent.DataSource = objRoadtest.PageList;
            rptPageContent.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetRoadtestData()
        {
            articleTitle = objRoadtest.Title;
            authorName = objRoadtest.AuthorName;
            displayDate = objRoadtest.DisplayDate.ToString();
            articleUrl = objRoadtest.ArticleUrl;
            basicId = objRoadtest.BasicId.ToString();
        }

        //Commented By : Ashwini Todkar on 29 Sept 2014
        //Modified all methods for getting data from carwale api

        //protected Label /*lblHeading, lblTitle,*/ lblAuthor, lblDate, lblDetails, /*lblMethodologies,*/ lblphoto, lblphoto1/*, lblBikeNames*/;
        //protected DropDownList drpPages, drpPages_footer;
        //protected Repeater rptPages, rptPages_footer;
        //protected DataList dlstPhoto;
        //protected HtmlGenericControl topNav, bottomNav, divOtherInfo;
        //protected string threadId = string.Empty;
        //protected string Priority = string.Empty;
        //protected string ArticleTitle = string.Empty, ArticleHeading = string.Empty, MakeName = string.Empty, ModelName = string.Empty, VersionName = string.Empty, prevUrl = string.Empty, nextUrl = string.Empty,fbLogoUrl = string.Empty;
        //protected string str = string.Empty;
        //protected int strCount,totalPages;
        //public string id = string.Empty, pageid = "1", articleType = string.Empty, type = string.Empty;	// number of links in the strip
        //protected string modelId = string.Empty, versionId = string.Empty;
        //protected bool ShowGallery = false;
        //protected bool IsPhotoGalleryPage = false;
        //protected string MakeMaskName = string.Empty, ModelMaskName = string.Empty;

        //protected override void OnInit(EventArgs e)
        //{         
        //    base.Load += new EventHandler(Page_Load);
        //    //this.drpPages.SelectedIndexChanged += new EventHandler(drpPages_SelectedIndexChanged);
        //    //this.drpPages_footer.SelectedIndexChanged += new EventHandler(drpPages_footer_SelectedIndexChanged);
        //}

        //private void Page_Load(object sender, EventArgs e)
        //{
        //    //code for device detection added by Ashwini Todkar
        //    DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
        //    dd.DetectDevice();

        //    Trace.Warn("Test");
        //    if (Request["pageId"] != null && Request.QueryString["pageId"].ToString() != "")
        //    {
        //        pageid = Request.QueryString["pageId"].ToString();

        //        if (CommonOpn.CheckId(pageid) == false)
        //        {
        //            return;
        //        }
        //    }

        //    if (!String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
        //    {
        //        id = Request.QueryString["id"].ToString();
        //        string basicId = BasicIdMapping.GetCWBasicId(id);
        //        Trace.Warn("Carwale basic id : " + basicId);

        //        if (!String.IsNullOrEmpty(basicId))
        //        {
        //            string _newUrl = Request.ServerVariables["HTTP_X_REWRITE_URL"];
        //            var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
        //            var _titleEndIndex = _newUrl.LastIndexOf('-');
        //            string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex+1);
        //            _newUrl = "/road-tests/" + _newUrlTitle + basicId + ".html";
        //            CommonOpn.RedirectPermanent(_newUrl);
        //            //Trace.Warn("_newUrl : " + _newUrl);
        //        }
        //        // Trace.Warn("url : " + Request.ServerVariables["HTTP_X_REWRITE_URL"]);
        //        // string newurl = Request.ServerVariables["HTTP_X_REWRITE_URL"];
        //        // Trace.Warn("new url : " + newurl);
        //        // var suffix = newurl.LastIndexOf('/');
        //        // Trace.Warn("index of / : " + suffix);
        //        // var dot = newurl.LastIndexOf('-');
        //        // Trace.Warn("id : " + dot);
        //        //Trace.Warn("Url title : " + newurl.Substring(suffix,dot-suffix+1));

        //    }
        //    else
        //    {
        //        id = "0";
        //    }
        //    Trace.Warn("ID: " + id);

        //    if (id != "0" && pageid != "0")
        //    {
        //        GetPageData();

        //        if (!IsPostBack)
        //        {
        //            FillPages();
        //            GetFbImageUrl();
        //            drpPages.SelectedValue = pageid;//drpPages.Items.IndexOf(drpPages.Items.FindByValue( pageid ));
        //            drpPages_footer.SelectedValue = pageid;//drpPages.Items.IndexOf(drpPages.Items.FindByValue( pageid ));
        //        }
        //        CreatePrevNextUrl();
        //    }
        //    else
        //    {
        //        Response.Redirect("/road-tests/", false);
        //        System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        //    }
        //}


        //private void CreatePrevNextUrl()
        //{
        //    Trace.Warn("inside create URL ", Url);
        //    string mainUrl = "http://www.bikewale.com/road-tests/" + Url + "-" + id + "/p";
        //    string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
        //    Trace.Warn("Page Id : ", pageid);
        //    Trace.Warn("totalPages : ", totalPages.ToString());     
        //    if (pageid == string.Empty || pageid == "1")    //if page is first page
        //    {
        //        nextPageNumber = "2";
        //        nextUrl = mainUrl + nextPageNumber + "/";
        //    }
        //    else if (int.Parse(pageid) == totalPages)    //if page is last page
        //    {
        //        prevPageNumber = (int.Parse(pageid) - 1).ToString();
        //        prevUrl = mainUrl + prevPageNumber + "/";
        //    }
        //    else
        //    {          //for middle pages
        //        prevPageNumber = (int.Parse(pageid) - 1).ToString();
        //        prevUrl = mainUrl + prevPageNumber + "/";
        //        nextPageNumber = (int.Parse(pageid) + 1).ToString();
        //        nextUrl = mainUrl + nextPageNumber + "/";
        //    }
        //    Trace.Warn("-----previous page url is :  " + prevUrl);
        //    Trace.Warn("-----Next page url is :  " + nextUrl);
        //}

        //void CheckPage()
        //{
        //    if (pageid == str)
        //    {
        //        Trace.Warn("CheckPage() str: " + str);
        //        IsPhotoGalleryPage = true;
        //        LoadPhotos();
           
        //    }
        //    else if (pageid != str)
        //    {
        //        IsPhotoGalleryPage = false;
        //    }
        //}

        //DataSet LoadSqlDataToCache()
        //{
        //    string sql = string.Empty;

        //    Database db = new Database();
        //    DataSet ds = null;
        //    DataSet dsPageData = new DataSet();

        //    DataTable dtPageData = new DataTable("PageData");
        //    Trace.Warn("@BasicId" + id);
        //    SqlParameter[] param = { new SqlParameter("@BasicId", id) };

        //    // Data to fill all the details on the page

        //    sql = " Select B.Title, B.AuthorName, B.Url, B.DisplayDate, CMA.Name As Make,CMA.MaskingName AS MakeMaskingName, CMO.Name As Model,CMO.MaskingName AS ModelMaskingName, CV.Name As Version, C.ModelId, C.VersionId, "
        //        + "		Case When C.VersionId = -1 Then CMA.Name + ' ' + CMO.Name  Else CMA.Name + ' ' + CMO.Name + ' ' + CV.Name End As Bike, CA.Name As Category, CF.FieldName, CF.ValueType, "
        //        + "		Case When CF.ValueType = 1 Then Cast(OI.BooleanValue As VarChar(1)) When CF.ValueType = 2 Then Cast(OI.NumericValue As VarChar(15)) When CF.ValueType = 3 Then Cast(OI.DecimalValue As VarChar) "
        //        + "		 When CF.ValueType = 4 Then OI.TextValue  When CF.ValueType = 5 Then Convert(VarChar, OI.DateTimeValue, 103) Else '' End As OtherInfoValue, ( Cast( P.Priority As VarChar(10) ) + '. ' + P.PageName ) As PageNameForDDL, P.PageName, P.Priority, PC.Data, SC.Name As SubCategory "
        //        + "	From Con_EditCms_Basic B With(NoLock) "
        //        + "     Left Join Con_EditCms_BasicSubCategories BSC With(NoLock) On BSC.BasicId = B.Id "
        //        + "     Left Join Con_EditCms_SubCategories SC With(NoLock) On SC.Id = BSC.SubCategoryId And SC.IsActive = 1 "
        //        + "		Left Join Con_EditCms_Bikes C With(NoLock) On C.BasicId = B.Id And C.IsActive = 1 "
        //        + "		Left Join Con_EditCms_OtherInfo OI With(NoLock) On OI.BasicId = B.Id "
        //        + "		Left Join Con_EditCms_CategoryFields CF With(NoLock) On CF.Id = OI.CategoryFieldId And B.CategoryId = CF.CategoryId "
        //        + "		Left Join Con_EditCms_Category Ca With(NoLock) On Ca.Id = B.CategoryId "
        //        + " 	Left Join Con_EditCms_Pages P With(NoLock) On P.BasicId = B.Id "
        //        + " 	Left Join Con_EditCms_PageContent PC With(NoLock) On PC.PageId = P.Id "
        //        + "		Left JOIN BikeMakes CMA With(NoLock) On C.MakeId = CMA.ID "
        //        + "		Left JOIN BikeModels CMO With(NoLock) On C.ModelId = CMO.ID "
        //        + "		Left JOIN BikeVersions CV With(NoLock) On C.VersionId = CV.ID "
        //        + "	Where "
        //        + "		B.ID = @BasicId "
        //        + " Order By P.Priority ";

        //    Trace.Warn("sql: " + sql);

        //    try
        //    {
        //        ds = db.SelectAdaptQry(sql, param);

        //        if (ds.Tables.Count > 0)
        //        {
        //            dtPageData = ds.Tables[0].Copy();
        //            Trace.Warn("page data : ", dtPageData.Rows.Count.ToString());
        //            Trace.Warn("Page Id new : ", pageid);

        //            if (dtPageData.Rows.Count > 0)
        //            {
        //                dsPageData.Tables.Add(dtPageData);
        //                Cache.Insert("dsBikeCompDataCache", dsPageData, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);
        //            }
        //            else
        //            {
        //                Response.Redirect("http://www.bikewale.com/road-tests/", false);
        //                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.Warn(ex.Message);
        //    }

        //    return dsPageData;
        //}

        //private void LoadPhotos()
        //{
        //    string sql = " SELECT * FROM Con_EditCms_Images CI With(NoLock) "
        //                + " Inner Join Con_PhotoCategory CP With(NoLock) On CP.Id = CI.ImageCategoryId "
        //                + " WHERE BasicId = @BasicId AND IsActive = 1 ORDER BY Sequence ASC";
        //    try
        //    {
        //        CommonOpn op = new CommonOpn();
        //        SqlParameter[] param = { new SqlParameter("@BasicId", id) };
        //        op.BindListReader(sql, dlstPhoto, param);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}

        ///// <summary>
        /////     Written By : Ashwini V. Todkar on 23/oct/2013
        /////     Function will return the fullimage path if hosturl or imagepath is not null for meta tag of Facebook like.
        /////     Else will return the nobike image
        ///// </summary>  
        //protected void GetFbImageUrl()
        //{            
        //    Database db = null;                 
        //    DataSet ds = null;            

        //    string hostUrl = string.Empty, imagePath = string.Empty;

        //    string sql = "SELECT HostUrl,ImagePathThumbnail  FROM Con_EditCms_Images CI With(NoLock) " +
        //                 "Inner Join Con_PhotoCategory CP With(NoLock) On CP.Id = CI.ImageCategoryId " +
        //                 "WHERE BasicId = @BasicId AND IsActive = 1 AND IsMainImage = 1  ";
            
        //    try
        //    {
        //        db = new Database();

        //        using (SqlCommand cmd = new SqlCommand())
        //        {                    
        //            cmd.CommandText = sql;
        //            cmd.Parameters.Add("@BasicId", System.Data.SqlDbType.Int).Value = id;

        //            ds = db.SelectAdaptQry(cmd);
                    
        //            DataTable dt = ds.Tables[0];
                    
        //            if (dt.Rows.Count > 0)
        //            {
        //                hostUrl =  dt.Rows[0]["HostUrl"].ToString();
        //                imagePath = dt.Rows[0]["ImagePathThumbnail"].ToString();

        //                fbLogoUrl = ImagingOperations.GetPathToShowImages(imagePath, hostUrl);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Trace.Warn("GetFbImageUrl : ", ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.Warn("GetFbImageUrl Ex : ", ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //} // End of GetFbImageUrl function


        //protected string CreateNavigationLink(string _priority)
        //{
        //    if (_priority == pageid)
        //    {
        //        return "<span class='pgSel'><acronym><b>" + _priority + "</b></acronym></span>";
        //    }
        //    else
        //    {
        //        string url = HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"];           
        //        if (url.IndexOf(".html") > 0)
        //        {
        //            url = url.Substring(0, url.IndexOf(".html")) + "/p" + _priority + "/";
        //        }
        //        else if (url.IndexOf("/p") > 0)
        //        {
        //            url = url.Substring(0, url.IndexOf("/p")) + "/p" + _priority + "/";                
        //        }
        //        return "<span class='pg'><a href=\"" + url + "\">" + _priority + "</a></span>";
        //    }
        //}

        //void FillPages()
        //{
        //    string sql = string.Empty;

        //    Database db = new Database();
        //    SqlDataReader dr = null;

        //    try
        //    {
        //        CommonOpn op = new CommonOpn();
        //        DataSet ds = new DataSet();

        //        if ((DataSet)Cache.Get("dsBikeCompDataCache") != null)
        //        {
        //            ds = (DataSet)Cache.Get("dsBikeCompDataCache");
        //        }
        //        else
        //        {
        //            ds = LoadSqlDataToCache();
        //        }

        //        // If data is available then only bind the data
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                DataTable distinctTable = ds.Tables[0].DefaultView.ToTable("DistinctTable", true, new string[] { "PageNameForDDL", "Priority" });
        //                DataRow[] distinctRows = distinctTable.Select("", "Priority ASC");

        //                DataTable dt = new DataTable();
        //                DataColumn column;
        //                DataRow row;

        //                Trace.Warn("distinctRows: " + distinctRows.Length.ToString());

        //                /*Create new DataColumn, Add ColumnName and add to DataTable. 
        //                The following is done since we cannont bind DataRows to the control directly*/
        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.String");
        //                column.ColumnName = "PageName";
        //                column.ReadOnly = true;

        //                dt.Columns.Add(column);

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.Int32");
        //                column.ColumnName = "Priority";
        //                column.ReadOnly = true;

        //                dt.Columns.Add(column);

        //                for (int i = 0; i < distinctRows.Length; ++i)
        //                {
        //                    row = dt.NewRow();
        //                    row["PageName"] = distinctRows[i]["PageNameForDDL"].ToString();
        //                    row["Priority"] = distinctRows[i]["Priority"].ToString() == "" ? 0 : Convert.ToInt32(distinctRows[i]["Priority"].ToString());
        //                    dt.Rows.Add(row);
        //                }

        //                if (distinctRows.Length <= 1)
        //                {
        //                    topNav.Visible = false;
        //                    bottomNav.Visible = false;
        //                }

        //                // Bind the New DataTable to the control and set the Text and Value fields
        //                drpPages.DataSource = dt;
        //                drpPages.DataTextField = "PageName";
        //                drpPages.DataValueField = "Priority";
        //                drpPages.DataBind();

        //                drpPages_footer.DataSource = dt;
        //                drpPages_footer.DataTextField = "PageName";
        //                drpPages_footer.DataValueField = "Priority";
        //                drpPages_footer.DataBind();

        //                strCount = drpPages.Items.Count;
        //                str = (strCount + 1).ToString();
        //                Trace.Warn("str Count ", strCount.ToString());
        //                totalPages = strCount;
        //                //if (strCount < 6)
        //                //    totalPages = Convert.ToInt32(str);
        //                //else
        //                //    totalPages = Convert.ToInt32(str) - 1;

        //                Trace.Warn("string  :", str);
        //                SqlParameter[] param = { new SqlParameter("@BasicId", id) };

        //                sql = " Select ShowGallery From Con_EditCms_Basic With(NoLock) Where Id = @BasicId";

        //                dr = db.SelectQry(sql, param);

        //                while (dr.Read())
        //                {
        //                    ShowGallery = Convert.ToBoolean(dr["ShowGallery"].ToString());
        //                }

        //                //dr.Close();

        //                if (ShowGallery)
        //                {
        //                    if (distinctRows.Length >= 1)
        //                    {
        //                        topNav.Visible = true;
        //                        bottomNav.Visible = true;
        //                    }
        //                    // If Photo Gallery is to be shown, then add the extra List Item
        //                    drpPages.Items.Insert(strCount, new ListItem((strCount + 1).ToString() + ". Photos", str));
        //                    drpPages_footer.Items.Insert(strCount, new ListItem((strCount + 1).ToString() + ". Photos", str));

        //                    drpPages.SelectedValue = pageid;
        //                    drpPages_footer.SelectedValue = pageid;
        //                    totalPages++;
        //                    CheckPage();
        //                }

        //                distinctTable = ds.Tables[0].DefaultView.ToTable("DistinctTable", true, "Priority");

        //                distinctRows = distinctTable.Select("", "Priority ASC");
        //                Trace.Warn("distinctRows 2: " + distinctRows.Length.ToString());
        //                dt = new DataTable();

        //                column = new DataColumn();
        //                column.DataType = System.Type.GetType("System.Int32");
        //                column.ColumnName = "Priority";
        //                column.ReadOnly = true;

        //                dt.Columns.Add(column);

        //                for (int i = 0; i < distinctRows.Length; ++i)
        //                {
        //                    row = dt.NewRow();
        //                    row["Priority"] = distinctRows[i]["Priority"].ToString() == "" ? 0 : Convert.ToInt32(distinctRows[i]["Priority"].ToString());
        //                    dt.Rows.Add(row);
        //                }

        //                rptPages.DataSource = dt;
        //                rptPages.DataBind();

        //                rptPages_footer.DataSource = dt;
        //                rptPages_footer.DataBind();
        //            }
        //        }
        //    }
        //    catch (SqlException err)
        //    {
        //        Trace.Warn("sqlError: " + err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn("Error FillPages: " + err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    finally
        //    {
        //        if (dr != null)
        //            dr.Close();
        //        db.CloseConnection();
        //    }
        //}

        //void GetPageData()
        //{
        //    string[] pageNames, pageData;
        //    string[] bikeNames, makes, models, versions, modelIds, versionIds;
        //    string[] fieldName, valueType, otherInfoValue;
        //    string[] MakeMaskingNames, ModelMaskingNames;

        //    string title = string.Empty;
        //    string authorName = string.Empty;
        //    string displayDate = string.Empty;
        //    string category = string.Empty;
        //    string description = string.Empty;
        //    string subCategory = string.Empty;

        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        ds = LoadSqlDataToCache();
        //        dt = ds.Tables[0];
        //        Trace.Warn("Row Count : ", dt.Rows.Count.ToString());

        //        if (dt.Rows.Count > 0)
        //        {
        //            DataTable distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "PageName", "Data" });

        //            DataRow[] distinctRows = distinctTable.Select();
        //            pageNames = new string[distinctRows.Length];
        //            pageData = new string[distinctRows.Length];

        //            for (int i = 0; i < distinctRows.Length; ++i)
        //            {
        //                pageNames[i] = " " + distinctRows[i]["PageName"].ToString();

        //                if (!IsPhotoGalleryPage)
        //                {
        //                    pageData[i] = distinctRows[i]["Data"].ToString();
        //                }
        //            }

        //            distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "bike", "Make", "Model", "Version", "ModelId", "VersionId" ,"MakeMaskingName" ,"ModelMaskingName"});
        //            distinctRows = distinctTable.Select();

        //            bikeNames = new string[distinctRows.Length];
        //            makes = new string[distinctRows.Length];
        //            models = new string[distinctRows.Length];
        //            versions = new string[distinctRows.Length];
        //            modelIds = new string[distinctRows.Length];
        //            versionIds = new string[distinctRows.Length];
        //            MakeMaskingNames = new string[distinctRows.Length];
        //            ModelMaskingNames = new string[distinctRows.Length];

        //            for (int i = 0; i < distinctRows.Length; ++i)
        //            {
        //                bikeNames[i] = " " + distinctRows[i]["bike"].ToString();
        //                makes[i] = " " + distinctRows[i]["Make"].ToString();
        //                models[i] = " " + distinctRows[i]["Model"].ToString();
        //                versions[i] = " " + distinctRows[i]["Version"].ToString();
        //                modelIds[i] = " " + distinctRows[i]["ModelId"].ToString();
        //                versionIds[i] = " " + distinctRows[i]["VersionId"].ToString();
        //                MakeMaskingNames[i] = distinctRows[i]["MakeMaskingName"].ToString();
        //                ModelMaskingNames[i] = distinctRows[i]["ModelMaskingName"].ToString();
        //            }

        //            distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "Title", "Url", "AuthorName", "DisplayDate", "Category", "SubCategory" });
        //            distinctRows = distinctTable.Select();

        //            title = distinctRows[0]["Title"].ToString();
        //            Url = distinctRows[0]["Url"].ToString();
        //            authorName = distinctRows[0]["AuthorName"].ToString();
        //            displayDate = Convert.ToDateTime(distinctRows[0]["DisplayDate"]).ToString("dd-MMM-yyyy");
        //            category = distinctRows[0]["Category"].ToString();
        //            subCategory = distinctRows[0]["SubCategory"].ToString();

        //            distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "FieldName", "ValueType", "OtherInfoValue" });
        //            distinctRows = distinctTable.Select();

        //            fieldName = new string[distinctRows.Length];
        //            valueType = new string[distinctRows.Length];
        //            otherInfoValue = new string[distinctRows.Length];

        //            for (int i = 0; i < distinctRows.Length; ++i)
        //            {
        //                fieldName[i] = distinctRows[i]["FieldName"].ToString();
        //                valueType[i] = distinctRows[i]["ValueType"].ToString();
        //                otherInfoValue[i] = distinctRows[i]["OtherInfoValue"].ToString();
        //                Trace.Warn(" FieldName = " + fieldName[i] + " ValueType = " + valueType[i] + " otherInfoValue = " + otherInfoValue[i]);
        //            }

        //            //lblbikeNames.Text = string.Empty;
        //            string bikes = string.Empty;
        //            string modelNames = string.Empty;
        //            string makeNames = string.Empty;
        //            string versionNames = string.Empty;
        //            string otherInfo = string.Empty;
        //            string makeMasks = string.Empty;
        //            string modelMasks= string.Empty;

        //            for (int i = 0; i < fieldName.Length; ++i)
        //            {
        //                otherInfo += "<b>" + fieldName[i] + "</b>" + ": " + otherInfoValue[i] + "<br />";
        //            }
        //            Trace.Warn("models: " + models.Length.ToString());
        //            Trace.Warn("bikeNames.Length: " + bikeNames.Length.ToString());

        //            for (int i = 0; i < bikeNames.Length; ++i)
        //            {
        //                if (i == (bikeNames.Length - 1))
        //                {
        //                    Trace.Warn("i: " + i.ToString());
        //                    if (bikeNames.Length > 1)
        //                    {
        //                        if (bikes.Substring(bikes.Length - 2).IndexOf(",") >= 0)
        //                        {
        //                            //lblbikeNames.Text = lblbikeNames.Text.Substring(0, lblbikeNames.Text.Length - 2) + " and <a href=\"/research/" + makes[i].ToLower().Replace(" ", string.Empty) + "-bikes/" + models[i].ToLower().Replace(" ", string.Empty) + "/\">" + bikeNames[i] + "</a>";
        //                            bikes = bikes.Substring(0, bikes.Length - 2) + " and " + bikeNames[i];
        //                        }
        //                        else
        //                        {
        //                            bikes += "and " + bikeNames[i];
        //                        }
        //                    }
        //                    else
        //                    {
        //                        bikes = bikeNames[i];
        //                    }

        //                    Trace.Warn("bikesa: " + bikes);
        //                    //modelNames += models[i];
        //                }
        //                else
        //                {
        //                    //bikeComparePageKeywords += bikeNames[i] + " comparison, ";
        //                    //lblbikeNames.Text += "<a href=\"/research/" + makes[i].ToLoweor().Replace(" ", string.Empty) + "-bikes/" + models[i].ToLower().Replace(" ", string.Empty) + "/\">" + bikeNames[i] + "</a>, ";
        //                    bikes += bikeNames[i] + ", ";
        //                    //modelNames += models[i] + " vs ";
        //                }
        //                versionNames += versions[i];
        //                modelNames += models[i];
        //                makeNames += makes[i];
        //                versionId += versionIds[i];
        //                makeMasks += MakeMaskingNames[i];
        //                modelMasks += ModelMaskingNames[i];
        //            }

        //            RoadTestPageKeywords = modelNames + ", road test, road tests, roadtests, roadtest, bike reviews, expert bike reviews, detailed bike reviews, test-drives, comprehensive bike tests, bike preview, first drives";

        //            Trace.Warn("Here:");
        //            //lblHeading.Text = makeNames + " " + modelNames;//title;
        //            //Url = UrlRewrite.FormatSpecial(makeNames) + "-bikes/" + UrlRewrite.FormatSpecial(modelNames) + "/road-tests";
        //            lblAuthor.Text = authorName;
        //            lblDate.Text = displayDate;
        //            Trace.Warn("Here2");
        //            if (pageid == "1")
        //            {
        //                //ArticleTitle = "Road Test: " + title;                    
        //                lblDetails.Text = pageData[0];
        //                //divOtherInfo.InnerHtml = otherInfo;
        //            }
        //            else
        //            {
        //                if (int.Parse(pageid) <= pageData.Length)
        //                {
        //                    //ArticleTitle = pageNames[Convert.ToInt32(pageid) - 1] + ": " + title;
        //                    lblDetails.Text = pageData[Convert.ToInt32(pageid) - 1];
        //                }
        //                else
        //                {
        //                    Trace.Warn("pageId: " + pageid);
        //                    Trace.Warn("str: " + str);
        //                    //ArticleTitle = "Photos: " + title;
        //                }
        //            }

        //            //ArticleHeading = (subCategory == string.Empty ? "Road Test" : subCategory) + ": " + makeNames + " " + modelNames;
        //            ArticleHeading = (subCategory == string.Empty ? "Road Test" : subCategory) + ": " + title;
        //            ArticleTitle = title;
        //            MakeName = makeNames;
        //            ModelName = modelNames;
        //            VersionName = versionNames;
        //            MakeMaskName = makeMasks;
        //            ModelMaskName = modelMasks;
                    

        //            Trace.Warn("VersionName: " + VersionName);
        //            Trace.Warn("VersionId:" + versionId);
        //            Trace.Warn("Here3");
        //            RoadTestPageDesc = "BikeWale tests " + bikes + ". Read the complete road test report to know how it performed.";
        //            RoadTestPageTitle = ArticleHeading;
        //            Trace.Warn("Here4");
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn("Error = " + err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}

        //protected string GetImagePath(string articleId, string photoId, string imgSize, string hostUrl)
        //{
        //    return Bikewale.Common.ImagingFunctions.GetImagePath("/ec/", hostUrl) + articleId + "/img/" + imgSize + "/" + photoId + ".jpg";
        //}

        //public string RoadTestPageTitle
        //{
        //    get
        //    {
        //        if (ViewState["RoadTestPageTitle"] != null)
        //            return ViewState["RoadTestPageTitle"].ToString();
        //        else
        //            return "";
        //    }
        //    set { ViewState["RoadTestPageTitle"] = value; }
        //}

        //public string RoadTestPageDesc
        //{
        //    get
        //    {
        //        if (ViewState["RoadTestPageDesc"] != null)
        //            return ViewState["RoadTestPageDesc"].ToString();
        //        else
        //            return "";
        //    }
        //    set { ViewState["RoadTestPageDesc"] = value; }
        //}

        //public string RoadTestPageKeywords
        //{
        //    get
        //    {
        //        if (ViewState["RoadTestPageKeywords"] != null)
        //            return ViewState["RoadTestPageKeywords"].ToString();
        //        else
        //            return "";
        //    }
        //    set { ViewState["RoadTestPageKeywords"] = value; }
        //}

        //private string _url = string.Empty;
        //public string Url
        //{
        //    get { return _url; }
        //    set { _url = value; }
        //}

        //public string oem
        //{
        //    get
        //    {
        //        if (ViewState["oem"] != null)
        //            return ViewState["oem"].ToString();
        //        else
        //            return "";
        //    }
        //    set { ViewState["oem"] = value; }
        //}

        //public string bodyType
        //{
        //    get
        //    {
        //        if (ViewState["bodyType"] != null)
        //            return ViewState["bodyType"].ToString();
        //        else
        //            return "";
        //    }
        //    set { ViewState["bodyType"] = value; }
        //}

        //public string subSegment
        //{
        //    get
        //    {
        //        if (ViewState["subSegment"] != null)
        //            return ViewState["subSegment"].ToString();
        //        else
        //            return "";
        //    }
        //    set { ViewState["subSegment"] = value; }
        //}
    }
}