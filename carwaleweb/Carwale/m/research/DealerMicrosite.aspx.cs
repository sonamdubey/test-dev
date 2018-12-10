using Carwale.BL.GrpcFiles;
using Carwale.Cache.Core;
using Carwale.Cache.Dealers;
using Carwale.DAL.Dealers;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Service;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace MobileWeb.Research
{
    public class DealerMicrosite : System.Web.UI.Page
    {
        protected int DealerId, DealerCityId, CampaignId;
        protected string DealerName, DealerAddress, Phone, WorkingHours, DealerEmailId, MakeId, MakeName,
                        PageTitle = "", PageDescription = "", DealerCityName;
        protected double Latitude, Longitude;
        protected DateTime date;
        protected Carwale.Entity.Dealers.DealerDetails details = new Carwale.Entity.Dealers.DealerDetails();
        protected Repeater rptImages;
        protected bool ImagesAvailable = false; private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;

        public DealerMicrosite()
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

        protected void Page_Load(object sender, EventArgs e)
        {
            details.Pincode = "";
            details.StateName = "";

            if (Request["dealerId"] != null && Request.QueryString["dealerId"] != "" && CommonOpn.CheckId(Request.QueryString["dealerId"]))
            {
                try
                {
                    DealerId = Convert.ToInt32(Request.QueryString["dealerId"]);
                    MakeId = Request.QueryString["make"];

                    INewCarDealers newCarDealersBL = UnityBootstrapper.Resolve<INewCarDealers>();
                    details = newCarDealersBL.GetPremiumDealerDetails(DealerId);

                    DealerName = details.Name;
                    CampaignId = details.CampaignId;
                    DealerAddress = details.Address;
                    Phone = details.Mobile;
                    WorkingHours = details.WorkingHours;
                    DealerEmailId = details.EMailId;
                    Latitude = details.Latitude;
                    Longitude = details.Longitude;
                    DealerCityId = details.CityId;
                    date = DateTime.Now;
                    DealerCityName = details.CityName;
                    if (MakeId != "0")
                    {
                        MakeName = _carMakesCacheRepo.GetCarMakeDetails(Convert.ToInt32(MakeId)).MakeName;
                    }
                    //Page meta tags
                    PageTitle = DealerName + " " + details.CityName + "-" + DealerName + " Dealer Showroom - CarWale";
                    PageDescription = DealerName + " is dealer of " + MakeName + " cars in " + details.CityName + ". Get best offers on " + MakeName + " cars at " + DealerName + " showroom";
                }
                catch (Exception err)
                {
                    Trace.Warn("EX: " + err.Message);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                if (details.IsPremium != 1)
                {
                    Response.Redirect("/m/" + Carwale.Utility.Format.FormatURL(MakeName) + "-dealer-showrooms/" + Carwale.Utility.Format.FormatText(Request.QueryString["cityname"]) + "-" + CustomParser.parseIntObject(Request.QueryString["cityid"]) + "/");
                }
                else LoadImages();
                return;
            }
            else
            {
                Response.Redirect("/m/");
                return;
            }
        }

        protected void LoadImages()
        {
            IDealers imgList = UnityBootstrapper.Resolve<IDealers>();

            List<AboutUsImageEntity> Images = imgList.GetDealerImages(DealerId);
            if (Images.Count > 0 && Images[0].OriginalImgPath != null)
            {
                ImagesAvailable = true;
                rptImages.DataSource = Images;
                rptImages.DataBind();
            }
        }
    }
}