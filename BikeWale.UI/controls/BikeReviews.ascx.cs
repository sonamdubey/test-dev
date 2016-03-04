using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public class BikeReviews : System.Web.UI.UserControl
    {
        protected HtmlGenericControl ctrlNews, divMoreNews, ctrlNewsNotFound;
        protected Repeater rptUserReviews;
        private string reviewerId = "-1";

        //this shows the number of reviews to be shown. default is set to 5
        public string ReviewCount
        {
            get
            {
                if (ViewState["ReviewCount"] != null && ViewState["ReviewCount"].ToString() != "")
                    return ("TOP " + ViewState["ReviewCount"].ToString());
                else
                    return "";
            }
            set
            {
                Trace.Warn("News Control : setting NewsCount : " + value.ToString());
                ViewState["ReviewCount"] = value;
            }
        }

        public bool ShowComment
        {
            get
            {
                if (ViewState["ShowComment"] != null && ViewState["ShowComment"].ToString() != "")
                    return Convert.ToBoolean(ViewState["ShowComment"]);
                else
                    return false;
            }
            set
            {
                ViewState["ShowComment"] = value;
            }
        }

        //this shows the number of reviews to be shown. default is set to 5
        public string RetriveBy
        {
            get
            {
                if (ViewState["RetriveBy"] != null && ViewState["RetriveBy"].ToString() != "")
                    return Convert.ToString(ViewState["RetriveBy"]);
                else
                    return "MostRecent";
            }
            set
            {
                Trace.Warn("News Control : setting NewsCount : " + value.ToString());
                ViewState["RetriveBy"] = value;
            }
        }

        //this shows the number of reviews to be shown. default is set to 5
        public string ReviewerId
        {
            get { return reviewerId; }
            set { reviewerId = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            BindReviews();
        }

        //This function gets the list of the top 5 news for the

        public void BindReviews()
        {
            string sql = "";

            CommonOpn objCom = new CommonOpn();

            if (ReviewCount != "0")
            {
                sql = " SELECT " + ReviewCount + " CR.Id AS ReviewId, Title, OverallR, "
                    + " CM.Name +'-'+ CMO.Name AS ModelName, C.Name AS CustomerName, C.Id AS CustomerId, CM.MaskingName + CMO.MaskingName AS ModelMaskingName, "
                    + " Substring(CR.Comments,0,Cast(Floor(LEN(CR.Comments)*0.4) AS INT)) AS  ShortComment, CM.Name AS MakeName, CM.MaskingName AS MakeMaskingName, CMO.Name CarModel "
                    + " FROM CustomerReviews AS CR, Customers AS C, "
                    + " BikeMakes AS CM, BikeModels AS CMO With(NoLock) "
                    + " WHERE CR.ModelId = CMO.Id AND CMO.BikeMakeId = CM.ID "
                    + " AND CR.CustomerId = C.Id AND CR.IsActive = 1 AND CR.IsVerified = 1 AND CustomerId <> -1 AND C.IsFake <> 1";

                if (this.ReviewerId != "-1")
                {
                    sql = sql + " AND C.Id = @ReviewerId ";
                }

                switch (ApplyRetriveBy())
                {
                    case "MostHelpful":
                        sql = sql + " ORDER BY (CR.Liked - CR.Disliked) DESC ";
                        break;

                    case "MostRead":
                        sql = sql + " ORDER BY CR.Viewed DESC ";
                        break;

                    case "MostRecent":
                        sql = sql + " ORDER BY CR.Id DESC ";
                        break;

                    case "MostRated":
                        sql = sql + " ORDER BY OverallR DESC ";
                        break;
                }
            }

            Trace.Warn(sql);

            SqlParameter[] param = 
				{
					new SqlParameter("@ReviewerId", this.ReviewerId)
				};

            try
            {
                objCom.BindRepeaterReader(sql, rptUserReviews, param);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private string ApplyRetriveBy()
        {
            string strRes = "";

            switch (this.RetriveBy)
            {
                case "MostHelpful":
                    strRes = "MostHelpful";
                    break;
                case "MostRead":
                    strRes = "MostRead";
                    break;
                case "MostRecent":
                    strRes = "MostRecent";
                    break;
                case "MostRated":
                    strRes = "MostRated";
                    break;
            }

            return strRes;
        }

        public string GetRetriveType()
        {
            string strRes = "";

            switch (this.RetriveBy)
            {
                case "MostHelpful":
                    strRes = "Most Helpful";
                    break;
                case "MostRead":
                    strRes = "Most Read";
                    break;
                case "MostRecent":
                    strRes = "Most Recent";
                    break;
                case "MostRated":
                    strRes = "Most Rated";
                    break;
            }

            return strRes;
        }

        //function to remove last string of substring passed as argument of this function
        public string GetComments(string value, string reviewId)
        {
            string img = "", str = "";
            if (value != "" && ShowComment != false)
            {
                str = "<br>" + value.Substring(0, value.LastIndexOf(' ') == -1 ? value.Length : value.LastIndexOf(' ')).Replace("\n", "<br>");
                img = "<a style='text-decoration:none;' href='userreviews/reviewdetails.aspx?rid=" + reviewId + "'>"
                    + "  <img src='" + ImagingFunctions.GetRootImagePath() + "/images/icons/more3.jpg' border=0/></a><br>";

                return str + img;
            }

            else
                return "";
        }

        public string GetRateImage(double value)
        {
            string oneImg = "<img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/1.gif\">";
            string zeroImg = "<img src=\"http://imgd2.aeplcdn.com/0x0/bw/static/design15/old-images/d/0.gif\">";
            string halfImg = "<img src=\"http://imgd3.aeplcdn.com/0x0/bw/static/design15/old-images/d/half.gif\">";           

            StringBuilder sb = new StringBuilder();
            int absVal = (int)Math.Floor(value);

            int i;
            for (i = 0; i < absVal; i++)
                sb.Append(oneImg);

            if (value > absVal)
                sb.Append(halfImg);
            else
                i--;

            for (int j = 5; j > i + 1; j--)
                sb.Append(zeroImg);

            return sb.ToString();
        }
    }
}