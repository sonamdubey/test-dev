using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    public class ListNewBikeDealersByCity_New : Page
    {
        protected Repeater rptCity;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected MakeModelVersion objMMV;

        public uint makeId;
        public string cityArr = string.Empty, stateMaskingName = string.Empty, stateName = string.Empty;
        public int citesCount = 0;
        public uint DealerCount = 0;
        public uint stateId = 0;
        public DealerStateCities dealerCity;

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
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (ProcessQS())
            {
                if (!IsPostBack)
                {
                    objMMV = new MakeModelVersion();
                    objMMV.GetMakeDetails(makeId.ToString());

                    BindControl();
                    BindCities();
                }
            }
        }

        private void BindControl()
        {
            string sql = "";
            Database db = new Database();

            sql = " SELECT  C.Id AS CityId,c.MaskingName AS CityMaskingName, "
                + " C.Name AS City, COUNT(DNC.Id) AS TotalBranches, "
                + " S.Name AS [State], S.ID AS StateId, "
                + " ROW_NUMBER() Over(Partition By StateId Order by StateId) AS StateRank "
                + " FROM Dealer_NewBike AS DNC, BWCities AS C, States AS S With(NoLock) "
                + " WHERE DNC.CityId = C.Id AND C.StateId = S.ID AND DNC.IsActive = 1 "
                + " AND C.IsDeleted = 0 AND DNC.MakeId = @MakeId "
                + " GROUP By C.Id, C.Name, S.Name, S.ID, StateId,C.MaskingName "
                + " Order By [State], CityId  ";

            SqlParameter[] param = { 				
				new SqlParameter("@MakeId", makeId)
			};

            Trace.Warn(sql);

            try
            {
                dsStateCity = db.SelectAdaptQry(sql, param);
                //CityCount = int.Parse(dsStateCity.Tables[0].Rows.Count.ToString());
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }//function

        private void BindCities()
        {
            ICity objCities = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICity, CityRepository>();
                objCities = container.Resolve<ICity>();
                dealerCity = objCities.GetDealerStateCities(makeId, stateId);
                if (dealerCity != null && dealerCity.dealerCities != null && dealerCity.dealerCities.Count() > 0)
                {
                    rptCity.DataSource = dealerCity.dealerCities;
                    rptCity.DataBind();
                    cityArr = Newtonsoft.Json.JsonConvert.SerializeObject(dealerCity.dealerCities);
                    cityArr = cityArr.Replace("cityId", "id").Replace("cityName", "name");
                    DealerCount = dealerCity.dealerCities.Select(o => o.DealersCount).Aggregate((x, y) => x + y);
                    citesCount = dealerCity.dealerCities.Count();
                    stateName = dealerCity.dealerStates.StateName;
                }

            }
        }

        //public DataSet BindCities(int StateId)
        //{
        //    DataSet dsCity = new DataSet();

        //    DataTable dt = dsCity.Tables.Add();
        //    dt.Columns.Add("CityId", typeof(int));
        //    dt.Columns.Add("City", typeof(string));
        //    dt.Columns.Add("StateId", typeof(int));
        //    dt.Columns.Add("State", typeof(string));
        //    dt.Columns.Add("TotalBranches", typeof(int));
        //    dt.Columns.Add("CityMaskingName", typeof(string));

        //    DataRow dr;

        //    DataRow[] rows = dsStateCity.Tables[0].Select("StateId = " + StateId, "City ASC");

        //    foreach (DataRow row in rows)
        //    {
        //        dr = dt.NewRow();
        //        dr["CityId"] = row["CityId"].ToString();
        //        dr["City"] = row["City"].ToString();
        //        dr["CityMaskingName"] = row["CityMaskingName"].ToString();
        //        dr["StateId"] = row["StateId"].ToString();
        //        dr["State"] = row["State"].ToString();
        //        dr["TotalBranches"] = row["TotalBranches"].ToString();
        //        DealerCount += int.Parse(row["TotalBranches"].ToString());
        //        dt.Rows.Add(dr);
        //    }
        //    Trace.Warn("City Dataset contains: " + dsCity.Tables[0].Rows.Count.ToString());
        //    return dsCity;

        //}

        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (!string.IsNullOrEmpty(Request["make"]) && !string.IsNullOrEmpty(Request["state"]))
            {
                string _makeId = MakeMapping.GetMakeId(Request.QueryString["make"].ToLower());
                makeId = Convert.ToUInt32(_makeId);

                //verify the id as passed in the url
                if (CommonOpn.CheckId(_makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
                stateMaskingName = Request.QueryString["state"];
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IState, StateRepository>()
                             .RegisterType<IStateCacheRepository, StateCacheRepository>()
                            ;
                    var objCache = container.Resolve<IStateCacheRepository>();
                    var objResponse = objCache.GetStateMaskingResponse(stateMaskingName);
                    if (objResponse != null)
                    {
                        stateId = objResponse.StateId;
                    }
                }
            }
            else
            {
                Trace.Warn("make id : ", Request.QueryString["make"]);
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            return isSuccess;
        }
    }   // End of class
}   // End of namespace