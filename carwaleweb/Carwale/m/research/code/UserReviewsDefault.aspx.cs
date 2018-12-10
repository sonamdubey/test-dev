using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using Carwale.Service;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Entity.Enum;
using System.Collections.Generic;
using System.Linq;

namespace MobileWeb.Research
{
    public class UserReviewsDefault : Page
    {
        protected DropDownList ddlMakes,ddlModels;
        protected LinkButton btnSubmit;
        protected TextBox txtMake, txtModel;
        protected Repeater rptMostReviewed, rptMostRead, rptMostHelpful, rptMostRecent, rptMostRated, rptMakes;
        protected DataSet dsUserReviews;
        IUserReviewsCache userReviewCacheRepo = UnityBootstrapper.Resolve<IUserReviewsCache>();

        protected string makeId = "-1";

        protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
		}
      
        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadMakes();
                ddlModels.Items.Insert(0, new ListItem("--Select--", "0"));
                btnSubmit.Attributes.Add("onclick", "javascript:if(IsValid()==false)return false;");
                GetMostReviewedCars();
                BindMostReadReviews();
                BindMostHelpfulReviews();
                BindMostRecentReviews();
                BindMostRatedReviews();
            }
        }

        private void LoadMakes()
        {
            ICarMakesCacheRepository makeCacheRepo = UnityBootstrapper.Resolve<ICarMakesCacheRepository>();
            ddlMakes.DataSource = makeCacheRepo.GetCarMakesByType("All");
            ddlMakes.DataTextField = "MakeName";
            ddlMakes.DataValueField = "MakeId";
            List<Carwale.Entity.CarData.CarMakeEntityBase> carMakes = makeCacheRepo.GetCarMakesByType("new").OrderBy(x => x.MakeName).ToList();
            rptMakes.DataSource = carMakes;
            rptMakes.DataBind();
            ddlMakes.DataBind();
            ddlMakes.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            string redirectUrl = "~/m";
                         
            if (txtMake.Text.Trim() != "" && txtModel.Text.Trim()!="")
            {
                redirectUrl += "/" + MobileWeb.Common.CommonOpn.FormatSpecial(txtMake.Text.Trim()) + "-cars/" + txtModel.Text.Trim();
            }

            redirectUrl += "/userreviews/";
            Response.Redirect(redirectUrl);
        }

        private void GetMostReviewedCars()
        {
            rptMostReviewed.DataSource =  userReviewCacheRepo.GetMostReviewedCars();
            rptMostReviewed.DataBind();
        }

        private void BindMostReadReviews()
        {
            rptMostRead.DataSource = userReviewCacheRepo.GetUserReviewsByType(UserReviewsSorting.Viewed);
            rptMostRead.DataBind();
        }

        private void BindMostHelpfulReviews()
        {
            rptMostHelpful.DataSource = userReviewCacheRepo.GetUserReviewsByType(UserReviewsSorting.Liked);
            rptMostHelpful.DataBind();
        }

        private void BindMostRecentReviews()
        {
            rptMostRecent.DataSource = userReviewCacheRepo.GetUserReviewsByType(UserReviewsSorting.EntryDateTime);
            rptMostRecent.DataBind();
        }

        private void BindMostRatedReviews()
        {
            rptMostRated.DataSource = userReviewCacheRepo.GetUserReviewsByType(UserReviewsSorting.OverallR);
            rptMostRated.DataBind();
        }        
    }
}