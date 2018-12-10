using System;
using System.Web.UI;
using Carwale.UI.Common;
using Carwale.Interfaces.Classified.SellCar;
using Microsoft.Practices.Unity;
using Carwale.Service;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class AddCarPhotos : Page
    {
        public string inquiryId = "-1", profileId = "";
        private ISellCarRepository _sellCarRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _sellCarRepo = container.Resolve<ISellCarRepository>();
            }
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUser.Id == "-1")
                    Response.Redirect("/Users/login.aspx?returnUrl=/MyCarwale/MyInquiries/EditSellCar.aspx");

                if (Request.QueryString["car"] != null && Request.QueryString["car"] != "")
                {
                    profileId = Request.QueryString["car"];
                    inquiryId = CommonOpn.GetProfileNo(profileId);

                    if (!CommonOpn.CheckId(inquiryId))
                    {
                        UrlRewrite.Return404();
                    }
                    if (!_sellCarRepo.IsCustomerAuthorizedToManageCar(Convert.ToInt32(CurrentUser.Id), Convert.ToInt32(inquiryId)))
                    {
                        UrlRewrite.Return404();
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
