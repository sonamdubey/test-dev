using Bikewale.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
                    return (ViewState["ReviewCount"].ToString());
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
            uint _reviewCount = 0;

            try
            {
                if (!string.IsNullOrEmpty(ReviewCount) && uint.TryParse(ReviewCount, out _reviewCount))
                {
                    sql = @" select  cr.id as reviewid, title, overallr, 
                        concat(cm.name,'-',cmo.name) as modelname,
                        c.name as customername, c.id as customerid, 
                        cmo.maskingname as modelmaskingname,
                        substring(cr.comments,0,cast(floor(length(cr.comments)*0.4) as unsigned int)) as  shortcomment, 
                        cm.name as makename, cm.maskingname as makemaskingname, cmo.name carmodel 
                     from customerreviews as cr, customers as c,
                     bikemakes as cm, bikemodels as cmo  
                     where cr.modelid = cmo.id and cmo.bikemakeid = cm.id 
                     and cr.customerid = c.id and cr.isactive = 1 and cr.isverified = 1 and customerid <> -1 and c.isfake <> 1";

                    if (this.ReviewerId != "-1")
                    {
                        sql = sql + " and c.id = @v_reviewerid ";
                    }

                    switch (ApplyRetriveBy())
                    {
                        case "MostHelpful":
                            sql = sql + " order by (cr.liked - cr.disliked) desc ";
                            break;

                        case "MostRead":
                            sql = sql + " order by cr.viewed desc ";
                            break;

                        case "MostRecent":
                            sql = sql + " order by cr.id desc ";
                            break;

                        case "MostRated":
                            sql = sql + " order by overallr desc ";
                            break;
                    }

                    sql = sql + "  limit @v_reviewcount";

                    Trace.Warn(sql);

                    DbParameter[] param = new[] { DbFactory.GetDbParam("@v_reviewerid", DbType.Int32,this.ReviewerId ),
                                          DbFactory.GetDbParam("@v_reviewcount", DbType.Int32,_reviewCount )  };

                    objCom.BindRepeaterReader(sql, rptUserReviews, param);
                }

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
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
            string oneImg = "<img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/1.gif\">";
            string zeroImg = "<img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/0.gif\">";
            string halfImg = "<img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/half.gif\">";

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