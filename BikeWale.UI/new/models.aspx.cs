using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Memcache;
using System.Configuration;

namespace Bikewale.New
{
	public class Models : Page
	{
        protected Repeater rptModels,rptSeries;
		protected Label lblMakeDesc, lblMakeDescFull;
        protected RoadTest ucRoadTestMin;

        protected UserReviewsMin ucUserReviewsMin;
        protected NewsMin newsMin;
        protected UpcomingBikesMin ucUpcoming;
        protected BikeBookingWidget ctrBikeBooking;

        protected string modelwiseSegments = "", price = string.Empty;
		protected string makeId = "-1", bodyStyle = "", segment = "";
		protected string make = "" , makeMaskingName = "";
		protected string previousUrl = "";
		protected string browseBy = "";
		protected int disCount = 0;
		protected bool isDescription = false;

        protected Literal ltrDefaultCityName;
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			//Function to process and validate Query String  
            if (ProcessQueryString())
            {
                ltrDefaultCityName.Text = Bikewale.Common.Configuration.GetDefaultCityName;

                if (!Page.IsPostBack)
                {
                    DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                    dd.DetectDevice();

                    // Assign user controls
                    ucRoadTestMin.MakeId = makeId;
                    ucRoadTestMin.HeaderText = make + " Road Tests/First Drives";

                    ucUpcoming.MakeId = makeId;

                    ucUserReviewsMin.MakeId = makeId;

                    newsMin.MakeId = makeId;

                    ctrBikeBooking.MakeId = makeId;
                    ctrBikeBooking.Make = make;
                    ShowSeriesModels();
                }
            }
		}
		
        private void ShowSeriesModels()
        {          
            DataSet ds = null;
            int minPrice = 999999999;
            int maxPrice = 0;

            try
            {
                ManageSeries objMS = new ManageSeries();

                ds = objMS.ShowSeriesModels(makeId);

                rptSeries.DataSource = ds;
                rptSeries.DataBind();

                //get min and max price of a bike
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(ds.Tables[0].Rows[i]["MinPrice"].ToString()))
                        {
                            if (Convert.ToInt32(ds.Tables[0].Rows[i]["MinPrice"]) < minPrice)
                            {
                                minPrice = Convert.ToInt32(ds.Tables[0].Rows[i]["MinPrice"]);
                            }

                            if (Convert.ToInt32(ds.Tables[0].Rows[i]["MinPrice"]) > maxPrice)
                            {
                                maxPrice = Convert.ToInt32(ds.Tables[0].Rows[i]["MinPrice"]);
                            }
                        }
                    }

