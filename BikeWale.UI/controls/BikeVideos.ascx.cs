using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public  class BikeVideos : System.Web.UI.UserControl
    {
        protected string modelId = string.Empty;
       // protected bool isVideoExists = false;
        protected Repeater rptVideos;
        private int recordCount = 0;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        public string ModelId
        {
            get; set; 
        }

        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideoData();
        }

        /// <summary>
        /// created by:Prashant vishe
        /// BindVideoData() is used to retrieve video data of bikes..
        /// </summary>
        public void BindVideoData()
        {
            DataSet ds=null;
            try
            {
                ManageVideos mV = new ManageVideos();

                ds = mV.GetVideosData("", ModelId, true);

                if (ds != null) //if video exists for particular model...
                {
                    // isVideoExists = true;
                    rptVideos.DataSource = ds;
                    rptVideos.DataBind();

                    recordCount = rptVideos.Items.Count;
                }
            }
            catch (SqlException err)            
            {
                HttpContext.Current.Trace.Warn("sql err msg:", err.Message);
                ErrorClass objErr = new ErrorClass(err, "BindVideos.BindVideoData");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("err msg:", err.Message);
                ErrorClass objErr = new ErrorClass(err, "BindVideos.BindVideoData");
                objErr.SendMail();
            }
        } // end of BindVideoData()

    }//class
}//namespace