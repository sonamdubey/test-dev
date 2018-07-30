using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;

namespace Bikewale.Controls
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 9/6/2012
    ///     Summary : Control will show the new bike manufacturers list
    /// </summary>
    public class BrowseBikeManufacturerMin : System.Web.UI.UserControl
    {
        protected DataList dltMakes;

        // Properties
        public string HeaderText { get; set; }
        public int NoOfColumns { get; set; }
        public string GridClass { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                LoadMakes();
            }
        }

        /// <summary>
        /// Function to get all new and used makes
        /// </summary>
        private void LoadMakes()
        {
            // Bind New Bike Makes            
            DataSet ds = null;
            
            try
            {
                //Memcache implemented by Ashish G. Kamble on 31 Oct 2013
                Memcache.BikeMakes objMakes = new Memcache.BikeMakes();
                ds = objMakes.GetNewBikeMakes();

                dltMakes.DataSource = ds;
                dltMakes.DataBind();
                dltMakes.RepeatColumns = NoOfColumns;
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, "Controls.LoadMakes");
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Controls.LoadMakes");
                
            }
        }   // End of LoadMakes method

    }   // End of class
}   // End of namespace