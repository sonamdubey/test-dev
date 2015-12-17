using Bikewale.Common;
using Bikewale.DTO.BikeData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Interfaces.Pager;
using Bikewale.BAL.Pager;

namespace Bikewale.BindViewModels.Controls
{
    public class BindNewLaunchedBikesControl
    {
        public int pageSize { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }

        public void BindNewlyLaunchedBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            List<NewLaunchedBikeEntity> objBikeList = null;
            int recordCount = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> _objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                    LaunchedBikeList objLaunched = new LaunchedBikeList();

                    objBikeList = _objModel.GetNewLaunchedBikesList(pageSize, out recordCount, curPageNo);

                    if (objBikeList != null && objBikeList.Count() > 0)
                    {
                        FetchedRecordsCount = objBikeList.Count();

                        if (FetchedRecordsCount > 0)
                        {
                            rptr.DataSource = objBikeList;
                            rptr.DataBind();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}