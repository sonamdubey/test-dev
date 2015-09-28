using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Collections.Generic;
using Bikewale.Memcache;
using System.Text.RegularExpressions;
using Bikewale.Controls;
using Bikewale.Entities.BikeData;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.BikeData;

namespace Bikewale.New
{
    public class comparebikes : System.Web.UI.Page
    {
        protected Repeater rptCommon, rptSpecs, rptFeatures, rptColors;
        protected HtmlAnchor delComp;
        protected Literal ltrTitle;
        protected AddBikeToCompare addBike;

        DataSet ds = null;
        protected string versions = string.Empty, featuredBikeId = string.Empty, spotlightUrl = string.Empty, title = string.Empty, pageTitle = string.Empty, keyword = string.Empty, canonicalUrl = string.Empty, reWriteURL = string.Empty, targetedModels = string.Empty;
        protected int count = 0, totalComp = 5;
        public int featuredBikeIndex = 0;
        protected bool isFeatured = false;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
		
        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();
            
            if (!IsPostBack)
            {
                getVersionIdList();
                BindRepeater();
                if (count == 1)
                {
                    Response.Redirect("/comparebikes/",false);//return;	
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                ltrTitle.Text = title;
                pageTitle = title;
            }
        }
        void BindRepeater()
        {
            try
            {
                //Commented By : Sadhana 

                /*db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetComparisonDetails";

                    Trace.Warn("versionlist : ",versions);
                    cmd.Parameters.Add("@BikeVersions", SqlDbType.VarChar, 50).Value = versions;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = Configuration.GetDefaultCityId;*/


                CompareBikes cb = new CompareBikes();

                ds = cb.GetComparisonBikeListByVersion(versions);
                count = ds.Tables[0].Rows.Count;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    rptCommon.DataSource = ds.Tables[0];
                    rptCommon.DataBind();

                    rptSpecs.DataSource = ds.Tables[1];
                    rptSpecs.DataBind();

                    rptFeatures.DataSource = ds.Tables[2];
                    rptFeatures.DataBind();

                    rptColors.DataSource = ds.Tables[0];
                    rptColors.DataBind();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    title += ds.Tables[0].Rows[i]["Bike"].ToString() + " vs ";
                    keyword += ds.Tables[0].Rows[i]["Bike"].ToString() + " and ";
                    canonicalUrl += ds.Tables[0].Rows[i]["MakeMaskingName"] + "-" + ds.Tables[0].Rows[i]["ModelMaskingName"] + "-vs-";
                    Trace.Warn("Bike Name : ", title);
                    targetedModels += ds.Tables[0].Rows[i]["Model"] + ",";
                }

                if (title.Length > 2)
                {
                    title = title.Substring(0, title.Length - 3);
                    keyword = keyword.Substring(0, keyword.Length - 5);
                    canonicalUrl = canonicalUrl.Substring(0, canonicalUrl.Length - 4);
                    targetedModels = targetedModels.Substring(0, targetedModels.Length - 1).ToLower();
                }

                if (isFeatured)
                {
                    title = title.Substring(0, title.LastIndexOf(" vs "));
                    keyword = keyword.Substring(0, keyword.LastIndexOf(" and "));
                    canonicalUrl = canonicalUrl.Substring(0, canonicalUrl.LastIndexOf("-vs-"));
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
                objErr.SendMail();
            }
        }


        protected void getVersionIdList()
        {
            string QueryString = Request.QueryString.ToString();
            
            if (QueryString.Contains("bike"))
            {
                for (int i = 1; i < totalComp; i++)
                {
                    if (!String.IsNullOrEmpty(Request["bike" + i]) && CommonOpn.CheckId(Request["bike" + i]) && Request["bike" + i].ToString() != "0")
                    {
                        versions += Request["bike" + i] + ",";
                        featuredBikeIndex++;
                    }
                    else
                    {
                        Trace.Warn("QS EMPTY");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpUtility.ParseQueryString(QueryString).Get("mo")))
                {
                    string[] models = HttpUtility.ParseQueryString(QueryString).Get("mo").Split(',');
                    
                    ModelMapping objCache = new ModelMapping();

                    string _newUrl = string.Empty;
                    ModelMaskingResponse objResponse = null;

                    for (int iTmp = 0; iTmp < models.Length; iTmp++)
                    {

                        objResponse = IsMaskingNameChanged(models[iTmp].ToLower());

                        if (objResponse != null && objResponse.StatusCode == 200)
                        {
                            versions += objCache.GetTopVersionId(models[iTmp].ToLower()) + ",";
                            featuredBikeIndex++;
                        }
                        else
                        {
                            if (objResponse != null && objResponse.StatusCode == 301)
                            {
                                if (String.IsNullOrEmpty(_newUrl))
                                    _newUrl = Request.RawUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                                else
                                    _newUrl = _newUrl.Replace(models[iTmp].ToLower(), objResponse.MaskingName);
                            }
                            else 
                            {
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(_newUrl))
                    {
                        //redirect permanent to new page 
                        CommonOpn.RedirectPermanent(_newUrl);
                    }
                }
            }

            Trace.Warn("versions :: " + versions);
            Trace.Warn("featured bike index : ", featuredBikeIndex.ToString());

            if (versions.Length > 0)
            {
                versions = versions.Substring(0, versions.Length - 1);

                // Get version id of the featured bike on the basis of versions selected for comparison
                // There might be multiple featured Bikes available. But only show top 1
                string featuredBike = CompareBikes.GetFeaturedBike(versions);

                if (featuredBike != "-1")
                {
                    featuredBikeId = featuredBike;
                    //spotlightUrl = featuredBike.Split('#')[1];
                    isFeatured = true;
                }


                // If featured bike available to show.
                // Check if featured bike is already selected by the user.
                if (featuredBikeId != "")
                {
                    versions += "," + featuredBikeId;
                }
            }

            addBike.VersionId = versions;
            addBike.IsFeatured = isFeatured;
        }

        private ModelMaskingResponse  IsMaskingNameChanged(string maskingName)
        {
            ModelMaskingResponse objResponse = null;
         
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                         .RegisterType<ICacheManager, MemcacheManager>()
                         .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        ;
                var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                objResponse = objCache.GetModelMaskingResponse(maskingName);
            }

            return objResponse;
        }


        protected string ShowFormatedData(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return "--";
            }
            else
            {
                return value;
            }
        }

