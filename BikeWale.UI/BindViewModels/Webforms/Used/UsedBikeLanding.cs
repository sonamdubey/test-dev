using Bikewale.Cache.Core;
using Bikewale.Cache.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UsedBikes;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 Oct 2016
    /// </summary>
    public class UsedBikeLandingPage
    {
        public IEnumerable<UsedBikeMakeEntity> TopMakeList;
        public IEnumerable<UsedBikeMakeEntity> OtherMakeList;

        public UsedBikeLandingPage()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikesCache, UsedBikesCache>();
                    IUsedBikesCache objUsedBikes = container.Resolve<IUsedBikesCache>();
                    var totalList = objUsedBikes.GetUsedBikeMakesWithCount();
                    if (totalList != null && totalList.Count() > 0)
                    {
                        TopMakeList = totalList.Take(6);
                        OtherMakeList = totalList.Skip(6);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.Used.UsedBikeLandingPage.constructor");
                objErr.SendMail();
            }
        }
    }
}