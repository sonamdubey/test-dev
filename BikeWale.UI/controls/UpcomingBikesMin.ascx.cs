using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using Bikewale.CoreDAL;


namespace Bikewale.Controls
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 1/8/2012
    ///     Class will show the upcoming bikes list
    /// </summary>
    public partial class UpcomingBikesMin : System.Web.UI.UserControl
    {
        protected Repeater rptUpcomingBikes;
        protected HtmlGenericControl divControl;
        private string _topRecords = "4";

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        private string _width = "grid_2";

        public string ControlWidth
        {
            get { return _width; }
            set { _width = value; }
        }

        private string _imageWidth = "136px;";

        public string ImageWidth
        {
            get { return _imageWidth; }
            set { _imageWidth = value; }
        }

        public string ModelId { get; set; }
        public string MakeId { get; set; }
        public bool Corousal { get; set; }
        public string WhereClause { get; set; }
        public string HeaderText { get; set; }
        public string SeriesId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if (!String.IsNullOrEmpty(MakeId) || !String.IsNullOrEmpty(ModelId) || !String.IsNullOrEmpty(SeriesId))
                    FetchUpcomingBikes();
            }
        }

        protected void FetchUpcomingBikes()
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getupcomingbikemin"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
                    //cmd.Parameters.Add("@ControlWidth", SqlDbType.VarChar, 10).Value = ControlWidth;
                    //cmd.Parameters.Add("@FetchAllRecords", SqlDbType.Bit).Value = Corousal;
                    //if (!String.IsNullOrEmpty(ModelId)) { cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = ModelId; }
                    //if(!String.IsNullOrEmpty(MakeId)) { cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = MakeId; }
                    //if(!String.IsNullOrEmpty(SeriesId)) { cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = SeriesId; }

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], TopRecords));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_controlwidth", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 10, ControlWidth));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fetchallrecords", DbParamTypeMapper.GetInstance[SqlDbType.Bit], Corousal));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (!String.IsNullOrEmpty(ModelId)) ? ModelId : null));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (!String.IsNullOrEmpty(MakeId)) ? MakeId : null));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (!String.IsNullOrEmpty(SeriesId)) ? SeriesId : null));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                        {
                            divControl.Attributes.Remove("class");
                            DataTable dt = ds.Tables[0];

                            rptUpcomingBikes.DataSource = dt;
                            rptUpcomingBikes.DataBind();
                        }
                        else
                            divControl.Attributes.Add("class", "hide");
                    }

                }
            }
            catch (SqlException exSql)
            {
                Trace.Warn("upcoming bikes FetchUpcomingBikes sqlex: ", exSql.Message);
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("upcoming bikes FetchUpcomingBikes Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // end of FetchUpcomingBikes method

        /// <summary>
        /// Retrun Topic name if the topic name lenght is greater than 30 then it should be substring and showing small string for that
        /// </summary>
        /// <param name="Topic"></param>
        /// <returns></returns>
        protected string FormatedTopic(string Topic)
        {
            string result = Topic.Length > 125 ? Topic.Substring(0, Topic.IndexOf(" ", 110)) + "..." : Topic;
            return result;
        }

        /// <summary>
        ///     Fuction will for the redirect url
        /// </summary>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string GetLink(string makeMaskingName, string ModelMaskingName)
        {
            return "/" + makeMaskingName + "-bikes/" + ModelMaskingName + "/";
        }

    }   // End of class
}   // End of namespace