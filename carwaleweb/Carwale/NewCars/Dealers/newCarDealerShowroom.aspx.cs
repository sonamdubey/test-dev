using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Dealers;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.Dealers;
using Carwale.Entity.Dealers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Carwale.Utility;
using Carwale.Service;
using Carwale.Interfaces;
using Carwale.Entity.CarData;
using Carwale.Interfaces.Geolocation;
using System.Linq;

namespace Carwale.UI.NewCars.Dealers
{
    public class newCarDealerShowroom : Page
    {
        protected string StartTime, EndTime, DealerName = "", InitialModelImage, InitialModelName, MakeName, DealerMobileNo
            , BannerImageUrl = "", DealerCityName, DealerAddress = "", StateName, Pincode, dealerId, EmailId = "", urlMakeName = "",
            CityIdFromQuerystring, MakeIdFromQueryString, CityName,UserIP;
        protected bool redirect = false;
        protected int DealerCityId, DealerId, InitialModelId, MakeId, IsPremium, CampaignId;
        protected double DealerLatitude, DealerLongitude;
        protected DealerShowroomDetails dealer;
        protected Repeater rptImages, rptGalleryImages;
        protected string pageTitle = "", keywords = "", description = "", canon = "", alternateUrl = "";
        protected int counter = 0, countGalleryImages = 0;
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;

