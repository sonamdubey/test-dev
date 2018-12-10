using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using Carwale.BL;
using Carwale.DTOs;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces;
using AEPLCore.Cache;
using Carwale.BL.CarData;
using Carwale.DAL.CarData;
using Carwale.Cache.CarData;
using Carwale.Entity.CarData;
using System.Web.UI.WebControls;

namespace Carwale.UI.Controls
{
    public class CarRating : System.Web.UI.UserControl
    {
        public CarModelDetails ModelDetails { get; set; }
        public Literal ltrRatingStars, ltrReviewCount;
        public String Rating { get; set; }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && ModelDetails != null)
            {
                //Literal controls used to form Google rich snippets - reviews
                ltrRatingStars.Text = CommonOpn.GetRateImage(ModelDetails.ModelRating);
                Rating = Math.Round((decimal)ModelDetails.ModelRating, 2).ToString();
                ltrReviewCount.Text = ModelDetails.ReviewCount.ToString();
            }
        } // Page_Load

    }

}