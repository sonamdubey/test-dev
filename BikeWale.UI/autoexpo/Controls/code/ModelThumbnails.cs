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
using System.Configuration;
using Bikewale.Common;

namespace AutoExpo
{
    public class ModelThumbnails : UserControl
    {
        private HttpContext objTrace = HttpContext.Current;

        protected Repeater rptThumbnail;       

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);            
        }


        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataSet dsThumbnail = new DataSet();

                //dsThumbnail = GetCarModels();
                //rptThumbnail.DataSource = dsThumbnail;
                //rptThumbnail.DataBind();
                BindMainRpt(GetCarModels());

            }
        }

        public DataSet GetCarModels()
        {
            string sql = "";

            Database db = new Database(); 
            DataSet ds = new DataSet();
            sql = "select Top 15 CI.id from Con_EditCms_Images CI  " +
                " INNER Join Con_EditCms_Basic CB on CB.Id=CI.BasicId where CB.CategoryId=9 and CI.IsActive=1 and CB.Id != '8474' AND YEAR(CI.LastUpdatedTime) >=2013 ";

            Trace.Warn(sql);
            SqlCommand cmd = new SqlCommand(sql);            
            try
            {
                ds = db.SelectAdaptQry(cmd);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }

        private void BindMainRpt(DataSet ds)
        {
            int pagesize = 4;
            DataTable dtMain = new DataTable();
            DataColumn dc = new DataColumn("Item", typeof(string));
            //DataRow dr; 
            dtMain.Columns.Add(dc);
            
            for (int i = 1; i <= ds.Tables[0].Rows.Count / pagesize; i++)
            {
                DataRow dr = dtMain.NewRow();
                dr["Item"] = i;              
                dtMain.Rows.Add(dr);
                if (i == ds.Tables[0].Rows.Count / pagesize)
                    if (ds.Tables[0].Rows.Count % pagesize > 0)
                    {
                        i++;
                        dr = dtMain.NewRow();
                        dr["Item"] = i;                      
                        dtMain.Rows.Add(dr);
                        break;
                    }
            }           

            rptThumbnail.DataSource = dtMain;
            rptThumbnail.DataBind();
        }

        protected DataSet GetImages(int PageNo)
        {
          
            int startNo = PageNo;
            int EndNo = 0;
            if (PageNo == 1)
            {
                startNo = PageNo;                
            }
            else
            {
                startNo = 4 * PageNo - 3;
            }
            EndNo = startNo + 3;

            string sql = "";

            Database db = new Database();
            DataSet ds = new DataSet();
            sql = "select * from (Select Top " + EndNo + " Row_Number() Over (Order By  DisplayDate Desc ) AS RowN, " +
                " CI.id,CI.HostUrl,CI.BasicId,CI.ImagePathThumbnail,CI.ImagePathLarge " +
                " from Con_EditCms_Images CI Join Con_EditCms_Basic CB on CB.Id=CI.BasicId" +
                " where CB.CategoryId=9 and YEAR(CI.LastUpdatedTime) >=2013 AND CI.IsActive=1) " +
                " AS TopRecords Where  RowN >= " + startNo + " AND RowN <= " + EndNo;

            Trace.Warn(sql);
            SqlCommand cmd = new SqlCommand(sql);
            try
            {
                ds = db.SelectAdaptQry(cmd);             
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
    }
}