using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Controls;
using Bikewale.Memcache;
using Bikewale.Entities.PhotoGallery;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Bikewale.BAL.PhotoGallery;
using Bikewale.Interfaces.PhotoGallery;
using Bikewale.Entities.CMS;
using Bikewale.BAL.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using System.Configuration;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Cache.BikeData;

namespace Bikewale.New
{
    /// <summary>
    ///     Written By : Ashish G. Kamble on 7/1/2012
    /// </summary>
    public class Versions_old : Page
    {
        protected Repeater rptVersions;

        protected HtmlGenericControl divModelDesc, divFuturistic,modelStatus,reviewedModelStatus, divVersions, div_description, div_BikeRatings,div_BikeRatingsNotAvail, divSpecs;
        protected HtmlSelect ddlVersion;

        protected NewsMin newsMin;
        protected UserReviewsMin ucUserReviewsMin;
        protected RoadTest ucRoadTestMin;
        protected BikeVideos ucBikeVideos;
        protected SimilarBikes ctrl_similarBikes;
        protected BikeRatings ctrl_BikeRatings;
        protected BikeBookingMinWidget ctrlBikeBooking;

        protected MakeModelVersion mmv;
        protected string modelId, usedBikeCount = string.Empty, seriesId = string.Empty, reviewLink = string.Empty;
        protected string makeName = string.Empty, modelName = string.Empty, expectedLaunchId = string.Empty, expectedLaunch = string.Empty, estimatedPrice = string.Empty,
                        smallDescription = string.Empty, largeDescription = string.Empty, hostURL = string.Empty, imagePath = string.Empty;
        public string make = "", model = "", previousUrl = "", makeId = string.Empty, modelOnly = "", ModelMaskingName = "", logoUrl = "", fbLogoUrl = string.Empty, MakeMaskingName = string.Empty, seriesName = String.Empty, seriesMaskingName = String.Empty,version = String.Empty,formattedPrice = string.Empty;
        protected int reviewCount = 0;
        public int modelCount = 0, Count = 0, isModel = 0, versionCount = 0;
        public bool isFuturistic = false, isNew = false, isUsed = false;
        protected bool isPhotoAvailable = false;

        public string versionId = string.Empty, imageUrl = string.Empty, bike = string.Empty, versionIdSpecs = string.Empty;

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
            divFuturistic.Visible = false;
            div_BikeRatings.Visible = true;
            div_BikeRatingsNotAvail.Visible = true;

            if (!IsPostBack)
            {
                if (ProcessQueryString())
                {
                    DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                    dd.DetectDevice();

                    try
                    {
                        if (!String.IsNullOrEmpty(modelId))
                        {
                            GetMakeName();

                            mmv = new MakeModelVersion();
                            mmv = GetModel(mmv);
                            reviewCount = GetReviewCount(Convert.ToInt32(mmv.ModelId));
                            GetSynopsis(mmv.ModelId);
                            reviewLink = "/content/userreviews/writereviews.aspx?bikem=" + mmv.ModelId;
                            usedBikeCount = Bikewale.Memcache.Classified.GetMakeWiseUsedBikeCount(makeId);

                            isFuturistic = mmv.IsFuturistic;
                            isNew = mmv.IsNew;
                            isUsed = mmv.IsUsed;
                            
                            //Modified By : Ashwini Todkar on 20 Jan 2015
                            if (!mmv.IsFuturistic)
                            {
                                //upcoming.Attributes.Add("class", "hide");

                                if (!isNew)
                                {
                                    modelStatus.InnerText = "(Discontinued)";
                                    reviewedModelStatus.InnerText = "(Discontinued)";
                                    //discontinued.Attributes.Add("class", "hide");
                              
                                }

                                ucRoadTestMin.ModelId = modelId;
                                ucRoadTestMin.HeaderText = model + " Road Tests/First Drives";
                                ctrl_BikeRatings.ModelId = modelId;
                                ucUserReviewsMin.ModelId = mmv.ModelId;

                                GetVersions();

                                if (!String.IsNullOrEmpty(seriesId))
                                    GetSeriesDetails();

                                ctrl_similarBikes.VersionId = versionId;
                                ctrl_similarBikes.IsNew = isNew;
                                Trace.Warn("Version Id : " + versionId + "isNew:" + isNew + " " + "isUsed:" + isUsed);
                            }
                            else
                            {
                               // discontinued.Attributes.Add("class", "hide");
                                modelStatus.InnerText = "(Upcoming)";
                                UpcomingModel(mmv.ModelId);
                                divVersions.Attributes.Add("class", "hide");
                                divSpecs.Attributes.Add("class", "hide");
                                //divFuturistic.Attributes.Add("class", "grid_5 omega margin-top15 show");
                                divFuturistic.Visible = true;
                                div_BikeRatings.Visible = false;
                                div_BikeRatingsNotAvail.Visible = false;
                                ucUserReviewsMin.Attributes.Add("class", "hide");
                            }

                            ucBikeVideos.ModelId = mmv.ModelId;
                            newsMin.ModelId = mmv.ModelId;
                            ctrlBikeBooking.ModelId = mmv.ModelId;
                            ctrlBikeBooking.Model = mmv.Make + " " + mmv.Model;

                            isPhotoExists();
                        }

                    }
                    catch (Exception err)
                    {
                        Trace.Warn(err.Message);
                        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                        objErr.SendMail();
                    
                    }
                }
            }
        }

