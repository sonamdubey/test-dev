using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.DAL.CarData;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Notifications;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using AEPLCore.Cache.Interfaces;

namespace Carwale.UI.Controls
{
    public class UpcomingCars : UserControl
    {
        private int _topCount = 0;
        private string _makeId = "-1", _make = "";
        private bool _verticalDisplay = true, _loadStatic = false;
        protected Repeater rptData;
        protected HtmlGenericControl divUpcomingCar;
        protected Label lblNotFound;



        public int TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public string MakeId
        {
            get { return _makeId; }
            set { _makeId = value; }
        }

        public string Make
        {
            get { return _make; }
            set { _make = value; }
        }

        public bool VerticalDisplay
        {
            get { return _verticalDisplay; }
            set { _verticalDisplay = value; }
        }

        public bool LoadStatic
        {
            get { return _loadStatic; }
            set { _loadStatic = value; }
        }

        public List<UpcomingCarModel> UpcomingCarsList { get; set; }
        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (VerticalDisplay == false) divUpcomingCar.Attributes.Add("class", "uc-hor");

            if (!IsPostBack)
            {
                Trace.Warn("user control" + TopCount);
                GetDetails();
            }
        } // Page_Load

        /// <summary>
        /// StoredProcedure used:cw.GetUpcomingCars
        /// </summary>
        /// 

        ///<summary>
        /// Modified by :Shalini on 25/08/14
        /// Populates the list of Upcoming Cars based on makeId passed 
        ///</summary>
        public void GetDetails()
        {
            int makeId;
            try
            {
                if (UpcomingCarsList != null)
                {
                    Trace.Warn("in make page or model page");
                    rptData.DataSource = UpcomingCarsList;
                    rptData.DataBind();
                }
                else
                {
                    if (MakeId != "-1" && MakeId.Trim() != "")
                    {
                        makeId = int.Parse(MakeId);
                    }
                    else
                    {
                        makeId = 0;
                    }

                    IUnityContainer container = new UnityContainer();
                    container.RegisterType<ICarModelRepository, CarModelsRepository>()
                      .RegisterType<ICacheManager, CacheManager>()
                      .RegisterType<ICarModelCacheRepository, CarModelsCacheRepository>();

                    ICarModelCacheRepository modelContainer = container.Resolve<ICarModelCacheRepository>();
                    rptData.DataSource = modelContainer.GetUpcomingCarModelsByMake(makeId, TopCount);
                    rptData.DataBind();
                }
            }

            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            if (rptData.Items.Count <= 0)
            {
                lblNotFound.Visible = true;
                lblNotFound.Text = "There is no Upcoming car expected from " + Make + " in near future";
            }

        }
    }
}