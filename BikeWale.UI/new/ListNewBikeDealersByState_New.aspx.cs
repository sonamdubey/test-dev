using Bikewale.Common;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    public class ListNewBikeDealersByState_New : Page
    {
        protected Repeater rptState;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected MakeModelVersion objMMV;

        public string makeId = string.Empty, stateArray = string.Empty, makeMaskingName = string.Empty;
        public int stateCount = 0, DealerCount = 0; protected int countryCount = 0;

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
                    objMMV.GetMakeDetails(makeId);

                    BindControl();
                    BindStates();
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

            try
            {
                dsStateCity = db.SelectAdaptQry(sql, param);
                stateCount = int.Parse(dsStateCity.Tables[0].Rows.Count.ToString());

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }//function
        /// <summary>
        /// Modified by: Sangram Nandkhile on 24th Jun 2016
        /// </summary>
        private void BindStates()
        {
            IEnumerable<DealerStateEntity> states = null;
            IState objStates = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IState, StateRepository>();
                objStates = container.Resolve<IState>();
                states = objStates.GetDealerStates(Convert.ToUInt32(makeId));
                if (states != null)
                {
                    rptState.DataSource = states;
                    rptState.DataBind();
                    stateArray = Newtonsoft.Json.JsonConvert.SerializeObject(states);
                    // To set correct properties in json array
                    stateArray = stateArray.Replace("stateId", "id").Replace("stateName", "name");
                    countryCount = states.Select(o => o.StateDealerCount).Aggregate((x, y) => x + y);
                    stateCount = states.Count();
                }
            }
        }


        /// <summary>
        /// Process query string and fetch make id
        /// </summary>
        /// <returns></returns>
        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (string.IsNullOrEmpty(Request["make"]))
            {
                makeMaskingName = Request["make"].ToString();
                makeId = MakeMapping.GetMakeId(Request.QueryString["make"].ToLower());
                if (CommonOpn.CheckId(makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
            }
            else
            {
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            return isSuccess;
        }
    }   // End of class
}   // End of namespace