using Bikewale.Controls;
using System;
using System.Web;

namespace Bikewale.Used
{
    public class BikeSold : System.Web.UI.Page
    {
        protected BikesInBudget cbBikesInBudget;
        public string profileNo = "";

        //string soldVersion = "", soldCity = "";
        int soldPrice = 0;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProcessQS())
            {
                if (!IsPostBack)
                {
                    GetSoldBikeDetails();

                    cbBikesInBudget.Price = soldPrice;
                    cbBikesInBudget.ProfileNo = profileNo;
                }
            }
            else
            {
                Response.Redirect("/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
        void GetSoldBikeDetails()
        {
            throw new Exception("Method not used/commented");

            //string sql = "";

            //if (CommonOpn.CheckIsDealerFromProfileNo(profileNo) == true)
            //{
            //    sql = " SELECT Si.BikeVersionId, Ds.CityId AS BikeCityId, Si.Price "
            //        + " FROM SellInquiries AS Si, Dealers AS Ds With(NoLock) "
            //        + " WHERE Si.DealerId = Ds.Id AND Si.Id = @Id";
            //}
            //else
            //{
            //    sql = " SELECT Csi.BikeVersionId, Csi.CityId AS BikeCityId, Csi.Price "
            //        + " FROM ClassifiedIndividualSellInquiries AS Csi With(NoLock)"
            //        + " WHERE Csi.Id = @Id";
            //}
            //Trace.Warn(sql);
            //Database db = new Database();
            //SqlDataReader dr = null;

            //try
            //{
            //    SqlParameter[] param = { new SqlParameter("@Id", CommonOpn.GetProfileNo(profileNo)) };
            //    dr = db.SelectQry(sql, param);

            //    if (dr.Read())
            //    {
            //        //set the city id as the current city id
            //        CommonOpn.SetCityId(dr["BikeCityId"].ToString());

            //        soldVersion = dr["BikeVersionId"].ToString();
            //        soldPrice = Convert.ToInt32(dr["Price"]);
            //        soldCity = dr["BikeCityId"].ToString();
            //        Trace.Warn("soldVersion : " + soldVersion + "soldPrice : " + soldPrice + "soldCity : " + soldCity);
            //    }

            //}
            //catch (Exception err)
            //{
            //    Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();

            //    db.CloseConnection();
            //}
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 15 July 2014
        /// Summary : To validate query String
        /// </summary>
        /// <returns></returns>
        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (!String.IsNullOrEmpty(Request.QueryString["profile"]))
            {

                profileNo = Request.QueryString["profile"];

                string firstChar = profileNo.Substring(0, 1).ToUpper();

                if (!String.Equals(firstChar, "S") && !String.Equals(firstChar, "D"))
                {
                    isSuccess = false;
                }
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }
    }   // end of class
}   // End of namespace
