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

namespace Bikewale.Content
{
    public class ViewCT : System.Web.UI.Page
    {
        protected Label lblHeading, /*lblTitle,*/ lblAuthor, lblDate, lblDetails, /*lblMethodologies,*/ lblphoto, lblphoto1, lblCarNames;
        protected DropDownList drpPages, drpPages_footer;
        protected Repeater rptPages, rptPages_footer;
        protected DataList dlstPhoto;
        protected HtmlGenericControl topNav, bottomNav;
        protected string threadId = string.Empty;
        protected string Priority = string.Empty;
        protected string ArticleTitle = string.Empty;
        protected string str = string.Empty;
        protected int strCount;
        public string id = string.Empty, pageid = "1", articleType = string.Empty, type = string.Empty;	// number of links in the strip
        protected string modelId = string.Empty, versionId = string.Empty;
        protected bool ShowGallery = false;
        protected bool IsPhotoGalleryPage = false;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
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
                pageid = Request.QueryString["pageId"].ToString();

                if (CommonOpn.CheckId(pageid) == false)
                {
                    return;
                }
            }

            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "" && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                id = Request.QueryString["id"].ToString();

                /** Modified By : Ashwini Todkar on 12 Aug 2014 , add when consuming carwale api
               //Check if basic id exists in mapped carwale basic id log **/

                string _basicId = BasicIdMapping.GetCWBasicId(id);

