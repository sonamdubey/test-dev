using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.Interfaces.Classified.SellCar;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Classified.UsedCarPhotos;
using Carwale.Entity.Classified;
using System.Collections.Generic;
using Carwale.Entity.Enum;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.Classified.MyListings;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class SellCarPhotos : Page
    {
        protected Repeater rptImageList;
        public string inquiryId = "-1", profileId = "";
        protected string imgCategory = "0";

        private ISellCarRepository _sellCarRepo;
        private ICarPhotosRepository _carPhotoRepo;
        private IMyListingsRepository _mylistingsRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _sellCarRepo = container.Resolve<ISellCarRepository>();
                _carPhotoRepo = container.Resolve<ICarPhotosRepository>();
                _mylistingsRepo = container.Resolve<IMyListingsRepository>();
            }
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUser.Id == "-1")
                    Response.Redirect("/Users/login.aspx?returnUrl=/MyCarwale/MyInquiries/MySellInquiry.aspx");
                profileId = Request.QueryString["car"];
                inquiryId = CommonOpn.GetProfileNo(profileId);
                if (!_mylistingsRepo.IsCarCustomerEditable(Convert.ToInt32(inquiryId))) // check if inquiry id is editable i.e inquiry is preimum
                {
                    Response.Redirect("/mycarwale/myinquiries/mysellinquiry.aspx?ispremium=true", true);
                }

                imgCategory = Convert.ToInt16(ImageCategories.USEDSELLCARS).ToString();

                if (Request["car"] != null && Request.QueryString["car"] != "")
                {
                    if (!CommonOpn.CheckId(inquiryId))
                    {
                        UrlRewrite.Return404();
                    }

                    if (_sellCarRepo.IsCustomerAuthorizedToManageCar(Convert.ToInt32(CurrentUser.Id), Convert.ToInt32(inquiryId)))
                    {
                        if (_sellCarRepo.GetSellCarStepsCompleted(Convert.ToInt32(inquiryId)) == (int)SellCarSteps.Confirmation)
                        {
                            bool isDealer = false;
                            List<CarPhoto> carPhotos = _carPhotoRepo.GetCarPhotos(Convert.ToInt32(inquiryId), isDealer);
                            rptImageList.DataSource = carPhotos;
                            rptImageList.DataBind();

                            // If image not uploaded by the user redirect to advance photo uploader page
                            if (carPhotos.Count == 0)
                            {
                                Response.Redirect("AddCarPhotos.aspx?car=" + profileId);
                            }
                        }
                        else
                        {
                            Response.Redirect("/mycarwale/myinquiries/mysellinquiry.aspx", true);
                        }
                    }
                    else
                    {
                        Response.Redirect("/mycarwale/", true);
                    }
                }
                else
                {
                    UrlRewrite.Return404();
                }
            }
        }
    }
}
