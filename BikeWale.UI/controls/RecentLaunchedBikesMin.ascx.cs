using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                    container.RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IPager, Bikewale.BAL.Pager.Pager>();
                    IBikeModelsCacheRepository<int> objModel = container.Resolve<IBikeModelsCacheRepository<int>>();
                    int startIndex = 0, endIndex = 0, curPageNo = 1;

                    var _objPager = container.Resolve<IPager>();
                    _objPager.GetStartEndIndex(_topRecords, curPageNo, out startIndex, out endIndex);

                    IEnumerable<NewLaunchedBikeEntity> objList = objModel.GetNewLaunchedBikesList(startIndex, endIndex).Models;

                    if (objList != null && objList.Any())
                    {
                        rptLaunchedBikes.DataSource = objList;
                        rptLaunchedBikes.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        }
    }
}