using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;

namespace Bikewale.New
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 14/8/2012
    ///     Class to show the new bikes search results
    /// </summary>
    public class Search_old : Page
    {
        protected Repeater rptMakes; /*rptListings,*/
        protected string query_string = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Warn("QueryString: " + Request.ServerVariables["QUERY_STRING"].ToString());

            if ( !String.IsNullOrEmpty(Request.ServerVariables["QUERY_STRING"].ToString()) )
            {
                Response.Redirect("/new/search.aspx",false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            else
            {
                FillMakes();
            }
        }

        /// <summary>
        /// This function cache makes DataSet for 2 hours.
        /// </summary>
        void FillMakes()
        {
            DataSet dsMakes = null;

            try
            {
                // Check if MakesDataSet available in cache
                dsMakes = (DataSet)Cache.Get("MakesDataSet");

                // If not available in cache, retrive makes and cache it
                if (dsMakes == null)
                {
                    DataSet ds = null;
                    BWCommon objCommon = new BWCommon();
                    ds = objCommon.GetNewMakes();

                    if (ds != null)
                    {
                        rptMakes.DataSource = ds;
                    }

                    // Cache the DataSet for next 30 minutes.  After every 30 minutes this DataSet will get refreshed.
                    Cache.Insert("MakesDataSet", ds, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
                }
                else// Bind data from cache
                {
                    rptMakes.DataSource = dsMakes;
                }

                rptMakes.DataBind();
            }
            catch (Exception ex)
            {
                Trace.Warn("err bind makes : ", ex.Message);
            }
        }
    }
}