        public newCarDealerShowroom()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _carMakesCacheRepo = container.Resolve<ICarMakesCacheRepository>();
                _geoCitiesCacheRepo = container.Resolve<IGeoCitiesCacheRepository>();
            }
        }
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }
        /// <summary>
        /// Binds the Dealer details  
        /// </summary>
        /// <param name="queryString"></param>
        protected void BindDetails(string queryString)
        {
            try
            {
                //To redirect if not Premium
                CityIdFromQuerystring = Request.QueryString["cityId"].ToString();
                MakeIdFromQueryString = Request.QueryString["make"].ToString();

                var dealershowDetails = GetDealerDetails(queryString, MakeIdFromQueryString);

                //To get the MakeName
                GetMakeName();

                //To fetch CityName from CityId 
                GetCityName();

                if (dealershowDetails.objDealerDetails.Name != null)
                {
                    dealer = dealershowDetails;

                    //Binds the Contact Details section
                    Carwale.Entity.Dealers.DealerDetails details = dealer.objDealerDetails;
                    IsPremium = details.IsPremium;

                    if (IsPremium == 1)
                    {
                        DealerName = details.Name;
                        DealerMobileNo = details.Mobile;
                        DealerAddress = String.IsNullOrEmpty(details.Address) ? "" : details.Address;
                        EmailId = details.EMailId;
                        StartTime = details.ShowroomStartTime;
                        EndTime = details.ShowroomEndTime;
                        DealerLatitude = details.Latitude;
                        DealerLongitude = details.Longitude;
                        DealerCityId = details.CityId;
                        DealerCityName = details.CityName;
                        StateName = details.StateName;
                        Pincode = details.Pincode;
                        DealerId = Convert.ToInt16(dealerId);
                        CampaignId = Convert.ToInt16(details.CampaignId);

                        //Binds the images in jcarousel
                        rptImages.DataSource = dealer.objModelDetails;
                        rptImages.DataBind();

                        // Binds Gallery Images
                        if (dealer.objImageList != null && dealer.objImageList[0].OriginalImgPath != null)
                        {
                            rptGalleryImages.DataSource = dealer.objImageList;
                            rptGalleryImages.DataBind();
                            //Banner image 

                            var bannerImg = dealer.objImageList.Where(x => x.IsMainBanner).FirstOrDefault();
                            if (bannerImg != null && bannerImg.OriginalImgPath != null)
                            {
                                BannerImageUrl = bannerImg.HostUrl + "0x0" + bannerImg.OriginalImgPath;
                            }
                            countGalleryImages = dealer.objImageList.Count;
                        }
                        
                        //Binds the first Large ModelImage 
                        InitialModelImage = string.IsNullOrEmpty(dealer.objModelDetails[0].OriginalImage) ? "" : dealer.objModelDetails[0].HostUrl + ImageSizes._559X314 + dealer.objModelDetails[0].OriginalImage;
                        InitialModelName = dealer.objModelDetails[0].ModelName;
                        InitialModelId = dealer.objModelDetails[0].ModelId;

                        

                        //Page meta tags
                        keywords = dealer.objDealerDetails.Name + "," + dealer.objDealerDetails.Name + " Dealer," + dealer.objDealerDetails.Name + " showroom," + dealer.objDealerDetails.Name + " " + dealer.objDealerDetails.CityName;
                        pageTitle = dealer.objDealerDetails.Name + " " + dealer.objDealerDetails.CityName + "-" + dealer.objDealerDetails.Name + " Dealer Showroom";
                        description = dealer.objDealerDetails.Name + " is dealer of " + urlMakeName + " cars in " + dealer.objDealerDetails.CityName + ". Get best offers on " + urlMakeName + " cars at " + dealer.objDealerDetails.Name + " showroom";
                        canon = Format.FormatURL(urlMakeName) + "-dealers/" + dealer.objDealerDetails.CityId + "-" + Format.FormatSpecial(dealer.objDealerDetails.CityName) + "/" + Format.FormatSpecial(dealer.objDealerDetails.Name) + "-" + DealerId;
                        alternateUrl = Format.FormatURL(urlMakeName) + "-dealers-showroom/" + dealer.objDealerDetails.CityId + "-" + Format.FormatSpecial(dealer.objDealerDetails.CityName) + "/" + Format.FormatSpecial(dealer.objDealerDetails.Name) + "-" + DealerId + "/";
                    }
                    else
                    {
                        redirect = true;
                    }
                }
                else
                {
                    redirect = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            if (redirect)
            {
                Response.Redirect("/" + UrlRewrite.FormatSpecial(urlMakeName) + "-dealer-showrooms/" + UrlRewrite.FormatSpecial(CityName) + "-" + UrlRewrite.FormatSpecial(CityIdFromQuerystring));
            }
        }

        /// <summary>
        /// To fetch MakeName from MakeId
        /// </summary>
        public void GetMakeName()
        {
            if (MakeIdFromQueryString != "0")
            {
                CarMakeEntityBase carMakes = _carMakesCacheRepo.GetCarMakeDetails(Convert.ToInt32(MakeIdFromQueryString));
                urlMakeName = carMakes.MakeName;
            }
        }

        /// <summary>
        /// To fetch the city from cityId
        /// </summary>
        public void GetCityName()
        {
            if (CityIdFromQuerystring != "0")
            {
               CityName = _geoCitiesCacheRepo.GetCityNameById(CityIdFromQuerystring);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            /*************************** Device Detection *****************************/
            string pageUrl = "";

            if (!string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_REWRITE_URL"]))
            {
                pageUrl = Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString();
            }

            DeviceDetection dd = new DeviceDetection(pageUrl);
            dd.DetectDevice();
            /*************************************************************/

            UserIP = UserTracker.GetUserIp();

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["DealerId"]) && CommonOpn.CheckId(Request.QueryString["DealerId"]))
                {
                    dealerId = Request.QueryString["DealerId"].ToString();
                    BindDetails(dealerId);
                }
                else
                {
                    Response.Redirect("/new/locatenewcardealers.aspx");
                }
            }
        }

        /// <summary>
        /// Fetches the Details required 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected DealerShowroomDetails GetDealerDetails(string queryString, string makeId)
        {
            DealerShowroomDetails objTask = null;
            try
            {
               INewCarDealers dealerDetails = UnityBootstrapper.Resolve<INewCarDealers>();
               objTask = dealerDetails.GetDealerDetails(Convert.ToInt32(queryString), null, null, Convert.ToInt32(makeId));
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objTask;
        }
    }//class
}//namespace