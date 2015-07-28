using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public class RecentLaunchedBikesMin : System.Web.UI.UserControl
    {
        protected Repeater rptLaunchedBikes;
        private int _topRecords = 4;

        public int TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FetchRecentLaunchedBikes();
            }
        }

        private void FetchRecentLaunchedBikes()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();


                    int recordCount = 0;

                    List<NewLaunchedBikeEntity> objList = objModel.GetNewLaunchedBikesList(0, TopRecords, out recordCount);

                    if (objList.Count > 0)
                    {
                        rptLaunchedBikes.DataSource = objList;
                        rptLaunchedBikes.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}