                //if id exists then redirect url to new basic id url
                if (!String.IsNullOrEmpty(_basicId))
                {
                    string _newUrl = "/comparos/"  + Request["t"] + "-" + _basicId + ".html";
                    //Trace.Warn("new Url : " + newUrl );
                    CommonOpn.RedirectPermanent(_newUrl);
                }
            }
            else
            {
                id = "0";
                //Response.Redirect("default.aspx");
            }

            GetPageData();

            if (!IsPostBack)
            {
                FillPages();
                GoogleKeywords();
                drpPages.SelectedValue = pageid;//drpPages.Items.IndexOf(drpPages.Items.FindByValue( pageid ));
                drpPages_footer.SelectedValue = pageid;//drpPages.Items.IndexOf(drpPages.Items.FindByValue( pageid ));
            }
        }

        void CheckPage()
        {
            if (pageid == str)
            {
                Trace.Warn("CheckPage() str: " + str);
                IsPhotoGalleryPage = true;
                LoadPhotos();
            }
            else if (pageid != str)
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

            //SqlParameter[] param = { new SqlParameter("@BasicId", id) };

            //// Data to fill all the details on the page

            //sql = " Select B.Title, B.AuthorName, B.Url, B.HostURL, B.DisplayDate, CMA.Name As Make, CMO.Name As Model, CV.Name As Version, C.ModelId, C.VersionId, "
            //    + "		Case When C.VersionId = -1 Then CMA.Name + ' ' + CMO.Name  Else CMA.Name + ' ' + CMO.Name + ' ' + CV.Name End As Car, CA.Name As Category, CF.FieldName, CF.ValueType, "
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
            //    Cache.Insert("dsCarCompDataCache", dsPageData, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);
            //}
            //catch (Exception ex)
            //{
            //    Trace.Warn(ex.Message);
            //}

            //return dsPageData;
        }

        private void LoadPhotos()
        {
            string sql = " SELECT * FROM Con_EditCms_Images CI With(NoLock) "
                        + " Inner Join Con_PhotoCategory CP With(NoLock) On CP.Id = CI.ImageCategoryId "
                        + " WHERE BasicId = @BasicId AND IsActive = 1 ORDER BY Sequence ASC";
            try
            {
                CommonOpn op = new CommonOpn();
                SqlParameter[] param = { new SqlParameter("@BasicId", id) };
                op.BindListReader(sql, dlstPhoto, param);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void drpPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Trace.Warn("/research/comparos/" + Url + "-" + id + "/p" + drpPages.SelectedItem.Value + "/");
            Response.Redirect("/research/comparos/" + Url + "-" + id + "/p" + drpPages.SelectedItem.Value + "/");
        }

        void drpPages_footer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Redirect("view.aspx?id="+ id +"&pageId=" + drpPages_footer.SelectedItem.Value);			
            Response.Redirect("/research/comparos/" + Url + "-" + id + "/p" + drpPages_footer.SelectedItem.Value + "/");
        }

        protected string CreateNavigationLink(string _priority, string url)
        {
            if (_priority == pageid)
            {
                return "<acronym><b>" + _priority + "</b></acronym>";
            }
            else
            {
                return "<a href=\"/research/comparos/" + url + "-" + id + "/p" + _priority + "/\">" + _priority + "</a>";
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

            //    if ((DataSet)Cache.Get("dsCarCompDataCache") != null)
            //    {
            //        ds = (DataSet)Cache.Get("dsCarCompDataCache");
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

            //    strCount = drpPages.Items.Count;
            //    str = (strCount + 1).ToString();

            //    SqlParameter[] param = { new SqlParameter("@BasicId", id) };

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
            //        drpPages.Items.Insert(strCount, new ListItem((strCount + 1).ToString() + ". Photos", str));
            //        drpPages_footer.Items.Insert(strCount, new ListItem((strCount + 1).ToString() + ". Photos", str));

            //        drpPages.SelectedValue = pageid;
            //        drpPages_footer.SelectedValue = pageid;

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
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn("Error: " + err.Message);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
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
            string[] carNames, makes, models, versions, modelIds, versionIds;
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
                //Check if data is already present in the Cache.
                //if ((DataSet)Cache.Get("dsCarCompDataCache") != null)
                //{
                //    ds = (DataSet)Cache.Get("dsCarCompDataCache");
                //}
                //else
                //{
                //    ds = LoadSqlDataToCache();
                //}
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
                }

                distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "Bike", "Make", "Model", "Version", "ModelId", "VersionId" });
                distinctRows = distinctTable.Select();

                carNames = new string[distinctRows.Length];
                makes = new string[distinctRows.Length];
                models = new string[distinctRows.Length];
                versions = new string[distinctRows.Length];
                modelIds = new string[distinctRows.Length];
                versionIds = new string[distinctRows.Length];

                for (int i = 0; i < distinctRows.Length; ++i)
                {
                    carNames[i] = " " + distinctRows[i]["Bike"].ToString();
                    makes[i] = " " + distinctRows[i]["Make"].ToString();
                    models[i] = " " + distinctRows[i]["Model"].ToString();
                    versions[i] = " " + distinctRows[i]["Version"].ToString();
                    modelIds[i] = " " + distinctRows[i]["ModelId"].ToString();
                    versionIds[i] = " " + distinctRows[i]["VersionId"].ToString();
                }

                distinctTable = dt.DefaultView.ToTable("DistinctTable", true, new string[] { "Title", "Url", "AuthorName", "DisplayDate", "Category" });
                distinctRows = distinctTable.Select();

                title = distinctRows[0]["Title"].ToString();
                Url = distinctRows[0]["Url"].ToString();
                authorName = distinctRows[0]["AuthorName"].ToString();
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

                lblCarNames.Text = string.Empty;
                string cars = string.Empty;
                string modelNames = string.Empty;



                for (int i = 0; i < carNames.Length; ++i)
                {
                    if (i == (carNames.Length - 1))
                    {
                        //CarComparePageKeywords += carNames[i] + " comparison";						
                        Trace.Warn("i: " + i.ToString());
                        if (cars.Substring(cars.Length - 2).IndexOf(",") >= 0)
                        {
                            lblCarNames.Text = lblCarNames.Text.Substring(0, lblCarNames.Text.Length - 2) + " and <a href=\"/research/" + makes[i].ToLower().Replace(" ", string.Empty) + "-cars/" + models[i].ToLower().Replace(" ", string.Empty) + "/\">" + carNames[i] + "</a>";
                            cars = cars.Substring(0, cars.Length - 2) + " and " + carNames[i];
                        }
                        else
                        {
                            cars += "and " + carNames[i];
                        }

                        Trace.Warn("carsa: " + cars);
                        modelNames += models[i];
                    }
                    else
                    {
                        //CarComparePageKeywords += carNames[i] + " comparison, ";
                        lblCarNames.Text += "<a href=\"/research/" + makes[i].ToLower().Replace(" ", string.Empty) + "-cars/" + models[i].ToLower().Replace(" ", string.Empty) + "/\">" + carNames[i] + "</a>, ";
                        cars += carNames[i] + ", ";
                        modelNames += models[i] + " vs ";
                    }
                }

                CarComparePageKeywords = modelNames + ", Comparo, comparison test, road test";

                Trace.Warn("Here:");
                lblHeading.Text = title;
                lblAuthor.Text = authorName;
                lblDate.Text = displayDate;
                Trace.Warn("Here2");
                if (pageid == "1")
                {
                    ArticleTitle = "Comparison Test: " + title;
                    //CarComparePageDesc = authorName + " compares " + cars + " for their looks, performance, comfort, space, fuel economy and other important driving parameters.";                    
                    lblDetails.Text = pageData[0];
                }
                else
                {

                    //CarComparePageDesc = pageNames[Convert.ToInt32(pageid)-1] + " - " + authorName + " compares " + cars + " for their looks, performance, comfort, space, fuel economy and other important driving parameters.";					
                    if (int.Parse(pageid) <= pageData.Length)
                    {
                        ArticleTitle = pageNames[Convert.ToInt32(pageid) - 1] + ": " + title;
                        lblDetails.Text = pageData[Convert.ToInt32(pageid) - 1];
                    }
                    else
                    {
                        Trace.Warn("pageId: " + pageid);
                        Trace.Warn("str: " + str);
                        ArticleTitle = "Photos: " + title;
                    }
                }
                Trace.Warn("Here3");
                CarComparePageDesc = "A comprehensive comparison road test of " + cars + ".  Read the full comparison review to know how these cars fair against each other.";
                CarComparePageTitle = ArticleTitle;
                Trace.Warn("Here4");
                //if (pageid == "1")
                //{
                //    hyplnkMethodologies.Visible = true;					
                //    lblMethodologies.Text = "<span style=\"font-style:italic;\">" + otherInfoValue[0] + "</span>";				
                //}
            }
            catch (Exception err)
            {
                Trace.Warn("Error = " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        string GetThreadId(string Id)
        {
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //SqlDataReader dr = null;
            //string sql = "";
            //string tId = "";

            //try
            //{
            //    if (type != "")
            //    {
            //        sql = " SELECT ThreadId FROM Forum_ArticleAssociation With(NoLock) "
            //            + " WHERE ArticleType = @type AND ArticleId = @Id";

            //        Trace.Warn(sql);
            //        SqlCommand cmd = new SqlCommand(sql);
            //        cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = Id;
            //        cmd.Parameters.Add("@type", SqlDbType.SmallInt).Value = type;

            //        dr = db.SelectQry(cmd);

            //        if (dr.Read())
            //        {
            //            tId = dr["ThreadId"].ToString();
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn("Error Message : " + err.Message);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
            //return tId;
        }

        protected void GoogleKeywords()
        {
            throw new Exception("Method not used/commented");

            //if (versionId == "" && modelId == "")
            //    return;

            //Trace.Warn(" versionId : " + versionId);
            //Trace.Warn(" modelId : " + modelId);

            //string sql = "";
            //SqlDataReader dr = null;

            //if (versionId != "")
            //{
            //    sql = " SELECT DISTINCT CM.Name AS Make, Se.Name AS SubSegment, Bo.Name BikeBodyStyle "
            //        + " FROM BikeModels AS CMO, BikeMakes AS CM, BikeBodyStyles Bo, "
            //        + " (BikeVersions Ve With(NoLock) LEFT JOIN BikeSubSegments Se With(NoLock) ON Se.Id = Ve.SubSegmentId ) "
            //        + " WHERE CM.ID=CMO.BikeMakeId AND CMO.ID=Ve.BikeModelId AND Bo.Id=Ve.BodyStyleId "
            //        + " AND Ve.Id = @versionId";
            //}
            //else
            //{
            //    sql = " SELECT DISTINCT CM.Name AS Make, Se.Name AS SubSegment, Bo.Name BikeBodyStyle "
            //        + " FROM BikeModels AS CMO, BikeMakes AS CM, BikeBodyStyles Bo, "
            //        + " (BikeVersions Ve With(NoLock) LEFT JOIN BikeSubSegments Se With(NoLock) ON Se.Id = Ve.SubSegmentId ) "
            //        + " WHERE CM.ID=CMO.BikeMakeId AND CMO.ID=Ve.BikeModelId AND Bo.Id=Ve.BodyStyleId "
            //        + " AND Ve.BikeModelId = @modelId";
            //}

            //Database db = new Database();
            //try
            //{
            //    SqlParameter[] param = { new SqlParameter("@modelId", modelId), new SqlParameter("@versionId", versionId) };
            //    dr = db.SelectQry(sql, param);
            //    if (dr.Read())
            //    {
            //        oem = dr["Make"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
            //        bodyType = dr["BikeBodyStyle"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
            //        subSegment = dr["SubSegment"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
            //    }

            //    Trace.Warn("sql : " + sql);
            //    Trace.Warn("oem : " + oem);
            //    Trace.Warn(" bodyType : " + bodyType);
            //    Trace.Warn(" subSegment : " + subSegment);

            //}
            //catch (Exception err)
            //{
            //    Exception ex = new Exception(err.Message + ":" + sql + " : " + id);
            //    Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
        }

        protected string GetImagePath(string articleId, string photoId, string imgSize, string hostUrl)
        {
            return ImagingFunctions.GetImagePath("/ec/", hostUrl) + articleId + "/img/" + imgSize + "/" + photoId + ".jpg";
        }

        public string CarComparePageTitle
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

        public string CarComparePageDesc
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

        public string CarComparePageKeywords
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

        private string _url = string.Empty;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
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
    }//class
}//namespace