        private int GetReviewCount(int modelId)
        {
            BikeModelEntity objModelEntity = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                //Get Model details
                objModelEntity = objModel.GetById(modelId);
            }

            return objModelEntity.ReviewCount;
        }

        private void isPhotoExists()
        {
            //Commented by : Ashwini Todkar on 1 Dec 2014

             //List<ModelPhotoEntity> objPhotosList = null;

             //using (IUnityContainer container = new UnityContainer())
             //{
             //    container.RegisterType<IModelPhotos<ModelPhotoEntity, int>, ModelPhotos<ModelPhotoEntity, int>>();
             //    IModelPhotos<ModelPhotoEntity, int> objPhotos = container.Resolve<IModelPhotos<ModelPhotoEntity, int>>();

             //    List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
             //    categorList.Add(EnumCMSContentType.RoadTest);
             //    categorList.Add(EnumCMSContentType.PhotoGalleries);
             //    categorList.Add(EnumCMSContentType.ComparisonTests);

             //    objPhotosList = new List<ModelPhotoEntity>();
             //    objPhotosList = objPhotos.GetModelPhotosList(Convert.ToInt32(modelId), categorList);

             //    if (objPhotosList.Count > 0)
             //        isPhotoAvailable =  true;
             //    else
             //        isPhotoAvailable = false;
             //}

            try
            {
                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.PhotoGalleries);
                categorList.Add(EnumCMSContentType.ComparisonTests);

                //sets the base URI for HTTP requests
                string contentTypeList = CommonOpn.GetContentTypesString(categorList);
                
                string _applicationid = Utility.BWConfiguration.Instance.ApplicationId;

                string _apiUrl = "webapi/image/modelphotolist/?applicationid=" + _applicationid + "&modelid=" + modelId + "&categoryidlist=" + contentTypeList;

                List<ModelImage> _objImageList = null;

                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objImageList = objClient.GetApiResponseSync<List<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objImageList);
                }                

                if (_objImageList != null && _objImageList.Count > 0)
                    isPhotoAvailable = true;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }

        private bool ProcessQueryString()
        {
            bool isSuccess = true;

            //Modified By : Ashwini Todkar on 19 Jan 2015

            if (!string.IsNullOrEmpty(Request.QueryString["model"]))
            {
                ModelMaskingResponse objResponse = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                    objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);

                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        modelId = objResponse.ModelId.ToString();
                    }
                    else
                    {
                        if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["model"], objResponse.MaskingName));
                            
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            isSuccess = false;
                        }
                    }
                }
            }
            else
            {
                //Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //this.Page.Visible = false;
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        ///  Function to get the make name and model name for the current model id
        /// </summary>
        protected void GetMakeName()
        {
            Database db = null;
            DataSet ds = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " select DISTINCT M.Name AS Make, BM.Name AS Model "
                                    + " from BikeModels AS BM "
                                    + " LEFT JOIN BikeMakes AS M ON M.ID = BM.BikeMakeId "
                                    + " where BM.ID = " + modelId;

                    ds = db.SelectAdaptQry(cmd);

                    if (ds != null)
                    {
                        makeName = ds.Tables[0].Rows[0]["Make"].ToString();
                        modelName = ds.Tables[0].Rows[0]["Model"].ToString();
                    }
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.versions.LoadMakes");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "new.versions.LoadMakes");
                objErr.SendMail();
                Trace.Warn("err makes : ",err.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        ///     Function to get versions list
        /// </summary>
        protected void GetVersions()
        {
            Database db = null;
            DataSet ds = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetVersions";

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;                    
                    cmd.Parameters.Add("@New", SqlDbType.Bit).Value = isNew;

                    Trace.Warn("MODELID : "+modelId);
                    ds = db.SelectAdaptQry(cmd);
                }
                if (ds != null)
                {
                    Count = ds.Tables[0].Rows.Count;
                    Trace.Warn("row count,,", ds.Tables[0].Rows.Count.ToString());
                    if (Count > 0)
                    {
                        rptVersions.DataSource = ds;
                        rptVersions.DataBind();

                        versionId = ds.Tables[0].Rows[0]["ID"].ToString();
                        version = ds.Tables[0].Rows[0]["Version"].ToString();

                        BindVersionDDL();
                    }
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("versions sql ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("versions ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }   // End of GetVersions method

        /// <summary>
        /// Written By : Ashwini Todkar on 18 march 2014
        /// Summary    : method to get series name ,masking name and number of models in a series
        /// </summary>
        protected void GetSeriesDetails()
        {

            SqlDataReader dr = null;
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetSeriesDetails";
                    cmd.Parameters.Add("@BikeSeriesId", SqlDbType.Int).Value = seriesId;

                    db = new Database();

                    dr = db.SelectQry(cmd);

                    if (dr.Read())
                    {
                        seriesName = dr["Name"].ToString();
                        seriesMaskingName = dr["SeriesMaskingName"].ToString();
                        modelCount = (int)dr["ModelCount"];
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
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
        }  

        /// <summary>
        ///     If price is not available show N/A else show original price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public string GetMinPrice(string price)
        {
            if (price == "")
                return "N/A";
            else
                return "Rs. " + CommonOpn.FormatNumeric(price);
        }

        /// <summary>
        ///     Function to get all the details of the current model
        /// </summary>
        /// <param name="mmv"></param>
        /// <returns></returns>
        MakeModelVersion GetModel(MakeModelVersion mmv)
        {
            mmv.GetModelDetails(modelId.ToString());

            //imageUrl = "/bikewaleimg/models/" + mmv.LargePic;
            imageUrl = mmv.OriginalImagePath;
            hostURL = mmv.HostUrl;
            //imagePath = MakeModelVersion.GetModelImage(hostURL, imageUrl);
            imagePath = MakeModelVersion.GetModelImage(hostURL, imageUrl, "210x118");
            
            estimatedPrice = CommonOpn.FormatPrice(mmv.MinPrice, mmv.MaxPrice);

            make = mmv.Make;
            model = make + " " + mmv.Model;
            modelOnly = mmv.Model;
            ModelMaskingName = mmv.ModelMappingName;
            MakeMaskingName = mmv.MakeMappingName;
            seriesId = mmv.SeriesId;
            
            //logoUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + mmv.LargePic, mmv.HostUrl);
            logoUrl = Bikewale.Utility.Image.GetPathToShowImages(mmv.OriginalImagePath, mmv.HostUrl, Bikewale.Utility.ImageSize._210x118);
            
            if (string.IsNullOrEmpty(mmv.LargePic))
                fbLogoUrl = "";
            else
                fbLogoUrl = logoUrl;

            hostURL = mmv.HostUrl;
            
            //imagePath = "/bikewaleimg/models/" + mmv.LargePic;
            imagePath = mmv.OriginalImagePath;
            estimatedPrice = String.IsNullOrEmpty(mmv.MinPrice) ? " - N/A" : CommonOpn.FormatNumeric(mmv.MinPrice);

            formattedPrice = String.IsNullOrEmpty(mmv.MinPrice) ? "" : CommonOpn.FormatNumeric(mmv.MinPrice);

            if (mmv.MinPrice != mmv.MaxPrice)
                formattedPrice += String.IsNullOrEmpty(formattedPrice) ? CommonOpn.FormatNumeric(mmv.MaxPrice) : " - " + CommonOpn.FormatNumeric(mmv.MaxPrice);

            formattedPrice = String.IsNullOrEmpty(formattedPrice) ? "N/A" :  formattedPrice;

            makeId = mmv.MakeId;
            return mmv;
        }

        /// <summary>
        ///     Get Upcoming models details
        /// </summary>
        /// <param name="modelId"></param>
        void UpcomingModel(string modelId)
        {
            string sql = "";
            sql = " SELECT ECL.ID, ECL.ExpectedLaunch, ECL.EstimatedPriceMin, ECL.EstimatedPriceMax, ECL.HostURL, ECL.LargePicImagePath, ECL.OriginalImagePath "	            
                + " FROM ExpectedBikeLaunches AS ECL "
                + " LEFT JOIN BikeSynopsis Csy ON ECL.BikeModelId = Csy.ModelId AND Csy.IsActive = 1 "
                + " WHERE ECL.BikeModelId=@ModelId";

            Trace.Warn("upcoming sql : " + sql);

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;

            SqlDataReader dr = null;
            Database db = new Database();

            try
            {
                dr = db.SelectQry(cmd);

                if (dr.Read())
                {
                    expectedLaunchId = dr["ID"].ToString();
                    expectedLaunch = dr["ExpectedLaunch"].ToString();
                    estimatedPrice = CommonOpn.FormatPrice(dr["EstimatedPriceMin"].ToString().Replace(".00", ""), dr["EstimatedPriceMax"].ToString().Replace(".00", ""));
                    hostURL = dr["HostURL"].ToString();
                    Trace.Warn("+++",dr["HostURL"].ToString());
                    //imageUrl = dr["LargePicImagePath"].ToString();
                    imageUrl = dr["OriginalImagePath"].ToString();

                    //logoUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(dr["LargePicImagePath"].ToString(), dr["HostURL"].ToString());
                    logoUrl = Bikewale.Utility.Image.GetPathToShowImages(dr["OriginalImagePath"].ToString(), dr["HostURL"].ToString(), Bikewale.Utility.ImageSize._210x118);

                    //if (string.IsNullOrEmpty(dr["LargePicImagePath"].ToString()))
                    if (string.IsNullOrEmpty(dr["OriginalImagePath"].ToString()))
                        fbLogoUrl = "";
                    else
                        fbLogoUrl = logoUrl;

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
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 21 June 2013
        ///     Summary : Get bike synopsis if any added.
        /// </summary>
        /// <param name="modelId"></param>
        void GetSynopsis(string modelId)
        {
            string sql = "";
            sql = " SELECT Csy.SmallDescription, Csy.FullDescription "
                + " FROM BikeModels AS Mo "
                + " LEFT JOIN BikeSynopsis Csy ON Mo.ID = Csy.ModelId AND Csy.IsActive = 1 "
                + " WHERE Mo.ID = @ModelId";

            Trace.Warn("Get Bike synopsis sql : " + sql);

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;

            SqlDataReader dr = null;
            Database db = new Database();

            try
            {
                dr = db.SelectQry(cmd);

                if (dr.Read())
                {                    
                    smallDescription = dr["SmallDescription"].ToString();
                    largeDescription = dr["FullDescription"].ToString();
                }

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
                {
                    dr.Close();
                }
                db.CloseConnection();
            }
        }   // End of GetSynopsis method

        protected string GetBikeSpecsMin(string displacement, string fuelType, string transmission, string fuelEconomy)
        {
            string specsMin = string.Empty;
            
            if (!String.IsNullOrEmpty(displacement))
            {
                specsMin += displacement + "cc";                
            }

            if (!String.IsNullOrEmpty(fuelType))
            {
                specsMin += ", " + GetFuelType(fuelType);
            }

            if (!String.IsNullOrEmpty(transmission))
            {
                specsMin += ", " + GetTransmissionType(transmission);
            }

            if (!String.IsNullOrEmpty(fuelEconomy))
            {
                specsMin += ", " + fuelEconomy + " kmpl*";
            }
            //If comma is the first character then remove it.
            if (specsMin.IndexOf(',') == 0)
            {
                specsMin = specsMin.Substring(1).Trim();
            }

            return specsMin;
        }   //End of GetBikeSpecsMin function

        /// <summary>
        ///     Function to get the Fuel type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetFuelType(string type)
        {
            string fuel = string.Empty;

            switch (type)
            {
                case "1": fuel = "Petrol";
                    break;
                case "2": fuel = "Diesel";
                    break;
                case "5": fuel = "Electric";
                    break;
            }

            return fuel;
        }   //End of GetfuelType function

        /// <summary>
        ///     Funtion to get transmission type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetTransmissionType(string type)
        { 
            string transmission = string.Empty;

            switch (type)
            {
                case "1": transmission = "Automatic";
                    break;
                case "2": transmission = "Manual";
                    break;
            }

            return transmission;
        }   // End of GetTransmissionType function


        /// <summary>
        ///     This function will get bike specifications for the verion selected
        /// </summary>
        //protected void GetBikeSpecs()
        //{
        //    Database db = null;
        //    SqlConnection conn = null;

        //    try
        //    {
        //        db = new Database();
        //        conn = new SqlConnection(db.GetConString());

        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "GetNewBikesSpecification_SP";
        //            cmd.Connection = conn;

        //            cmd.Parameters.Add("@BikeVersionId", SqlDbType.SmallInt).Value = versionId;
        //            cmd.Parameters.Add("@Displacement", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Cylinders", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@MaxPower", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@MaximumTorque", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Bore", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Stroke", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@ValvesPerCylinder", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FuelDeliverySystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FuelType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Ignition", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@SparkPlugsPerCylinder", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@CoolingSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@GearboxType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@NoOfGears", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@TransmissionType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Clutch", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Performance_0_60_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Performance_0_80_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Performance_0_40_m", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@TopSpeed", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Performance_60_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Performance_80_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@KerbWeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@OverallLength", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@OverallWidth", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@OverallHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Wheelbase", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@GroundClearance", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@SeatHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FuelTankCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@ReserveFuelCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FuelEfficiencyOverall", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FuelEfficiencyRange", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@ChassisType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FrontSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@RearSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@BrakeType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FrontDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FrontDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@RearDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@RearDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@CalliperType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@WheelSize", SqlDbType.Float).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FrontTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@RearTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@TubelessTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@RadialTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@AlloyWheels", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@ElectricSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Battery", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@HeadlightType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@HeadlightBulbType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Brake_Tail_Light", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@TurnSignal", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@PassLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Speedometer", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Tachometer", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@TachometerType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@ShiftLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@ElectricStart", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Tripmeter", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@NoOfTripmeters", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@TripmeterType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@LowFuelIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@LowOilIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@LowBatteryIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@FuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@DigitalFuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@PillionSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@PillionFootrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@PillionBackrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@PillionGrabrail", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@StandAlarm", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@SteppedSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@AntilockBrakingSystem", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Killswitch", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Clock", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Colors", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

        //            conn.Open();
        //            cmd.ExecuteNonQuery();

        //            ShowSpecs(cmd);

        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Trace.Warn("GetBikeSpecs SqlEX: " + ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.Warn("GetBikeSpecs EX: " + ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //        db.CloseConnection();
        //    }
        //}   // end of GetBikeSpecs function

        //protected void ShowSpecs(SqlCommand sqlCmd)
        //{
        //    ltr_Displacement.Text = ShowNotAvailable(sqlCmd.Parameters["@Displacement"].Value.ToString());
        //    ltr_Cylinders.Text = ShowNotAvailable(sqlCmd.Parameters["@Cylinders"].Value.ToString());
        //    ltr_MaxPower.Text = ShowNotAvailable(sqlCmd.Parameters["@MaxPower"].Value.ToString());
        //    ltr_MaximumTorque.Text = ShowNotAvailable(sqlCmd.Parameters["@MaximumTorque"].Value.ToString());
        //    ltr_Bore.Text = ShowNotAvailable(sqlCmd.Parameters["@Bore"].Value.ToString());
        //    ltr_Stroke.Text = ShowNotAvailable(sqlCmd.Parameters["@Stroke"].Value.ToString());
        //    ltr_ValvesPerCylinder.Text = ShowNotAvailable(sqlCmd.Parameters["@ValvesPerCylinder"].Value.ToString());
        //    ltr_FuelDeliverySystem.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelDeliverySystem"].Value.ToString());
        //    ltr_FuelType.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelType"].Value.ToString());
        //    ltr_Ignition.Text = ShowNotAvailable(sqlCmd.Parameters["@Ignition"].Value.ToString());
        //    ltr_SparkPlugsPerCylinder.Text = ShowNotAvailable(sqlCmd.Parameters["@SparkPlugsPerCylinder"].Value.ToString());
        //    ltr_CoolingSystem.Text = ShowNotAvailable(sqlCmd.Parameters["@CoolingSystem"].Value.ToString());
        //    ltr_GearboxType.Text = ShowNotAvailable(sqlCmd.Parameters["@GearboxType"].Value.ToString());
        //    ltr_NoOfGears.Text = ShowNotAvailable(sqlCmd.Parameters["@NoOfGears"].Value.ToString());
        //    ltr_TransmissionType.Text = ShowNotAvailable(sqlCmd.Parameters["@TransmissionType"].Value.ToString());
        //    ltr_Clutch.Text = ShowNotAvailable(sqlCmd.Parameters["@Clutch"].Value.ToString());
        //    ltr_Performance_0_60_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_0_60_kmph"].Value.ToString());
        //    ltr_Performance_0_80_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_0_80_kmph"].Value.ToString());
        //    ltr_Performance_0_40_m.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_0_40_m"].Value.ToString());
        //    ltr_TopSpeed.Text = ShowNotAvailable(sqlCmd.Parameters["@TopSpeed"].Value.ToString());
        //    ltr_Performance_60_0_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_60_0_kmph"].Value.ToString());
        //    ltr_Performance_80_0_kmph.Text = ShowNotAvailable(sqlCmd.Parameters["@Performance_80_0_kmph"].Value.ToString());
        //    ltr_KerbWeight.Text = ShowNotAvailable(sqlCmd.Parameters["@KerbWeight"].Value.ToString());
        //    ltr_OverallLength.Text = ShowNotAvailable(sqlCmd.Parameters["@OverallLength"].Value.ToString());
        //    ltr_OverallWidth.Text = ShowNotAvailable(sqlCmd.Parameters["@OverallWidth"].Value.ToString());
        //    ltr_OverallHeight.Text = ShowNotAvailable(sqlCmd.Parameters["@OverallHeight"].Value.ToString());
        //    ltr_Wheelbase.Text = ShowNotAvailable(sqlCmd.Parameters["@Wheelbase"].Value.ToString());
        //    ltr_GroundClearance.Text = ShowNotAvailable(sqlCmd.Parameters["@GroundClearance"].Value.ToString());
        //    ltr_SeatHeight.Text = ShowNotAvailable(sqlCmd.Parameters["@SeatHeight"].Value.ToString());
        //    ltr_FuelTankCapacity.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelTankCapacity"].Value.ToString());
        //    ltr_ReserveFuelCapacity.Text = ShowNotAvailable(sqlCmd.Parameters["@ReserveFuelCapacity"].Value.ToString());
        //    ltr_FuelEfficiencyOverall.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelEfficiencyOverall"].Value.ToString());
        //    ltr_FuelEfficiencyRange.Text = ShowNotAvailable(sqlCmd.Parameters["@FuelEfficiencyRange"].Value.ToString());
        //    ltr_ChassisType.Text = ShowNotAvailable(sqlCmd.Parameters["@ChassisType"].Value.ToString());
        //    ltr_FrontSuspension.Text = ShowNotAvailable(sqlCmd.Parameters["@FrontSuspension"].Value.ToString());
        //    ltr_RearSuspension.Text = ShowNotAvailable(sqlCmd.Parameters["@RearSuspension"].Value.ToString());
        //    ltr_BrakeType.Text = ShowNotAvailable(sqlCmd.Parameters["@BrakeType"].Value.ToString());
        //    ltr_FrontDisc.Text = GetFeatures(sqlCmd.Parameters["@FrontDisc"].Value.ToString());
        //    ltr_FrontDisc_DrumSize.Text = ShowNotAvailable(sqlCmd.Parameters["@FrontDisc_DrumSize"].Value.ToString());
        //    ltr_RearDisc.Text = GetFeatures(sqlCmd.Parameters["@RearDisc"].Value.ToString());
        //    ltr_RearDisc_DrumSize.Text = ShowNotAvailable(sqlCmd.Parameters["@RearDisc_DrumSize"].Value.ToString());
        //    ltr_CalliperType.Text = ShowNotAvailable(sqlCmd.Parameters["@CalliperType"].Value.ToString());
        //    ltr_WheelSize.Text = ShowNotAvailable(sqlCmd.Parameters["@WheelSize"].Value.ToString());
        //    ltr_FrontTyre.Text = ShowNotAvailable(sqlCmd.Parameters["@FrontTyre"].Value.ToString());
        //    ltr_RearTyre.Text = ShowNotAvailable(sqlCmd.Parameters["@RearTyre"].Value.ToString());
        //    ltr_TubelessTyres.Text = GetFeatures(sqlCmd.Parameters["@TubelessTyres"].Value.ToString());
        //    ltr_RadialTyres.Text = GetFeatures(sqlCmd.Parameters["@RadialTyres"].Value.ToString());
        //    ltr_AlloyWheels.Text = GetFeatures(sqlCmd.Parameters["@AlloyWheels"].Value.ToString());
        //    ltr_ElectricSystem.Text = ShowNotAvailable(sqlCmd.Parameters["@ElectricSystem"].Value.ToString());
        //    ltr_Battery.Text = ShowNotAvailable(sqlCmd.Parameters["@Battery"].Value.ToString());
        //    ltr_HeadlightType.Text = ShowNotAvailable(sqlCmd.Parameters["@HeadlightType"].Value.ToString());
        //    ltr_HeadlightBulbType.Text = ShowNotAvailable(sqlCmd.Parameters["@HeadlightBulbType"].Value.ToString());
        //    ltr_Brake_Tail_Light.Text = ShowNotAvailable(sqlCmd.Parameters["@Brake_Tail_Light"].Value.ToString());
        //    ltr_TurnSignal.Text = ShowNotAvailable(sqlCmd.Parameters["@TurnSignal"].Value.ToString());
        //    ltr_PassLight.Text = GetFeatures(sqlCmd.Parameters["@PassLight"].Value.ToString());
        //    ltr_Speedometer.Text = ShowNotAvailable(sqlCmd.Parameters["@Speedometer"].Value.ToString());
        //    ltr_Tachometer.Text = GetFeatures(sqlCmd.Parameters["@Tachometer"].Value.ToString());
        //    ltr_TachometerType.Text = ShowNotAvailable(sqlCmd.Parameters["@TachometerType"].Value.ToString());
        //    ltr_ShiftLight.Text = GetFeatures(sqlCmd.Parameters["@ShiftLight"].Value.ToString());
        //    ltr_ElectricStart.Text = GetFeatures(sqlCmd.Parameters["@ElectricStart"].Value.ToString());
        //    ltr_Tripmeter.Text = GetFeatures(sqlCmd.Parameters["@Tripmeter"].Value.ToString());
        //    ltr_NoOfTripmeters.Text = ShowNotAvailable(sqlCmd.Parameters["@NoOfTripmeters"].Value.ToString());
        //    ltr_TripmeterType.Text = ShowNotAvailable(sqlCmd.Parameters["@TripmeterType"].Value.ToString());
        //    ltr_LowFuelIndicator.Text = GetFeatures(sqlCmd.Parameters["@LowFuelIndicator"].Value.ToString());
        //    ltr_LowOilIndicator.Text = GetFeatures(sqlCmd.Parameters["@LowOilIndicator"].Value.ToString());
        //    ltr_LowBatteryIndicator.Text = GetFeatures(sqlCmd.Parameters["@LowBatteryIndicator"].Value.ToString());
        //    ltr_FuelGauge.Text = GetFeatures(sqlCmd.Parameters["@FuelGauge"].Value.ToString());
        //    ltr_DigitalFuelGauge.Text = GetFeatures(sqlCmd.Parameters["@DigitalFuelGauge"].Value.ToString());
        //    ltr_PillionSeat.Text = GetFeatures(sqlCmd.Parameters["@PillionSeat"].Value.ToString());
        //    ltr_PillionFootrest.Text = GetFeatures(sqlCmd.Parameters["@PillionFootrest"].Value.ToString());
        //    ltr_PillionBackrest.Text = GetFeatures(sqlCmd.Parameters["@PillionBackrest"].Value.ToString());
        //    ltr_PillionGrabrail.Text = GetFeatures(sqlCmd.Parameters["@PillionGrabrail"].Value.ToString());
        //    ltr_StandAlarm.Text = GetFeatures(sqlCmd.Parameters["@StandAlarm"].Value.ToString());
        //    ltr_SteppedSeat.Text = GetFeatures(sqlCmd.Parameters["@SteppedSeat"].Value.ToString());
        //    ltr_AntilockBrakingSystem.Text = GetFeatures(sqlCmd.Parameters["@AntilockBrakingSystem"].Value.ToString());
        //    ltr_Killswitch.Text = GetFeatures(sqlCmd.Parameters["@Killswitch"].Value.ToString());
        //    ltr_Clock.Text = GetFeatures(sqlCmd.Parameters["@Clock"].Value.ToString());
        //    ltr_Colors.Text = ShowNotAvailable(sqlCmd.Parameters["@Colors"].Value.ToString());
        //}   // End of ShowSpecs function

        ///// <summary>
        /////     Function to show the text "N/A" if data is not available
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //protected string ShowNotAvailable(string value)
        //{
        //    if (String.IsNullOrEmpty(value))
        //    {
        //        return "--";
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}

        ///// <summary>
        /////     PopulateWhere will check whether value is true or false and return whether value is available or not
        ///// </summary>
        ///// <param name="featureValue"></param>
        ///// <returns></returns>
        //protected string GetFeatures(string featureValue)
        //{
        //    string showValue = String.Empty;

        //    if (String.IsNullOrEmpty(featureValue))
        //    {
        //        showValue = "--";
        //    }
        //    else
        //    {
        //        showValue = featureValue == "True" ? "Yes" : "No";
        //    }
        //    return showValue;
        //}   // End of GetFeatures method

        protected void GetVersionDetails()
        {
            if (!String.IsNullOrEmpty(modelId) && modelId != "-1")
            {
                ctrl_BikeRatings.ModelId = modelId;
            }
            else
            {
                Response.Redirect("/" + Request.QueryString["make"] + "-bikes/");
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2014
        /// Summary : to bind version dropdownlist
        /// </summary>
        protected void BindVersionDDL()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>();
                IBikeVersions<BikeVersionEntity, int> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                EnumBikeType SpecsType = mmv.IsNew ? EnumBikeType.NewBikeSpecs : EnumBikeType.UsedBikeSpecs;

                List<BikeVersionsListEntity> objVersionList = objVersion.GetVersionsByType(SpecsType,Convert.ToInt32(modelId));

                versionCount = objVersionList.Count;

                if (versionCount > 0)
                {
                    ddlVersion.DataSource = objVersionList;
                    ddlVersion.DataValueField = "VersionId";
                    ddlVersion.DataTextField = "VersionName";
                    ddlVersion.DataBind();

                    versionIdSpecs = objVersionList[0].VersionId.ToString();
                }
            }
        }

    }   // End of class
}   // End of namespace