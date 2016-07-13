using System;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Memcache;

namespace Bikewale.New
{   
    /// <summary>
    /// Created By : Sadhana Upadhyay on 4th March 2014
    /// Summary : Added class for bike series
    /// </summary>
    public partial class bikeseries : Page
    {
        protected Repeater rptModels;
        protected Label lblSeriesDesc;
        protected RoadTest ucRoadTestMin;

        protected UserReviewsMin ucUserReviewsMin;
        protected NewsMin newsMin;
        protected UpcomingBikesMin ucUpcoming;
        protected BikeBookingWidget ctrBikeBooking;

        //protected string modelwiseSegments = "";
        protected string bodyStyle = "", segment = "", seriesId = string.Empty;
        protected string make = "", makeMaskingName = "", series = string.Empty, seriesMaskingName = string.Empty, seriesDesc = string.Empty;
        protected string previousUrl = "";
        protected string browseBy = "";
        protected int disCount = 0,makeId = 0;
        protected bool isDescription = false;
        protected string price=String.Empty;

        protected Literal ltrDefaultCityName;

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
            //Function to process and validate Query String  
            if (ProcessQueryString())
            {
                ltrDefaultCityName.Text = Configuration.GetDefaultCityName;

                // Modified By :Lucky Rathore on 12 July 2016.
                Form.Action = Request.RawUrl;

                if (!Page.IsPostBack)
                {
                    // Modified By :Ashish Kamble on 5 Feb 2016
                    string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    if (String.IsNullOrEmpty(originalUrl))
                        originalUrl = Request.ServerVariables["URL"];

                    DeviceDetection dd = new DeviceDetection(originalUrl);
                    dd.DetectDevice();

                    if (!String.IsNullOrEmpty(seriesId))
                    {
                        ShowSeries();
                        SeriesSynopsis();

                        ucRoadTestMin.MakeId = makeId.ToString();
                        ucRoadTestMin.HeaderText = make + " " + series + " Road Tests/First Drives";

                        ucUpcoming.SeriesId = seriesId;
                      

                        // Assign user controls
                        ucUserReviewsMin.SeriesId = seriesId;
                        newsMin.SeriesId = seriesId;
                        ctrBikeBooking.SeriesId = seriesId;
                        ctrBikeBooking.MakeId = makeId.ToString();
                        ctrBikeBooking.Make = make;
                        ctrBikeBooking.Series = series;
                    }
                }
            }
        }

        bool ProcessQueryString()
        {
            bool isSuccess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["series"]))
            {
                seriesId = SeriesMapping.GetSeriesId(Request.QueryString["series"]);

                if (String.IsNullOrEmpty(seriesId))
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
            }
            else
            {
                Response.Redirect("/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4th March 2014
        /// Summary : To get bike series synopsis
        /// </summary>
        protected void SeriesSynopsis()
        {
            try
            {
                ManageSeries ms = new ManageSeries();
                ms.GetSeriesSynopsis(seriesId,ref seriesDesc,ref makeId);
                Trace.Warn("series Synpsis : "+seriesDesc);
                if (!String.IsNullOrEmpty(seriesDesc))
                    lblSeriesDesc.Text = seriesDesc;

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4th March 2014
        /// Summary : Function to show all models of a series
        /// </summary>
        protected void ShowSeries()
        {
            DataSet ds = null;
            int minPrice = 999999999 ;
            int maxPrice = 0;
          
            try
            {
                ManageSeries ms = new ManageSeries();
                ds = ms.GetModelSeries(seriesId);

                if (ds != null)
                {
                    rptModels.DataSource = ds;
                    rptModels.DataBind();

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        make = ds.Tables[0].Rows[0]["MakeName"].ToString();
                        makeMaskingName = ds.Tables[0].Rows[0]["MakeMaskingName"].ToString();
                        series = ds.Tables[0].Rows[0]["SeriesName"].ToString();
                        seriesMaskingName = ds.Tables[0].Rows[0]["SeriesMaskingName"].ToString();

                        //get min and max price of a bike
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
                    }

                    price = ((!String.IsNullOrEmpty(minPrice.ToString()) && minPrice != 999999999) ? " Rs." + CommonOpn.FormatPrice(minPrice.ToString()) : String.Empty) + ((!String.IsNullOrEmpty(maxPrice.ToString()) && maxPrice != 0) ? " to Rs." + CommonOpn.FormatPrice(maxPrice.ToString()) : String.Empty);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of ShowSeries
    }   //End of class
}   //End of namespace