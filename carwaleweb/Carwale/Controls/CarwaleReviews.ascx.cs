using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;
using Carwale.UI.Common;
using Carwale.Notifications;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Controls
{
    public class CarwaleReviews : UserControl
    {
        protected HtmlGenericControl ctrlNews, divMoreNews, ctrlNewsNotFound;
        protected Repeater rptUserReviews;
        private string reviewerId = "-1";

        //this shows the number of reviews to be shown. default is set to 5
        public int ReviewCount
        {
            get
            {
                if (ViewState["ReviewCount"] != null && ViewState["ReviewCount"].ToString() != "")
                    return Convert.ToInt32(ViewState["ReviewCount"]);
                else
                    return 0;
            }
            set
            {
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
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            Trace.Warn("reviewerId" + reviewerId);

            BindReviews();
        } // Page_Load

        //This function gets the list of the top 5 news for the

        public void BindReviews()
        {    
            CommonOpn objCom = new CommonOpn();
            DataSet ds = new DataSet();

            if (!ReviewCount.Equals(0))
            {
                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("BindAllTypeOfReviews_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewerId", DbType.Int32, this.ReviewerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewCond", DbType.String, 20, ApplyRetriveBy()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewCount", DbType.Int32, ReviewCount));
                        ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                    }
                    objCom.BindRepeaterReaderDataSet(ds, rptUserReviews);
                }
                catch (Exception err)
                {
                    Trace.Warn(err.Message + err.Source);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
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
                img = "<a style='text-decoration:none;' href='research/userreviews/reviewdetails.aspx?rid=" + reviewId + "'>"
                    + "  <img src='" + ImagingFunctions.GetRootImagePath() + "/images/icons/more3.jpg' border=0/></a><br>";

                return str + img;
            }

            else
                return "";
        }
    }
}