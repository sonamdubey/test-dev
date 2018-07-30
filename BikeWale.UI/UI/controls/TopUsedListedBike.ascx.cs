using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data.Common;
using MySql.CoreDAL;

namespace Bikewale.Controls
{
    public class TopUsedListedBike : System.Web.UI.UserControl
    {
        protected HtmlContainerControl noCarsMessage, topUsedCarItems;

        protected Repeater rptListings;
        private string _topRecords;

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        bool _DisplayTwoColumn = false;
        public bool DisplayTwoColumn
        {
            get { return _DisplayTwoColumn; }
            set { _DisplayTwoColumn = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindListedBike();
        }//pageload

        private void BindListedBike()
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("geusedbikelistings"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, TopRecords));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr!=null)
                        {
                            rptListings.DataSource = dr;
                            rptListings.DataBind();
                            dr.Close();
                        }
                        else
                        {
                            topUsedCarItems.Visible = false;
                            noCarsMessage.Visible = true;
                        } 
                    } 
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }      
    }//class
}//namespace