                    price = ((!String.IsNullOrEmpty(minPrice.ToString()) && minPrice != 999999999) ? "Rs." + CommonOpn.FormatPrice(minPrice.ToString()) : String.Empty) + ((!String.IsNullOrEmpty(maxPrice.ToString()) && maxPrice != 0) ? " - Rs." + CommonOpn.FormatPrice(maxPrice.ToString()) : String.Empty);
                }
                
            }       
            catch (Exception err)
            {
                Trace.Warn("Exception in GetSeriesModels");
                ErrorClass objErr = new ErrorClass(err, "ShowSeriesModels");
                objErr.SendMail();
            }          
        }

		bool ProcessQueryString()
		{
            bool isSucess = true;

			if(!String.IsNullOrEmpty(Request.QueryString["make"]))
			{
                makeId = MakeMapping.GetMakeId(Request.QueryString["make"]);
                //verify the id as passed in the url
                if (CommonOpn.CheckId(makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSucess = false;
                }
                else
                {
                    GetMakeName();
                }
			}
			else
			{             
                //invalid make id, hence redirect to the new default page
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSucess = false;
			}

            return isSucess;
		}

        protected void GetMakeName()
        {
            Database db = null;
            DataSet ds = null;

            try
            {
                db = new Database();

                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select Name from bikemakes With(NoLock) where id=" + makeId;

                    ds = db.SelectAdaptQry(cmd);
                   
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        make = ds.Tables[0].Rows[0]["Name"].ToString();
                        makeMaskingName = Request.QueryString["make"].ToString();
                    }
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
            finally
            {
                db.CloseConnection();
            }
        }

        //protected void ShowModels()
        //{
        //    Database db = null;
        //    DataSet ds = null;
        //    string sql = string.Empty;

        //    try
        //    {
        //        //sql = " SELECT DISTINCT Bike.ID AS ID, Bike.BikeMakeId As MakeId, Bike.Name AS Model, IsNull(Bike.ReviewRate, 0) AS ReviewRate, "
        //            //+ " IsNull(Bike.ReviewCount, 0) AS ReviewCount, Bike.SmallPic, Bike.New, Bike.HostURL, "
        //            ////+ " (SELECT TOP 1 MIN(AvgPrice) FROM Con_NewBikeNationalPrices WHERE Con_NewBikeNationalPrices.VersionId IN "
        //            //+ " (SELECT ID FROM BikeVersions WHERE BikeModelId = Bike.ID AND New = 1) AND AvgPrice > 0) AS MinPrice "
        //            //+ " FROM BikeModels AS Bike, BikeVersions AS CV, NewBikeSpecifications NS "
        //            //+ " WHERE Bike.IsDeleted = 0 AND CV.BikeModelId = Bike.ID AND  NS.BikeVersionID=CV.ID AND "
        //            //+ " Bike.BikeMakeId = @BikeMakeId ";

        //        sql =   "SELECT DISTINCT BM.ID AS ID, BM.Name AS Model, "
        //                + " MIN(SP.Price) MinPrice,MAX(SP.Price) MaxPrice, "
        //                + " IsNull(BM.ReviewRate, 0) AS ReviewRate, "
        //                + " IsNull(BM.ReviewCount, 0) AS ReviewCount, BM.SmallPic, BM.HostURL,BM.MaskingName As ModelMaskingName,BMO.MaskingName AS MakeMaskingName"
        //                + " FROM BikeModels AS BM "
        //                + "	INNER JOIN BikeMakes BMO ON BM.BikeMakeId = BMO.ID "
        //                + " LEFT JOIN NewBikeShowroomPrices SP ON SP.BikeModelId = BM.ID AND SP.CityId= " + Bikewale.Common.Configuration.GetDefaultCityId
        //                + " WHERE BM.IsDeleted = 0 AND BM.Futuristic = 0 AND BM.New = 1 AND BM.BikeMakeId = @BikeMakeId "
        //                + " GROUP BY BM.ID,BM.Name, IsNull(BM.ReviewRate, 0), IsNull(BM.ReviewCount, 0), BM.SmallPic, BM.HostURL,BM.MaskingName,BMO.MaskingName"
        //                + " ORDER BY MIN(SP.Price) ";

        //        db = new Database();

        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandText = sql;
        //            cmd.CommandType = CommandType.Text;

        //            cmd.Parameters.Add("@BikeMakeId", SqlDbType.VarChar, 10).Value = makeId;

        //            ds = db.SelectAdaptQry(cmd);

        //            rptModels.DataSource = ds;
        //            rptModels.DataBind();
        //        }
        //    }
        //    catch (SqlException err)
        //    {
        //        ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
        //        objErr.SendMail();
        //    }
        //    catch (Exception err)
        //    {
        //        ErrorClass objErr = new ErrorClass(err, "new.default.LoadMakes");
        //        objErr.SendMail();
        //    }
        //    finally
        //    {
        //        db.CloseConnection();
        //    }
        //}

        //public string GetRateImage(double value)
        //{
        //    string oneImg = "<img src='http://img.carwale.com/images/ratings/1.gif' align='absmiddle'>";
        //    string zeroImg = "<img src='http://img.carwale.com/images/ratings/0.gif' align='absmiddle'>";
        //    string halfImg = "<img src='http://img.carwale.com/images/ratings/half.gif' align='absmiddle'>";	
			
        //    StringBuilder sb = new StringBuilder();
        //    int absVal = (int)Math.Floor(value);
			
        //    Trace.Warn(absVal.ToString());
			
        //    int i;
        //    for(i = 0; i < absVal; i++)
        //        sb.Append(oneImg);
				
        //    if(value > absVal)
        //        sb.Append(halfImg);
        //    else
        //        i--;
			
        //    for(int j = 5; j > i + 1; j--)
        //        sb.Append(zeroImg);
				
        //    return sb.ToString();
        //}

        /// <summary>
        /// Written By : Ashwini Todkar on 4 March 2014
        /// summary    : function add series row for displaying series
        /// </summary>
        /// <param name="seriesRank"></param>
        /// <param name="seriesId"></param>
        /// <param name="bikeSeries"></param>
        /// <param name="bikeModel"></param>
        /// <param name="minPrice"></param>
        /// <param name="hostUrl"></param>
        /// <param name="smallPic"></param>
        /// <param name="modelReviewCount"></param>
        /// <param name="modelMappingName"></param>
        /// <param name="makeMappingname"></param>
        /// <param name="seriesMappingName"></param>
        /// <param name="modelCount"></param>
        /// <returns></returns>
        public string GetSeriesRow(string seriesRank, string seriesId, string bikeSeries, string bikeModel, string minPrice, string hostUrl, string smallPic, string modelReviewCount, string modelMappingName, string makeMappingname, string seriesMappingName,string modelCount,string modelId,string reviewRate)
        {
            StringBuilder sb = new StringBuilder();
            string strShowPrice = string.Empty;
            string strShowRatings = string.Empty;
            string strShowReviews = string.Empty;
            int modelCountValue;
            int.TryParse(modelCount, out modelCountValue);
            modelCountValue = modelCountValue + 1;

            if (seriesRank == "1")
            {
                if (modelCount == "1")
                {
                    Trace.Warn("++", minPrice);

                    double modelReviewRate = Convert.ToDouble(reviewRate);
                    strShowPrice = (minPrice.Equals("N/A")) ? string.Empty : "</br><a href='/pricequote/default.aspx?model=" + modelId + "' pageCatId=\"1\" class='fillPopupData' modelId='"+modelId+"'>Check on-road price</a>";
                    strShowReviews = (Convert.ToInt16(modelReviewCount) <= 0) ? "<a href='/content/userreviews/writereviews.aspx?bikem=" + modelId + "'>Write a review</a>" : "<a href='/" + makeMaskingName + "-bikes/" + modelMappingName + "/user-reviews/" + "'> " + modelReviewCount + " user reviews </a>";
                    strShowRatings = (modelReviewRate > 0) ? "<span>" + CommonOpn.GetRateImage(modelReviewRate) + " | </span>" : string.Empty;

                    Trace.Warn("++rate", modelReviewRate.ToString());
                    Trace.Warn("++img ", CommonOpn.GetRateImage(modelReviewRate));
                    string imgSrc = smallPic == "" ? "http://img.carwale.com/bikewaleimg/common/nobike.jpg" : MakeModelVersion.GetModelImage(hostUrl, smallPic);

                    sb.Append("<tr id='series_" + seriesId + "' class='series-row'>");
                    sb.Append("<td valign='top'><a href='/" + makeMappingname + "-bikes/" + modelMappingName + "/'><img alt='" + bikeSeries + "' title='" + bikeSeries + "' src='" + imgSrc + "' /></a></td>");
                    sb.Append("<td valign='top'><h3><a class='href-title' href='/" + makeMappingname + "-bikes/" + modelMappingName + "/'>" + bikeSeries);
                    sb.Append("</a></h3><p class='text-grey'>" + strShowRatings + strShowReviews + "</p>");
                    sb.Append("</td>");
                    sb.Append("<td valign='top'><strong> Starts At Rs. " + minPrice + "</strong>" + strShowPrice + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    string imgSrc = smallPic == "" ? "http://img.carwale.com/bikewaleimg/common/nobike.jpg" : MakeModelVersion.GetModelImage(hostUrl, smallPic);

                    sb.Append("<tr id='series_" + seriesId + "' class='series-row'>");
                    sb.Append("<td valign='top' rowspan='" + modelCountValue + "'><a href='/" + makeMappingname + "-bikes/" + seriesMappingName + "-series/'><img alt='" + bikeSeries + "' title='" + bikeSeries + "' src='" + imgSrc + "' /></a></td>");
                    sb.Append("<td valign='top'><h3><a class='href-title' href='/" + makeMappingname + "-bikes/" + seriesMappingName + "-series/'>" + bikeSeries);
                    sb.Append("</a></h3><p class='text-grey'><a id='" + seriesId + "' class='text-grey viewVersions'><span id='modShowIcon' class='icon-sheet right-arrow'></span></a></p>");
                    sb.Append("</td>");
                    sb.Append("<td>&nbsp;</td>");
                    sb.Append("</tr>");
                }
            }

            Trace.Warn("++++",MakeModelVersion.GetFormattedPrice(minPrice));

            return sb.ToString();
        }
     }
}
		
		