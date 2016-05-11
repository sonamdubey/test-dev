/****************************************************************************************
Made By - Deepak Tripathi
Edited By - 
Date - 03-Dec-2008
Purpose - This is report which shows the car purchase inquiries data. 
Process - 
		1. Fetch the data from database and write it into an xml file.
		2. Now at the time of data show read that xml file and show the data.
		3. There is only 1 xml file against 1 month-year.
		4. XML file location is /Webanalytics/PQ/filename.xml

Modified 1: Vaibhav K (03-May-2012)
            Get data directly from database rather than XML
            Tables (PQMatrix, PQMatrixUniquePerDay, PQMatrixUniquePerMonth)
****************************************************************************************/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.IO;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;
using System.Data.Common;

namespace BikeWaleOpr.Content
{
    public class BKPQMatrixNew : Page
    {
        protected DateControl selDate;
        protected Label lblMessage;
        protected Repeater rptDays, rptMakes, rptSum, rptUnique;
        protected DropDownList drpCities, drpState;
        protected TextBox txt_City, txt_Month, txt_Year, txt_State;
        protected RadioButtonList rbtnlOption;
        protected HtmlInputHidden hdn_rbtn;

        DataSet dsMakes = new DataSet();
        DataSet dsModels = new DataSet();

        protected Button butShow, butExcel;
        protected Panel pnlReport;

        CommonOpn objComm1 = new CommonOpn();

        public int newCarPrice = 300;
        public int financePrice = 200;

        public string month = "";
        public string year = "";
        public string city = "";

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
            butShow.Click += new System.EventHandler(butShow_Click);
            butExcel.Click += new System.EventHandler(SendToExcel);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (User.Identity.IsAuthenticated != true)
                Response.Redirect("../users/Login.aspx?ReturnUrl=../WebAnalytics/PQMatrix.aspx");

            if (!IsPostBack)
            {
                FillCities();
                FillStates();
            }
            //lblMessage.Text = " " + DateTime.Today.ToString("dd-MMM-yyyy");
        }

        //OnClick bind all PQ values for specific month and city
        void butShow_Click(object sender, EventArgs e)
        {
            BindValues();
        }

