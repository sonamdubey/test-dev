﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Used
{
    public class BikeSold : System.Web.UI.Page
    {
        protected BikesInBudget cbBikesInBudget;
        public string profileNo = "";

        string soldVersion = "", soldCity = "";
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


            string sql = "";

            if (CommonOpn.CheckIsDealerFromProfileNo(profileNo) == true)
            {
                sql = " select si.bikeversionid, ds.cityid as bikecityid, si.price from sellinquiries as si, dealers as ds where si.dealerid = ds.id and si.id = @id";
            }
            else
            {
                sql = " select csi.bikeversionid, csi.cityid as bikecityid, csi.price from classifiedindividualsellinquiries as csi  where csi.id = @id";
            }

            try
            {
                DbParameter[] param = new[] { DbFactory.GetDbParam("@id", DbType.Int32, CommonOpn.GetProfileNo(profileNo)) };

                using (IDataReader dr = MySqlDatabase.SelectQuery(sql, param))
                {
                    if (dr!=null && dr.Read())
                    {
                        //set the city id as the current city id
                        CommonOpn.SetCityId(dr["BikeCityId"].ToString());

                        soldVersion = dr["BikeVersionId"].ToString();
                        soldPrice = Convert.ToInt32(dr["Price"]);
                        soldCity = dr["BikeCityId"].ToString();
                        Trace.Warn("soldVersion : " + soldVersion + "soldPrice : " + soldPrice + "soldCity : " + soldCity);
                        dr.Close();
                    } 
                }

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
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
