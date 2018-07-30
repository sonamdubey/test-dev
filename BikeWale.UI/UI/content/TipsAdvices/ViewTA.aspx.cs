using Bikewale.Common;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    public class ViewTA : System.Web.UI.Page
    {
        protected DropDownList drpPages, drpPages_footer;
        protected Repeater rptPages, rptPages_footer;
        protected DataList dlstPhoto;
        protected Label lblHeading, lblAuthor, lblDate, lblDetails;
        protected HtmlGenericControl topNav, bottomNav;
        protected string Url = string.Empty, PageId = "1", BasicId = string.Empty, Str = string.Empty, ArticleTitle;
        protected bool ShowGallery = false, IsPhotoGalleryPage = false;
        protected int StrCount = 0;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            drpPages.SelectedIndexChanged += new EventHandler(drpPages_SelectedIndexChanged);
            drpPages_footer.SelectedIndexChanged += new EventHandler(drpPages_footer_SelectedIndexChanged);
        }

        //void InitializeComponent()
        //{
        //    base.Load += new EventHandler(Page_Load);

        //}

        void Page_Load(object Sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (Request["pageId"] != null && Request.QueryString["pageId"].ToString() != "")
            {
                PageId = Request.QueryString["pageId"].ToString();

                if (CommonOpn.CheckId(PageId) == false)
                {
                    return;
                }
            }

            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "" && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                BasicId = Request.QueryString["id"].ToString();
            }
            else
            {
                BasicId = "0";
                //Response.Redirect("default.aspx");
            }

            GetPageData();

            if (!IsPostBack)
            {
                FillPages();
                //GoogleKeywords();
                drpPages.SelectedValue = PageId;//drpPages.Items.IndexOf(drpPages.Items.FindByValue( pageid ));
                drpPages_footer.SelectedValue = PageId;//drpPages.Items.IndexOf(drpPages.Items.FindByValue( pageid ));
            }


        }

        void CheckPage()
        {
            if (PageId == Str)
            {
                Trace.Warn("CheckPage() str: " + Str);
                IsPhotoGalleryPage = true;
                LoadPhotos();
            }
            else if (PageId != Str)
            {
                IsPhotoGalleryPage = false;
                //GetFileDetails();
            }
        }

        DataSet LoadSqlDataToCache()
        {
            throw new Exception("Method not used/commented");

            //string sql = string.Empty;

            //Database db = new Database();
            //DataSet ds = new DataSet();
            //DataSet dsPageData = new DataSet();

            //DataTable dtPageData = new DataTable("PageData");

            //SqlParameter[] param = { new SqlParameter("@BasicId", BasicId) };

            //// Data to fill all the details on the page

            //sql = " Select B.Title, B.AuthorName, B.Url, B.Description, B.DisplayDate, CMA.Name As Make, CMO.Name As Model, CV.Name As Version, C.ModelId, C.VersionId, "
            //    + "		Case When C.VersionId = -1 Then CMA.Name + ' ' + CMO.Name  Else CMA.Name + ' ' + CMO.Name + ' ' + CV.Name End As Bike, CA.Name As Category, CF.FieldName, CF.ValueType, "
            //    + "		Case When CF.ValueType = 1 Then Cast(OI.BooleanValue As VarChar(1)) When CF.ValueType = 2 Then Cast(OI.NumericValue As VarChar(15)) When CF.ValueType = 3 Then Cast(OI.DecimalValue As VarChar) "
            //    + "		 When CF.ValueType = 4 Then OI.TextValue  When CF.ValueType = 5 Then Convert(VarChar, OI.DateTimeValue, 103) Else '' End As OtherInfoValue, ( Cast( P.Priority As VarChar(10) ) + '. ' + P.PageName ) As PageNameForDDL, P.PageName, P.Priority, PC.Data "
            //    + "	From Con_EditCms_Basic B With(NoLock) "
            //    + "		Left Join Con_EditCms_Bikes C With(NoLock) On C.BasicId = B.Id And C.IsActive = 1 "
            //    + "		Left Join Con_EditCms_OtherInfo OI With(NoLock) On OI.BasicId = B.Id "
            //    + "		Left Join Con_EditCms_CategoryFields CF With(NoLock) On CF.Id = OI.CategoryFieldId And B.CategoryId = CF.CategoryId "
            //    + "		Left Join Con_EditCms_Category Ca With(NoLock) On Ca.Id = B.CategoryId "
            //    + " 	Left Join Con_EditCms_Pages P With(NoLock) On P.BasicId = B.Id "
            //    + " 	Left Join Con_EditCms_PageContent PC With(NoLock) On PC.PageId = P.Id "
            //    + "		Left JOIN BikeMakes CMA With(NoLock) On C.MakeId = CMA.ID "
            //    + "		Left JOIN BikeModels CMO With(NoLock) On C.ModelId = CMO.ID "
            //    + "		Left JOIN BikeVersions CV With(NoLock) On C.VersionId = CV.ID "
            //    + "	Where "
            //    + "		B.ID = @BasicId "
            //    + " Order By P.Priority ";

            //Trace.Warn("sql: " + sql);

            //try
            //{
            //    dtPageData = db.SelectAdaptQry(sql, param).Tables[0].Copy();
            //    dsPageData.Tables.Add(dtPageData);
            //    Cache.Insert("dsBikeCompDataCache", dsPageData, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);
            //}
            //catch (Exception ex)
            //{
            //    Trace.Warn(ex.Message);
            //}

            //return dsPageData;
        }

        private void LoadPhotos()
        {
            throw new Exception("Method not used/commented");

            //string sql = string.Empty;
            //Database db = new Database();
            //SqlCommand cmd = new SqlCommand();
            //DataSet ds = new DataSet();

            //sql = "Select CP.Name As CategoryName, Cma.Name As MakeName, Cmo.Name As ModelName,"+
            //" CI.HostUrl, CI.ImagePathLarge, CI.ImagePathThumbnail, CI.Caption"+
            //" From Con_EditCms_Images CI With(NoLock) " +
            //" Inner Join Con_PhotoCategory CP With(NoLock) On CP.Id = CI.ImageCategoryId" +
            //" Left Join BikeModels Cmo With(NoLock) On Cmo.Id = CI.ModelId" +
            //" Left Join BikeMakes Cma With(NoLock) On Cma.Id = CI.MakeId" +   
            //" Where BasicId = @BasicId And IsActive = 1 Order By Sequence ";

            //cmd.CommandText = sql;
            //cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = BasicId;


            //Trace.Warn("LoadPhotos Sql: " + sql);

            //try
            //{
            //    ds = db.SelectAdaptQry(cmd);
            //    dlstPhoto.DataSource = ds;
            //    dlstPhoto.DataBind();
            //}
            //catch (SqlException ex)
            //{
            //    Trace.Warn("sqlex.Message: " + ex.Message);
            //    ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
            //    
            //}
            //catch (Exception ex)
            //{
            //    Trace.Warn("ex.Message: " + ex.Message);
            //    ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
            //    
            //}
            //finally
            //{
            //    db.CloseConnection();
            //    cmd.Dispose();
            //}
        }

        void drpPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Trace.Warn("/research/comparos/" + Url + "-" + id + "/p" + drpPages.SelectedItem.Value + "/");
            Response.Redirect("/tipsadvices/" + Url + "-" + BasicId + "/p" + drpPages.SelectedItem.Value + "/");
        }

        void drpPages_footer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Redirect("view.aspx?id="+ id +"&pageId=" + drpPages_footer.SelectedItem.Value);			
            Response.Redirect("/tipsadvices/" + Url + "-" + BasicId + "/p" + drpPages_footer.SelectedItem.Value + "/");
        }

        protected string CreateNavigationLink(string _priority, string url)
        {
            if (_priority == PageId)
            {
                return "<acronym><b>" + _priority + "</b></acronym>";
            }
            else
            {
                return "<a href=\"/tipsadvices/" + url + "-" + BasicId + "/p" + _priority + "/\">" + _priority + "</a>";
            }
        }

        void FillPages()
        {
            throw new Exception("Method not used/commented");

            //string sql = string.Empty;

            //Database db = new Database();
            //SqlDataReader dr = null;

            //try
            //{
            //    CommonOpn op = new CommonOpn();
            //    DataSet ds = new DataSet();

            //    if ((DataSet)Cache.Get("dsBikeCompDataCache") != null)
            //    {
            //        ds = (DataSet)Cache.Get("dsBikeCompDataCache");
            //    }
            //    else
            //    {
            //        ds = LoadSqlDataToCache();
            //    }

            //    DataTable distinctTable = ds.Tables[0].DefaultView.ToTable("DistinctTable", true, new string[] { "PageNameForDDL", "Priority" });

            //    DataRow[] distinctRows = distinctTable.Select("", "Priority ASC");

            //    DataTable dt = new DataTable();
            //    DataColumn column;
            //    DataRow row;

            //    /*Create new DataColumn, Add ColumnName and add to DataTable. 
            //    The following is done since we cannont bind DataRows to the control directly*/
            //    column = new DataColumn();
            //    column.DataType = System.Type.GetType("System.String");
            //    column.ColumnName = "PageName";
            //    column.ReadOnly = true;

            //    dt.Columns.Add(column);

            //    column = new DataColumn();
            //    column.DataType = System.Type.GetType("System.Int32");
            //    column.ColumnName = "Priority";
            //    column.ReadOnly = true;

            //    dt.Columns.Add(column);

            //    for (int i = 0; i < distinctRows.Length; ++i)
            //    {
            //        row = dt.NewRow();
            //        row["PageName"] = distinctRows[i]["PageNameForDDL"].ToString();
            //        row["Priority"] = distinctRows[i]["Priority"].ToString();
            //        dt.Rows.Add(row);
            //    }

            //    if (distinctRows.Length <= 1)
            //    {
            //        topNav.Visible = false;
            //        bottomNav.Visible = false;
            //    }

            //    // Bind the New DataTable to the control and set the Text and Value fields
            //    drpPages.DataSource = dt;
            //    drpPages.DataTextField = "PageName";
            //    drpPages.DataValueField = "Priority";
            //    drpPages.DataBind();

            //    drpPages_footer.DataSource = dt;
            //    drpPages_footer.DataTextField = "PageName";
            //    drpPages_footer.DataValueField = "Priority";
            //    drpPages_footer.DataBind();

            //    StrCount = drpPages.Items.Count;
            //    Str = (StrCount + 1).ToString();

            //    SqlParameter[] param = { new SqlParameter("@BasicId", BasicId) };

            //    sql = " Select ShowGallery From Con_EditCms_Basic With(NoLock) Where Id = @BasicId";

            //    dr = db.SelectQry(sql, param);

            //    while (dr.Read())
            //    {
            //        ShowGallery = Convert.ToBoolean(dr["ShowGallery"].ToString());
            //    }

            //    //dr.Close();

            //    if (ShowGallery)
            //    {
            //        if (distinctRows.Length >= 1)
            //        {
            //            topNav.Visible = true;
            //            bottomNav.Visible = true;
            //        }
            //        // If Photo Gallery is to be shown, then add the extra List Item
            //        drpPages.Items.Insert(StrCount, new ListItem((StrCount + 1).ToString() + ". Photos", Str));
            //        drpPages_footer.Items.Insert(StrCount, new ListItem((StrCount + 1).ToString() + ". Photos", Str));

            //        drpPages.SelectedValue = PageId;
            //        drpPages_footer.SelectedValue = PageId;

            //        CheckPage();
            //    }

            //    distinctTable = ds.Tables[0].DefaultView.ToTable("DistinctTable", true, "Priority");

            //    distinctRows = distinctTable.Select("", "Priority ASC");

            //    dt = new DataTable();

            //    column = new DataColumn();
            //    column.DataType = System.Type.GetType("System.Int32");
            //    column.ColumnName = "Priority";
            //    column.ReadOnly = true;

            //    dt.Columns.Add(column);

            //    for (int i = 0; i < distinctRows.Length; ++i)
            //    {
            //        row = dt.NewRow();
            //        row["Priority"] = distinctRows[i]["Priority"].ToString();
            //        dt.Rows.Add(row);
            //    }

            //    rptPages.DataSource = dt;
            //    rptPages.DataBind();

            //    rptPages_footer.DataSource = dt;
            //    rptPages_footer.DataBind();

            //}
            //catch (SqlException err)
            //{
            //    Trace.Warn("Error: " + err.Message);
            //    ErrorClass.LogError(err, Request.ServerVariables["URL"]);
            //    
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn("Error: " + err.Message);
            //    ErrorClass.LogError(err, Request.ServerVariables["URL"]);
            //    
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
        }

        void GetPageData()
        {
            string[] pageNames, pageData;
            string[] bikeNames, makes, models, versions, modelIds, versionIds;
            string[] fieldName, valueType, otherInfoValue;

            string title = string.Empty;
            string authorName = string.Empty;
            string displayDate = string.Empty;
            string category = string.Empty;
            string description = string.Empty;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                ds = LoadSqlDataToCache();
                dt = ds.Tables[0];

                DataTable distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "PageName", "Data" });

                DataRow[] distinctRows = distinctTable.Select();
                pageNames = new string[distinctRows.Length];
                pageData = new string[distinctRows.Length];

                for (int i = 0; i < distinctRows.Length; ++i)
                {
                    pageNames[i] = " " + distinctRows[i]["PageName"].ToString();

                    if (!IsPhotoGalleryPage)
                    {
                        pageData[i] = distinctRows[i]["Data"].ToString();
                    }


                    Trace.Warn("pageData[" + i.ToString() + "]: " + pageData[i]);
                }

                distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "Bike", "Make", "Model", "Version", "ModelId", "VersionId" });
                distinctRows = distinctTable.Select();

                bikeNames = new string[distinctRows.Length];
                makes = new string[distinctRows.Length];
                models = new string[distinctRows.Length];
                versions = new string[distinctRows.Length];
                modelIds = new string[distinctRows.Length];
                versionIds = new string[distinctRows.Length];

                for (int i = 0; i < distinctRows.Length; ++i)
                {
                    bikeNames[i] = " " + distinctRows[i]["Bike"].ToString();
                    makes[i] = " " + distinctRows[i]["Make"].ToString();
                    models[i] = " " + distinctRows[i]["Model"].ToString();
                    versions[i] = " " + distinctRows[i]["Version"].ToString();
                    modelIds[i] = " " + distinctRows[i]["ModelId"].ToString();
                    versionIds[i] = " " + distinctRows[i]["VersionId"].ToString();
                }

                distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "Title", "Url", "AuthorName", "Description", "DisplayDate", "Category" });
                distinctRows = distinctTable.Select();

                title = distinctRows[0]["Title"].ToString();
                Url = distinctRows[0]["Url"].ToString();
                authorName = distinctRows[0]["AuthorName"].ToString();
                description = distinctRows[0]["Description"].ToString();
                displayDate = Convert.ToDateTime(distinctRows[0]["DisplayDate"]).ToString("dd-MMM-yyyy");
                category = distinctRows[0]["Category"].ToString();

                distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "FieldName", "ValueType", "OtherInfoValue" });
                distinctRows = distinctTable.Select();

                fieldName = new string[distinctRows.Length];
                valueType = new string[distinctRows.Length];
                otherInfoValue = new string[distinctRows.Length];

                for (int i = 0; i < distinctRows.Length; ++i)
                {
                    fieldName[i] = distinctRows[i]["FieldName"].ToString();
                    valueType[i] = distinctRows[i]["ValueType"].ToString();
                    otherInfoValue[i] = distinctRows[i]["OtherInfoValue"].ToString();
                    Trace.Warn(" FieldName = " + fieldName[i] + " ValueType = " + valueType[i] + " otherInfoValue = " + otherInfoValue[i]);
                }

                //lblBikeNames.Text = string.Empty;
                string bikes = string.Empty;
                string modelNames = string.Empty;

                BikeComparePageKeywords = modelNames + ", bike tips, bike advices, bike how to, do it yourself, DIY, bike DIY";

                Trace.Warn("Here:");

                lblAuthor.Text = authorName;
                lblDate.Text = displayDate;
                Trace.Warn("Here2");
                Trace.Warn("PageId: " + PageId);

                if (PageId == "1")
                {
                    ArticleTitle = title;
                    //BikeComparePageDesc = authorName + " compares " + cars + " for their looks, performance, comfort, space, fuel economy and other important driving parameters.";                    
                    lblDetails.Text = pageData[0];
                }
                else
                {

                    //BikeComparePageDesc = pageNames[Convert.ToInt32(pageid)-1] + " - " + authorName + " compares " + cars + " for their looks, performance, comfort, space, fuel economy and other important driving parameters.";					
                    if (int.Parse(PageId) <= pageData.Length)
                    {
                        ArticleTitle = pageNames[Convert.ToInt32(PageId) - 1] + ": " + title;
                        lblDetails.Text = pageData[Convert.ToInt32(PageId) - 1];
                    }
                    else
                    {
                        Trace.Warn("pageId: " + PageId);
                        Trace.Warn("str: " + Str);
                        ArticleTitle = "Images: " + title;
                    }
                }
                Trace.Warn("Here3");
                BikeComparePageDesc = description;
                BikeComparePageTitle = ArticleTitle;
                Trace.Warn("Here4");
            }
            catch (Exception err)
            {
                Trace.Warn("Error = " + err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }


        protected string GetImagePath(string articleId, string photoId, string imgSize, string hostUrl)
        {
            return ImagingFunctions.GetImagePath("/ec/", hostUrl) + articleId + "/img/" + imgSize + "/" + photoId + ".jpg";
        }

        public string BikeComparePageTitle
        {
            get
            {
                if (ViewState["BikeComparePageTitle"] != null)
                    return ViewState["BikeComparePageTitle"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeComparePageTitle"] = value; }
        }

        public string BikeComparePageDesc
        {
            get
            {
                if (ViewState["BikeComparePageDesc"] != null)
                    return ViewState["BikeComparePageDesc"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeComparePageDesc"] = value; }
        }

        public string BikeComparePageKeywords
        {
            get
            {
                if (ViewState["BikeComparePageKeywords"] != null)
                    return ViewState["BikeComparePageKeywords"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeComparePageKeywords"] = value; }
        }

        public string oem
        {
            get
            {
                if (ViewState["oem"] != null)
                    return ViewState["oem"].ToString();
                else
                    return "";
            }
            set { ViewState["oem"] = value; }
        }

        public string bodyType
        {
            get
            {
                if (ViewState["bodyType"] != null)
                    return ViewState["bodyType"].ToString();
                else
                    return "";
            }
            set { ViewState["bodyType"] = value; }
        }

        public string subSegment
        {
            get
            {
                if (ViewState["subSegment"] != null)
                    return ViewState["subSegment"].ToString();
                else
                    return "";
            }
            set { ViewState["subSegment"] = value; }
        }
    }
}