        //Fill Dropdown State
        void FillStates()
        {
            CommonOpn op = new CommonOpn();
            string sql = "select ID, Name from states where isdeleted <> 1 order by name ";
            try
            {
                op.FillDropDown(sql, drpState, "Name", "ID");
                drpState.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        //dropdown to fill the cities
        //Modified By : Sadhana Upadhyay on 30 Jan 2015
        void FillCities()
        {
            string sql = "";
            CommonOpn op = new CommonOpn();
            sql = "select ID, Name from cities   where isdeleted = 0 order by name";

            Trace.Warn(sql);

            try
            {
                op.FillDropDown(sql, drpCities, "Name", "ID");
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            drpCities.Items.Insert(0, new ListItem("--Select--", "-1"));
            drpCities.Items.Insert(1, new ListItem("All Cities", ""));
            drpCities.Items.Insert(2, new ListItem("Mumbai & Surrounding", "1,6,8,13,40,278,395")); // Mumbai and surround
            drpCities.Items.Insert(4, new ListItem("Delhi/NCR", "10,224,225,246,273,313"));
            drpCities.Items.Insert(5, new ListItem("Ahmedabad", "128"));
            drpCities.Items.Insert(6, new ListItem("Bangalore", "2"));
            drpCities.Items.Insert(7, new ListItem("Chandigarh", "244"));
            drpCities.Items.Insert(8, new ListItem("Chennai", "176"));
            drpCities.Items.Insert(9, new ListItem("Cochin", "9"));
            drpCities.Items.Insert(10, new ListItem("Hyderabad", "105"));
            drpCities.Items.Insert(11, new ListItem("Kolkata", "198"));
            drpCities.Items.Insert(12, new ListItem("Pune", "12"));

            drpCities.Items.Insert(13, new ListItem("--------------", "-1"));
        }

        //Modified : Vaibhav K (03-May-2012)
        //           Get data from table PQMatrix
        //           Get all price quotes for desired month & city(if selected)
        DataSet GetAllPQ(string city)
        {
            Trace.Warn("GetAllPQ");
            string sql = "";
            DateTime dtMonthYear = selDate.Value;
            DataSet ds = new DataSet();
            Database db = new Database();
            DbCommand cmd = DbFactory.GetDBCommand();

            sql = @" select count( distinct nbp.id) as CNT,vw.Make AS Make, 
                vw.Model,vw.modelid AS ModelId,vw.Version,vw.makeid AS MakeId,vw.VersionId, 
                monthname(nbp.requestdatetime)as Month , dayname(nbp.requestdatetime)as Day 
                from newbikepricequotes nbp inner join vwmmv vw on nbp.bikeversionid = vw.versionid 
                where  month(nbp.requestdatetime) = @month and year(nbp.requestdatetime) = @year ";

            if (rbtnlOption.SelectedValue == "2")
            {

                if (city == "")
                {
                    sql += "";//Record for all cities
                }
                else
                {
                    sql += " and nbp.cityid in  (" + db.GetInClauseValue(city, "CityIds", cmd) + ")";
                }
            }
            else
            {
                sql += " and nbp.cityid in (select id from cities where stateid=@state and isdeleted=0)";

            }

            sql += " group by vw.make,vw.model,vw.version,vw.makeid,vw.modelid,vw.versionid, month,day order by vw.makeid , vw.model ";

            try
            {
                cmd.CommandText = sql;
                cmd.Parameters.Add(DbFactory.GetDbParam("@month", DbParamTypeMapper.GetInstance[SqlDbType.Int], dtMonthYear.Month));
                cmd.Parameters.Add(DbFactory.GetDbParam("@year", DbParamTypeMapper.GetInstance[SqlDbType.Int], dtMonthYear.Year));
                cmd.Parameters.Add(DbFactory.GetDbParam("@state", DbParamTypeMapper.GetInstance[SqlDbType.Int], drpState.SelectedValue));

                ds = MySqlDatabase.SelectAdapterQuery(cmd);

                txt_Month.Text = Convert.ToString(dtMonthYear.Month);
                txt_Year.Text = Convert.ToString(dtMonthYear.Year);
                txt_City.Text = city;
                txt_State.Text = drpState.SelectedValue;


            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }

        //Modified :Vaibhav K (03-May-2012)
        //          Get data from table PQMatrixUniquePerDay & PQMatrixUniquePerMonth
        //          Get unique data perday and permonth
        //Modified By : Sadhana Upadhyay on 30 Jan 2915 to chnage sql query
        DataSet GetUniquePQ(string city)
        {
            Trace.Warn("GetUniquePQ");
            string sql = "";
            DateTime dtMonthYear = selDate.Value;
            DataSet ds = new DataSet();
            Database db = new Database();
            DbCommand cmd = DbFactory.GetDBCommand();

            sql = @" select cast(day(nbp.requestdatetime) as char) AS Day, 
                    count(distinct nbp.id)as CNT 
                    from newbikepricequotes nbp  
                    left join pricequotebuyingpreferences pqbp on pqbp.id = nbp.buyingpreference
                    where month(nbp.requestdatetime)=@month and 
                    year(nbp.requestdatetime) = @year ";


            if (rbtnlOption.SelectedValue == "2")
            {
                if (city == "")
                {
                    sql += "";//Record for all cities
                }
                else
                {
                    sql += " and nbp.cityid in  (" + db.GetInClauseValue(city, "CityIds", cmd) + ")";
                }
            }
            else
            {
                sql += " and nbp.cityid in (select id from cities where stateid=@state and isdeleted=0)";

            }

            sql += " group by cast(dayname(nbp.requestdatetime) as char(3) ) ";
            //+", CASE PQBP.IsForwarded WHEN 1 THEN 't' ELSE 'f' END ";
            //sql += " UNION"
            //    //To cast month and year in the form:- Month-yr (eg. May-12) as Varchar
            //    + " SELECT left(DateName(month ,DateAdd(month,Month,0)-1),3) + '-' + cast(cast(Year%100 as char) as varchar(10)) AS Day,"
            //    + " CNT, CASE ForwardedLead WHEN 1 THEN 't' ELSE 'f' END AS ForwardedLead"
            //    + " FROM PQMatrixUniquePerMonth WITH (NOLOCK)"
            //    + " WHERE Month=@Month AND Year=@Year";

            sql += @" union 
                select concat(cast(monthname(nbp.requestdatetime) as char(3)) ,'-' ,year(nbp.requestdatetime)) as Day,Count (distinct nbp.id)as CNT 
                from newbikepricequotes nbp 
                where month(nbp.requestdatetime)=@month and
                year(nbp.requestdatetime) = @year ";

            if (rbtnlOption.SelectedValue == "2")
            {

                if (city == "")
                {
                    sql += "";//Record for all cities
                }
                else
                {
                    sql += " and nbp.cityid in  (" + db.GetInClauseValue(city, "Citys", cmd) + ")";
                }
            }
            else
            {
                sql += " and nbp.cityid in (select id from cities where stateid=@state and isdeleted=0)";

            }
            sql += "group by concat(cast(monthname(nbp.requestdatetime) as char(3)) ,'-' ,year(nbp.requestdatetime))  ";
            Trace.Warn("unique data qry: " + sql);

            try
            {

                cmd.CommandText = sql;
                cmd.Parameters.Add(DbFactory.GetDbParam("@month", DbParamTypeMapper.GetInstance[SqlDbType.Int], dtMonthYear.Month));
                cmd.Parameters.Add(DbFactory.GetDbParam("@year", DbParamTypeMapper.GetInstance[SqlDbType.Int], dtMonthYear.Year));
                cmd.Parameters.Add(DbFactory.GetDbParam("@state", DbParamTypeMapper.GetInstance[SqlDbType.Int], drpState.SelectedValue));

                ds = MySqlDatabase.SelectAdapterQuery(cmd);
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }

        //Bind Repeater
        void BindValues()
        {
            Trace.Warn("Bind Values");
            DataSet dsDays = new DataSet();
            DataSet dsData = new DataSet();

            DataTable dt = dsDays.Tables.Add();
            dt.Columns.Add("Value", typeof(int));
            dt.Columns.Add("Text", typeof(string));
            DataRow dr;

            DateTime dtMon = selDate.Value;
            int days = DateTime.DaysInMonth(dtMon.Year, dtMon.Month);
            for (int i = 1; i <= days; i++)
            {
                dr = dt.NewRow();

                dr["Value"] = i;
                dr["Text"] = (new DateTime(dtMon.Year, dtMon.Month, i)).DayOfWeek.ToString().Substring(0, 3);

                dt.Rows.Add(dr);
            }

            rptDays.DataSource = dsDays;
            rptDays.DataBind();

            Trace.Warn("Header Binded");

            //Now Prepare two datasets one for makes and one for models. Those contains whole data
            string totalMake = "";
            string totalMF = "";
            string currentMake = "";

            string totalModel = "";
            string totalMOF = "";
            string currentModel = "";

            int today = DateTime.Today.Day;
            double proj = 0;
            double projF = 0;

            DataTable dtMake = dsMakes.Tables.Add();
            DataTable dtModel = dsModels.Tables.Add();
            DataRow drMake;
            DataRow drModel;

            dtMake.Columns.Add("Make", typeof(string));
            dtMake.Columns.Add("MakeId", typeof(string));
            dtMake.Columns.Add("Total", typeof(double));
            dtMake.Columns.Add("TotalF", typeof(double));
            dtMake.Columns.Add("Proj", typeof(double));
            dtMake.Columns.Add("ProjF", typeof(double));

            dtModel.Columns.Add("MakeId", typeof(string));
            dtModel.Columns.Add("Model", typeof(string));
            dtModel.Columns.Add("ModelId", typeof(string));
            dtModel.Columns.Add("Total", typeof(double));
            dtModel.Columns.Add("TotalF", typeof(double));
            dtModel.Columns.Add("Proj", typeof(double));
            dtModel.Columns.Add("ProjF", typeof(double));

            for (int i = 1; i <= days; i++)
            {
                dtMake.Columns.Add("Day" + i, typeof(double));
                dtMake.Columns.Add("DayF" + i, typeof(double));

                dtModel.Columns.Add("Day" + i, typeof(double));
                dtModel.Columns.Add("DayF" + i, typeof(double));
            }

            try
            {
                //Get Data to Bind Repeater
                Trace.Warn("Reading Data, city: " + drpCities.SelectedItem.Value);
                if (drpCities.SelectedIndex > 1)
                {
                    dsData = GetAllPQ(drpCities.SelectedItem.Value);//Get PQ for selected city
                }
                else
                {
                    dsData = GetAllPQ("");//Get PQ for all cities
                }

                if (dsData.Tables[0].Rows.Count > 0)
                {
                    //If dataset contains record then only show panel data
                    pnlReport.Visible = true;
                    Trace.Warn("Pannel Visible");

                    foreach (DataRow rowPQ in dsData.Tables[0].Rows)
                    {
                        if (currentMake != rowPQ["MakeId"].ToString())
                        {
                            drMake = dtMake.NewRow();

                            //Calculation for make
                            drMake["Make"] = rowPQ["Make"].ToString();
                            drMake["MakeId"] = rowPQ["MakeId"].ToString();
                            currentMake = drMake["MakeId"].ToString();

                            //Trace.Warn("currentMake=" + currentMake);

                            totalMake = (dsData.Tables[0].Compute("Sum(Cnt)", "MakeId = '" + drMake["MakeId"].ToString() + "' ")).ToString();
                            totalMF = dsData.Tables[0].Compute("Sum(Cnt)", "MakeId = '" + drMake["MakeId"].ToString() + "' ").ToString();
                            //totalMF = dsData.Tables[0].Compute("Sum(Cnt)", "MakeId = '" + drMake["MakeId"].ToString() + "'  ").ToString();

                            drMake["Total"] = Convert.ToInt32(totalMake == "" ? "0" : totalMake);
                            drMake["TotalF"] = Convert.ToInt32(totalMF == "" ? "0" : totalMF);

                            // Trace.Warn("currentMake TOTAL=" + drMake["Total"] + "/" + drMake["TotalF"]);

                            for (int i = 1; i <= days; i++)
                            {
                                totalMake = (dsData.Tables[0].Compute("Sum(Cnt)", "MakeId = '" + drMake["MakeId"].ToString() + "' AND Day = '" + i + "'")).ToString();
                                totalMF = dsData.Tables[0].Compute("Sum(Cnt)", "MakeId = '" + drMake["MakeId"].ToString() + "' AND Day = '" + i + "' ").ToString();
                                //totalMF = dsData.Tables[0].Compute("Sum(Cnt)", "MakeId = '" + drMake["MakeId"].ToString() + "' AND Day = '" + i + "' ").ToString();

                                drMake["Day" + i] = Convert.ToInt32(totalMake == "" ? "0" : totalMake);
                                drMake["DayF" + i] = Convert.ToInt32(totalMF == "" ? "0" : totalMF);

                                //Trace.Warn("currentMake=" +  drMake["Day" + i]);
                            }



                            //Calculate Projection
                            if (DateTime.Today.Month == Convert.ToDateTime(selDate.Value).Month)
                            {
                                switch (today)
                                {
                                    case 1:
                                        proj = 0;
                                        projF = 0;
                                        break;

                                    case 2:
                                        proj = (Convert.ToInt32((drMake["Day" + (today - 1)]))) * (days - today);
                                        projF = (Convert.ToInt32((drMake["DayF" + (today - 1)]))) * (days - today);
                                        break;

                                    case 3:
                                        proj = ((Convert.ToInt32(drMake["Day" + (today - 1)]) + Convert.ToInt32(drMake["Day" + (today - 2)])) / 2) * (days - today);
                                        projF = ((Convert.ToInt32(drMake["DayF" + (today - 1)]) + Convert.ToInt32(drMake["DayF" + (today - 2)])) / 2) * (days - today);
                                        break;

                                    default:
                                        proj = ((Convert.ToInt32(drMake["Day" + (today - 1)]) + Convert.ToInt32(drMake["Day" + (today - 2)]) + Convert.ToInt32(drMake["Day" + (today - 3)])) / 3) * (days - today);
                                        projF = ((Convert.ToInt32(drMake["DayF" + (today - 1)]) + Convert.ToInt32(drMake["DayF" + (today - 2)]) + Convert.ToInt32(drMake["DayF" + (today - 3)])) / 3) * (days - today);
                                        break;
                                }
                            }

                            drMake["Proj"] = proj;
                            drMake["ProjF"] = projF;

                            dtMake.Rows.Add(drMake);
                        }


                        if (currentModel != rowPQ["ModelId"].ToString())
                        {
                            drModel = dtModel.NewRow();

                            //Calculation for Model
                            drModel["MakeId"] = rowPQ["MakeId"].ToString();
                            drModel["Model"] = rowPQ["Model"].ToString();
                            drModel["ModelId"] = rowPQ["ModelId"].ToString();
                            currentModel = drModel["ModelId"].ToString();

                            //Trace.Warn("currentModel=" + currentModel);

                            totalModel = (dsData.Tables[0].Compute("Sum(Cnt)", "ModelId = '" + drModel["ModelId"].ToString() + "' ")).ToString();
                            totalMOF = dsData.Tables[0].Compute("Sum(Cnt)", "ModelId = '" + drModel["ModelId"].ToString() + "' ").ToString();
                            //totalMOF = dsData.Tables[0].Compute("Sum(Cnt)", "ModelId = '" + drModel["ModelId"].ToString() + "' ").ToString();

                            drModel["Total"] = Convert.ToInt32(totalModel == "" ? "0" : totalModel);
                            drModel["TotalF"] = Convert.ToInt32(totalMOF == "" ? "0" : totalMOF);

                            //Trace.Warn("currentModel total=" + drModel["Total"]);

                            for (int i = 1; i <= days; i++)
                            {
                                totalModel = (dsData.Tables[0].Compute("Sum(Cnt)", "ModelId = '" + drModel["ModelId"].ToString() + "' AND Day = '" + i + "'")).ToString();
                                totalMOF = dsData.Tables[0].Compute("Sum(Cnt)", "ModelId = '" + drModel["ModelId"].ToString() + "' AND Day = '" + i + "' ").ToString();
                                //totalMOF = dsData.Tables[0].Compute("Sum(Cnt)", "ModelId = '" + drModel["ModelId"].ToString() + "' AND Day = '" + i + "' ").ToString();

                                drModel["Day" + i] = Convert.ToInt32(totalModel == "" ? "0" : totalModel);
                                drModel["DayF" + i] = Convert.ToInt32(totalMOF == "" ? "0" : totalMOF);

                                //Trace.Warn("currentModel=" +  drModel["Day" + i]);
                            }



                            //Calculate Projection
                            if (DateTime.Today.Month == Convert.ToDateTime(selDate.Value).Month)
                            {
                                switch (today)
                                {
                                    case 1:
                                        proj = 0;
                                        projF = 0;
                                        break;

                                    case 2:
                                        proj = (Convert.ToInt32((drModel["Day" + (today - 1)]))) * (days - today);
                                        projF = (Convert.ToInt32((drModel["DayF" + (today - 1)]))) * (days - today);
                                        break;

                                    case 3:
                                        proj = ((Convert.ToInt32(drModel["Day" + (today - 1)]) + Convert.ToInt32(drModel["Day" + (today - 2)])) / 2) * (days - today);
                                        projF = ((Convert.ToInt32(drModel["DayF" + (today - 1)]) + Convert.ToInt32(drModel["DayF" + (today - 2)])) / 2) * (days - today);
                                        break;

                                    default:
                                        proj = ((Convert.ToInt32(drModel["Day" + (today - 1)]) + Convert.ToInt32(drModel["Day" + (today - 2)]) + Convert.ToInt32(drModel["Day" + (today - 3)])) / 3) * (days - today);
                                        projF = ((Convert.ToInt32(drModel["DayF" + (today - 1)]) + Convert.ToInt32(drModel["DayF" + (today - 2)]) + Convert.ToInt32(drModel["DayF" + (today - 3)])) / 3) * (days - today);
                                        break;
                                }
                            }

                            drModel["Proj"] = proj;
                            drModel["ProjF"] = projF;

                            dtModel.Rows.Add(drModel);
                        }
                    }
                    Trace.Warn("Data Manipulation Completed");

                    DataView dv = dtMake.DefaultView;
                    dv.Sort = "Total Desc";
                    rptMakes.DataSource = dv;
                    rptMakes.DataBind();

                    GetTotal();
                    Trace.Warn("geting unique");
                    GetUnique();
                    Trace.Warn("geting unique complete");
                    Trace.Warn("Data Binding make Completed");
                }
                else
                {
                    pnlReport.Visible = false;
                    Trace.Warn("Pannel Not Visible");
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public DataSet GetMakesData(string makeId)
        {
            Trace.Warn("GetMakesData");
            //initialize a dataset with the number of rows as the number of days
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            DataRow[] rows;

            dt.Columns.Add("Value", typeof(string));

            DateTime dtMon = selDate.Value;
            int days = DateTime.DaysInMonth(dtMon.Year, dtMon.Month);

            DataRow dr;

            rows = dsMakes.Tables[0].Select("MakeId = '" + makeId + "'", "");
            foreach (DataRow rowPQ in rows)
            {

                dr = dt.NewRow();
                dr["Value"] = ((rowPQ["Total"].ToString() == "0" && rowPQ["TotalF"].ToString() == "0") ? "" : rowPQ["Total"].ToString());   //rowPQ["TotalF"] + "/" + rowPQ["Total"].ToString());
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Value"] = ((rowPQ["Proj"].ToString() == "0" && rowPQ["ProjF"].ToString() == "0") ? "" : rowPQ["Proj"].ToString());  // rowPQ["ProjF"] + "/" + rowPQ["Proj"].ToString());
                dt.Rows.Add(dr);

                for (int i = 1; i <= days; i++)
                {
                    dr = dt.NewRow();
                    dr["Value"] = ((rowPQ["Day" + i].ToString() == "0" && rowPQ["DayF" + i].ToString() == "0") ? "" : rowPQ["Day" + i].ToString()); //rowPQ["DayF" + i] + "/" + rowPQ["Day" + i].ToString());
                    dt.Rows.Add(dr);
                }

                dr = dt.NewRow();
                dr["Value"] = ((rowPQ["Total"].ToString() == "0" && rowPQ["TotalF"].ToString() == "0") ? "" : rowPQ["Total"].ToString());   //rowPQ["TotalF"] + "/" + rowPQ["Total"].ToString());
                dt.Rows.Add(dr);
            }

            return ds;
        }

        public DataSet GetModelsData(string modelId)
        {
            //Trace.Warn("GetModelsData");
            //initialize a dataset with the number of rows as the number of days
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            DataRow[] rows;

            dt.Columns.Add("Value", typeof(string));

            DateTime dtMon = selDate.Value;
            int days = DateTime.DaysInMonth(dtMon.Year, dtMon.Month);

            DataRow dr;

            rows = dsModels.Tables[0].Select("ModelId = '" + modelId + "'", "");
            foreach (DataRow rowPQ in rows)
            {
                dr = dt.NewRow();
                dr["Value"] = ((rowPQ["Total"].ToString() == "0" && rowPQ["TotalF"].ToString() == "0") ? "" : rowPQ["Total"].ToString());   //rowPQ["TotalF"] + "/" + rowPQ["Total"].ToString());
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Value"] = ((rowPQ["Proj"].ToString() == "0" && rowPQ["ProjF"].ToString() == "0") ? "" : rowPQ["Proj"].ToString());  //rowPQ["ProjF"] + "/" + rowPQ["Proj"].ToString());
                dt.Rows.Add(dr);

                for (int i = 1; i <= days; i++)
                {
                    dr = dt.NewRow();
                    dr["Value"] = ((rowPQ["Day" + i].ToString() == "0" && rowPQ["DayF" + i].ToString() == "0") ? "" : rowPQ["Day" + i].ToString()); //rowPQ["DayF" + i] + "/" + rowPQ["Day" + i].ToString());
                    dt.Rows.Add(dr);
                }

                dr = dt.NewRow();
                dr["Value"] = ((rowPQ["Total"].ToString() == "0" && rowPQ["TotalF"].ToString() == "0") ? "" : rowPQ["Total"].ToString());   //rowPQ["TotalF"] + "/" + rowPQ["Total"].ToString());
                dt.Rows.Add(dr);
            }

            return ds;
        }

        public DataView GetModels(string makeId)
        {
            Trace.Warn("GetModels");
            //initialize a dataset with the number of rows as the number of days
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            DataRow[] rows;

            dt.Columns.Add("ModelId", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("MakeId_Model", typeof(string));
            dt.Columns.Add("total", typeof(double));

            DataRow dr;

            rows = dsModels.Tables[0].Select("MakeId = '" + makeId + "'", "");
            foreach (DataRow rowPQ in rows)
            {

                dr = dt.NewRow();
                dr["ModelId"] = rowPQ["ModelId"].ToString();
                dr["Model"] = rowPQ["Model"].ToString();
                dr["MakeId_Model"] = rowPQ["MakeId"].ToString();
                dr["total"] = rowPQ["total"].ToString();
                dt.Rows.Add(dr);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "Total Desc";

            return dv;
        }

        void GetTotal()
        {
            Trace.Warn("GetTotal");
            string total = "";
            string totalF = "";
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();

            dt.Columns.Add("Value", typeof(string));

            DateTime dtMon = selDate.Value;
            int days = DateTime.DaysInMonth(dtMon.Year, dtMon.Month);

            DataRow dr;

            Trace.Warn("Getting Total");
            dr = dt.NewRow();
            total = dsMakes.Tables[0].Compute("Sum(Total)", "").ToString();
            totalF = dsMakes.Tables[0].Compute("Sum(TotalF)", "").ToString();

            dr["Value"] = ((total == "0" && totalF == "0") ? "" : total); //totalF + "/" + total);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            total = dsMakes.Tables[0].Compute("Sum(Proj)", "").ToString();
            totalF = dsMakes.Tables[0].Compute("Sum(ProjF)", "").ToString();
            dr["Value"] = ((total == "0" && totalF == "0") ? "" : total);   //totalF + "/" + total);
            dt.Rows.Add(dr);

            for (int i = 1; i <= days; i++)
            {
                dr = dt.NewRow();
                total = dsMakes.Tables[0].Compute("Sum(Day" + i + ")", "").ToString();
                totalF = dsMakes.Tables[0].Compute("Sum(DayF" + i + ")", "").ToString();

                dr["Value"] = ((total == "0" && totalF == "0") ? "" : total);   //totalF + "/" + total);
                dt.Rows.Add(dr);
            }

            dr = dt.NewRow();
            total = dsMakes.Tables[0].Compute("Sum(Total)", "").ToString();
            totalF = dsMakes.Tables[0].Compute("Sum(TotalF)", "").ToString();

            dr["Value"] = ((total == "0" && totalF == "0") ? "" : total);   //totalF + "/" + total);
            dt.Rows.Add(dr);

            rptSum.DataSource = ds;
            rptSum.DataBind();
        }

        void GetUnique()
        {
            Trace.Warn("GetUnique");
            string total = "";
            string totalF = "";
            DataSet ds = new DataSet();

            Trace.Warn("Getting Unique, city: " + drpCities.SelectedItem.Value);
            DataSet dsUnique = new DataSet();
            if (drpCities.SelectedIndex > 1)
            {
                dsUnique = GetUniquePQ(drpCities.SelectedItem.Value);//Get unique PQ per day & month for selected city
            }
            else
            {
                dsUnique = GetUniquePQ("");//Get unique PQ per day & month for all cities
            }

            Trace.Warn("Count..." + dsUnique.Tables[0].Rows.Count.ToString() + " ABCDE=" + selDate.Value.ToString("MMM-yyyy"));

            if (dsUnique.Tables.Count > 0)
            {
                Trace.Warn("DataSet fetched");
                DataTable dt = ds.Tables.Add();

                dt.Columns.Add("Value", typeof(string));

                DateTime dtMon = selDate.Value;
                int days = DateTime.DaysInMonth(dtMon.Year, dtMon.Month);

                DataRow dr;

                //Manipulate Data	
                Trace.Warn("All Total");
                dr = dt.NewRow();
                total = dsUnique.Tables[0].Compute("Sum(CNT)", "Day='" + selDate.Value.ToString("MMM-yyyy") + "'").ToString();
                totalF = dsUnique.Tables[0].Compute("Sum(CNT)", "Day='" + selDate.Value.ToString("MMM-yyyy") + "' ").ToString();

                //To get record as forwardedLeads / totalLeads
                dr["Value"] = ((total == "0" && totalF == "0") ? "" : total);   //totalF + "/" + total);
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Value"] = "NA";
                dt.Rows.Add(dr);

                DataRow[] tmpCount = dsUnique.Tables[0].Select("Day='" + selDate.Value.ToString("MMM-yyyy") + "'");

                Trace.Warn("PerDay Total" + total + "-" + totalF + "-" + tmpCount.Length);
                for (int i = 1; i <= days; i++)
                {
                    dr = dt.NewRow();
                    total = dsUnique.Tables[0].Compute("Sum(CNT)", "Day='" + i + "'").ToString();
                    totalF = dsUnique.Tables[0].Compute("Sum(CNT)", "Day='" + i + "' ").ToString();
                    //To get record as forwardedLeads / totalLeads
                    dr["Value"] = ((total == "" && totalF == "") ? "" : total); //totalF + "/" + total);
                    Trace.Warn("unq val: " + dr["Value"].ToString());
                    dt.Rows.Add(dr);
                }

                dr = dt.NewRow();
                total = dsUnique.Tables[0].Compute("Sum(CNT)", "Day='" + selDate.Value.ToString("MMM-yyyy") + "'").ToString();
                totalF = dsUnique.Tables[0].Compute("Sum(CNT)", "Day='" + selDate.Value.ToString("MMM-yyyy") + "' ").ToString();
                //To get record as forwardedLeads / totalLeads
                dr["Value"] = ((total == "0" && totalF == "0") ? "" : total);   //totalF + "/" + total);
                dt.Rows.Add(dr);

                rptUnique.DataSource = ds;
                rptUnique.DataBind();
            }
        }

        void SendToExcel(object sender, EventArgs e)
        {
            Response.AddHeader("content-disposition", "attachment; filename=PriceQuoteMatrix" + selDate.Value.ToString("MMMyyyy") + ".xls");

            Response.ContentType = "application/vnd.ms-excel";

            this.EnableViewState = false;

            Response.Charset = String.Empty;

            System.IO.StringWriter myTextWriter = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter myHtmlTextWriter = new System.Web.UI.HtmlTextWriter(myTextWriter);

            pnlReport.RenderControl(myHtmlTextWriter);

            Response.Write(myTextWriter.ToString());

            Response.End();
        }
    }
}