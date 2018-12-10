using System;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Specialized;
using RabbitMqPublishing;
using Carwale.UI.Common;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Entity.Classified;
using System.Collections.Generic;
using Carwale.Interfaces.Classified.UsedCarPhotos;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Enum;
using Carwale.Utility;
using Carwale.Interfaces.Classified.MyListings;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class BasicUploader : Page
    {
        protected Repeater rptImageList;
        protected HtmlGenericControl divAlertMsg;

        protected string inquiryId = "-1", profileId = "";
        private readonly bool isDealer = false;
        private ISellCarRepository _sellCarRepo;
        private ICarPhotosRepository _carPhotoRepo;
        private IMyListings _myListings;
        private IMyListingsRepository _mylistingsRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected override void OnError(EventArgs e)
        {
            // At this point we have information about the error
            HttpContext ctx = HttpContext.Current;
            bool result = ctx.Server.GetLastError().GetType() == typeof(HttpException);

            if (result)
            {
                HttpException httpEx = (HttpException)ctx.Server.GetLastError();
                ctx.Server.ClearError();
                base.OnError(e);

                ErrorClass objErr = new ErrorClass(httpEx, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            else
            {
                Exception err = ctx.Server.GetLastError();
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                ctx.Response.Redirect("UploadBasic.aspx?error=error");
            }
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _sellCarRepo = container.Resolve<ISellCarRepository>();
                _carPhotoRepo = container.Resolve<ICarPhotosRepository>();
                _myListings = container.Resolve<IMyListings>();
                _mylistingsRepo = container.Resolve<IMyListingsRepository>();
            }
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
                Response.Redirect("/Users/login.aspx?returnUrl=/MyCarwale/MyInquiries/EditSellCar.aspx");

            if (!String.IsNullOrEmpty(Request.QueryString["error"]) && Request.QueryString["error"].ToString() == "error")
            {
                divAlertMsg.Visible = true;
                divAlertMsg.InnerText = "An error has occurred.  Please try again or skip this step for now and upload pictures later.";
            }

            if (Request["car"] != null && Request.QueryString["car"] != "")
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
                //This cookie will be used for updating the Database with uploaded image directly to S3 from client side
                _myListings.AddCookie("SellInquiry", CarwaleSecurity.Encrypt(inquiryId.ToString()), "/");
            }
            else
            {
                UrlRewrite.Return404();
            }

            if (!_mylistingsRepo.IsCarCustomerEditable(Convert.ToInt32(inquiryId))) // check if inquiry id is editable i.e inquiry is preimum
            {
                Response.Redirect("/mycarwale/myinquiries/mysellinquiry.aspx?ispremium=true", true);
            }
            ShowPhotos();
        }

        void ShowPhotos()
        {
            List<CarPhoto> carPhotos = _carPhotoRepo.GetCarPhotos(Convert.ToInt32(inquiryId), isDealer);
            rptImageList.DataSource = carPhotos;
            rptImageList.DataBind();
        }
    }
}