        public string ShowFeature(string featureValue)
        {
            string adString = "";

            if (String.IsNullOrEmpty(featureValue))
                return "--";

            switch (featureValue)
            {
                case "True":
                    adString = "<img align=\"absmiddle\" src=\"http://img.aeplcdn.com/images/icons/tick.gif\" />";
                    break;
                case "False":
                    adString = "<img align=\"absmiddle\" src=\"http://img.aeplcdn.com/images/icons/delete.ico\" />";
                    break;
                default:
                    adString = "-";
                    break;
            }
            return adString;
        }   // End of ShowFeature method



        public string GetModelRatings(string versionId)
        {
            string sql = "";

            sql = " SELECT (SELECT MaskingName FROM BikeMakes With(NoLock) WHERE ID = MO.BikeMakeId) AS MakeMaskingName, MO.ID as ModelId, MO.Name AS ModelName,MO.MaskingName AS ModelMaskingName, IsNull(MO.ReviewRate, 0) AS ModelRate, IsNull(MO.ReviewCount, 0) AS ModelTotal, "
                + " IsNull(CV.ReviewRate, 0) AS VersionRate, IsNull(CV.ReviewCount, 0) AS VersionTotal "
                + " FROM BikeModels AS MO, BikeVersions AS CV With(NoLock) WHERE CV.ID = @ID AND MO.ID = CV.BikeModelId ";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = versionId;

            SqlDataReader dr = null;
            Database db = new Database();

            string reviewString = "";

            try
            {
                dr = db.SelectQry(cmd);

                while (dr.Read())
                {
                    if (Convert.ToDouble(dr["ModelRate"]) > 0)
                    {
                        string reviews = Convert.ToDouble(dr["ModelTotal"]) > 1 ? " reviews" : " review";
                        //reviewString += "<div align='center'>" + CommonOpn.GetRateImage(Convert.ToDouble(dr["ModelRate"].ToString())) + "</div>"
                        //									 + " <div style='margin-top:10px;' align='center'><a href='/Research/ReadUserReviews-Bikem-"+ dr["ModelId"].ToString() +".html'>"+ dr["ModelTotal"].ToString() + reviews +" </a></div>";
                        reviewString += "<div class='margin-top10'>" + CommonOpn.GetRateImage(Convert.ToDouble(dr["ModelRate"].ToString()))
                                     + " <a style='border-left:1px solid #E2E2E2;' class='margin-left5' href='/" + dr["MakeMaskingName"].ToString() + "-bikes/" + dr["ModelMaskingName"].ToString() + "/user-reviews/'>" + dr["ModelTotal"].ToString() + reviews + " </a></div>";

                    }
                    else
                        reviewString = "<div style='margin-top:10px;'><a href='/content/userreviews/writereviews.aspx?bikem=" + dr["ModelId"].ToString() + "'>Write a review</a></div>";
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }
            return reviewString;
        }

        protected string GetModelColors(string versionId, int index)
        {
            string colorString = String.Empty;

            DataView dv = ds.DefaultViewManager.CreateDataView(ds.Tables[3]);
            dv.Sort = "BikeVersionId";
            DataRowView[] drv = dv.FindRows(versionId);

            if (drv.Length > 0)
            {
                Trace.Warn("drv data .............", drv.Length.ToString());

                for (int jTmp = 0; jTmp < drv.Length; jTmp++)
                {
                    colorString += "<div style='width:100; text-align:center;padding:5px;'> "
                                + " <div style='border:1px solid #dddddd;width:50px;margin:auto;background-color:#" + drv[jTmp].Row["HexCode"].ToString() + "'>"
                                + " <img src='http://img.aeplcdn.com/images/spacer.gif' width='50' height='45' /></div> "
                                + " <div style='padding-top:3px;'>" + drv[jTmp].Row["Color"].ToString() + "</div></div> ";
                }
            }
            return colorString;

        }


        ///// <summary>
        ///// Created By : Ashish G. Kamble on 13 Mar 2014
        ///// Summary : Function to transpose the datatable.
        ///// </summary>
        ///// <param name="inputTable">DataTable to be transposed.</param>
        ///// <returns>Returns new datatable which is transpose of the input table.</returns>
        //private DataTable GenerateTransposedTable(DataTable inputTable)
        //{
        //    DataTable outputTable = new DataTable();

        //    // Add columns by looping rows

        //    for (int iTmp = 1; iTmp <= inputTable.Rows.Count; iTmp++)
        //    {
        //        outputTable.Columns.Add("Version" + iTmp);                
        //    }

        //    // Add rows by looping columns
        //    for (int rCount = 0; rCount < inputTable.Columns.Count; rCount++)
        //    {
        //        DataRow newRow = outputTable.NewRow();

        //        for (int cCount = 0; cCount < inputTable.Rows.Count; cCount++)
        //        {
        //            string colValue = inputTable.Rows[cCount][rCount].ToString();
        //            newRow[cCount] = colValue;
        //        }
        //        outputTable.Rows.Add(newRow);
        //    }

        //    return outputTable;
        //}   // End of GenerateTransposedTable

    }
}