using Carwale.BL.Stock;
using Carwale.DAL.Classified.Stock;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Stock;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace Carwale.UI.m.used
{
    public partial class PhotoGallery : System.Web.UI.Page
    {
        private IStockBL _stockBL;
        private ISearchUtility _searchUtility;
        public string maskingNumber;
        public string profileId, rootId, cityName, kmnumeric, dc, similarCarsUrl;
        public bool isTopRated;
        private bool isSold;
        private string urlToRedirect;
        protected int ctePackageId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["inquiryId"]) && !string.IsNullOrEmpty(Request.QueryString["isDealer"]))
            {
                using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
                {
                    _stockBL = container.Resolve<IStockBL>();
                    _searchUtility = container.Resolve<ISearchUtility>();
                }
                BindPhotoGallery();
                FillBuyerProcessParams();
                if(isSold)
                {
                    Response.RedirectPermanent(urlToRedirect);
                }
            }
            else
                Response.Redirect("~/m/used/");
        }

        protected void BindPhotoGallery()
        {
            try
            {
                string inquiryId = HttpContext.Current.Request.QueryString["inquiryid"].ToString();
                bool isDealer = Convert.ToBoolean(HttpContext.Current.Request.QueryString["isdealer"]);
                maskingNumber = HttpContext.Current.Request.QueryString["mask"]!=null?HttpContext.Current.Request.QueryString["mask"].ToString():"";

                IUnityContainer container = new UnityContainer();

                container.RegisterType<IStockRepository, StockRepository>();
                IStockRepository stockDetails = container.Resolve<IStockRepository>();
                ImageGalleryEntity imgs = stockDetails.GetImagesByProfileId(inquiryId, isDealer);

                rptPhotos.DataSource = imgs.ImgFullURLs;
                rptPhotos.DataBind();

                rptPhotoTabs.DataSource = imgs.ImgFullURLs;
                rptPhotoTabs.DataBind();
            }
            catch (Exception ex)
            {
                SendErrorMail(ex, "BindPhotoGallery");
            }
        }

        protected void FillBuyerProcessParams()
        {
            try
            {
                HttpRequest httpContext = HttpContext.Current.Request;
                profileId = httpContext.QueryString["profileid"];
                isTopRated = Convert.ToBoolean(httpContext.QueryString["istoprated"]);
                CarDetailsEntity carDetails = _stockBL.GetStock(profileId);
                if (carDetails == null || carDetails.IsSold)
                {
                    isSold = true;
                    urlToRedirect = $"/m{ _searchUtility.GetURL(carDetails?.BasicCarInfo?.MakeName, carDetails?.BasicCarInfo?.RootName, carDetails?.BasicCarInfo?.CityMaskingName) }?issold=true";
                }
                else
                {
                    rootId = carDetails.BasicCarInfo.RootId.ToString();
                    cityName = carDetails.BasicCarInfo.CityName;
                    kmnumeric = carDetails.BasicCarInfo.KmNumeric;
                    dc = httpContext.QueryString["dc"];
                    int deliveryCityId, priceNumeric;
                    Int32.TryParse(dc, out deliveryCityId);
                    Int32.TryParse(carDetails.BasicCarInfo.PriceNumeric, out priceNumeric);
                    int versionSubSegmentIdNumeric;
                    Int32.TryParse(carDetails.BasicCarInfo.VersionSubSegmentID, out versionSubSegmentIdNumeric);
                    similarCarsUrl = StockRecommendationsBL.GetSimilarCarsUrl(profileId, Convert.ToInt32(rootId), Convert.ToInt32(carDetails.BasicCarInfo.CityId), deliveryCityId, priceNumeric, versionSubSegmentIdNumeric);
                    ctePackageId = carDetails.BasicCarInfo.CtePackageId;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        void SendErrorMail(Exception ex, string methodName)
        {
            ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : " + methodName);
            objErr.SendMail();
        }

    }
}