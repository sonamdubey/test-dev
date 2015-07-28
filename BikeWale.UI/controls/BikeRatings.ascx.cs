using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public class BikeRatings : System.Web.UI.UserControl
    {
        private string modelId = string.Empty, versionId = string.Empty, makeName = string.Empty, modelName = string.Empty,modelMaskingName = string.Empty,makeMaskingName = string.Empty;
        protected HtmlGenericControl divDetails;
        private double modelReviewRate = 0, modelReviewCount = 0;
        ModelVersionDescription mmv = new ModelVersionDescription();

        public string ModelId
        {
            get { return (modelId == "" ? "-1" : modelId); }
            set { modelId = value; }
        }

        public string VersionId
        {
            get { return (versionId == "" ? "-1" : versionId); }
            set { versionId = value; }
        }

        public ModelVersionDescription Mmv
        {
            get { return mmv; }
            set { mmv = value; }
        }			

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Trace.Warn("ModelId bikeratings : ", ModelId);

                if (!String.IsNullOrEmpty(ModelId) && ModelId != "-1")
                {
                    Mmv.GetDetailsByModel(ModelId);

                    modelReviewRate = Mmv.ModelRatingOverall;
                    modelReviewCount = Mmv.ModelReviewCount;
                    makeName = Mmv.MakeName;
                    modelName = Mmv.ModelName;
                    modelMaskingName = Mmv.ModelMaskingName;
                    makeMaskingName = Mmv.MakeMaskingName;

                    ShowImage(modelReviewRate, modelReviewCount, makeMaskingName, modelName, modelMaskingName);
                }
            }
          //  ModelId = Mmv.ModelId;
        }//pageload
        void ShowImage(double modelRate, double modelTotal, string makeMaskingName, string modelName, string modelMaskingName)
        {
            string writeReviewUrl = "/content/userreviews/writereviews.aspx?" + (VersionId == "-1" ? "bikem=" + ModelId : "bikev=" + VersionId);
            string readReviewUrlM = "/";

            divDetails.InnerHtml = "<span class='userRatingHd'>Average User Rating: </span><br />";

            if (modelTotal > 0)
            {
                //display the model rating only
                readReviewUrlM += makeMaskingName + "-bikes/" + modelMaskingName + "/user-reviews/";
                divDetails.InnerHtml += CommonOpn.GetRateImage(modelRate);
                divDetails.InnerHtml += "<div itemprop='rating' itemscope itemtype='http://data-vocabulary.org/Rating'>";
                divDetails.InnerHtml += "<span class='margin-left5' itemprop='value'>" + modelRate + "</span>/";
                divDetails.InnerHtml += "<span itemprop='best'>"+ 5 + "</span>";
                divDetails.InnerHtml += "</div>";
                divDetails.InnerHtml += " Based on ";
                divDetails.InnerHtml += "&nbsp;&nbsp;<a title='" + makeName + " " + modelName + " Reviews' class='reviewLink' href='" + readReviewUrlM + "'><span itemprop='votes'>" + modelTotal.ToString() + "</span> reviews</a>";
                //divDetails.InnerHtml += "&nbsp;&nbsp;<a class='reviewLink' href='" + readReviewUrlM + "'>" + modelTotal.ToString() + " reviews</a>";
                divDetails.InnerHtml += "<span class='reviewText'> | </span>";
                divDetails.InnerHtml += "<a rel='nofollow' class='reviewLink' href='" + writeReviewUrl + "'>Write a review</a>";
               

            }
            else
            {
                divDetails.InnerHtml += "<span class='reviewText'>No review available for this model.</span><br>";
                divDetails.InnerHtml += "<a rel='nofollow' class='reviewLink' href='" + writeReviewUrl + "'>Be the first one to write  a review</a>";
            }
        }
    }//class
}//namespace
