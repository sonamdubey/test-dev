using Bikewale.Notifications;
using BikewaleOpr.Entity.ConfigurePageMetas;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ConfigurePageMetas;
using BikewaleOpr.Models.ConfigurePageMetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.PageMetasConfiguration
{
    public class ConfigurePageMetas
    {
        private readonly IBikeMakesRepository _makesRepo = null;
        private readonly IConfigurePageMetasRepository _pageMetasRepo = null;

        public ConfigurePageMetas(IBikeMakesRepository makesRepo, IConfigurePageMetasRepository pageMetasRepo)
        {           
            _makesRepo = makesRepo;
            _pageMetasRepo = pageMetasRepo;
        }

        public ConfigurePageMetasVM GetData()
        {
            ConfigurePageMetasVM objPageModel = new ConfigurePageMetasVM();
            try
            {
                objPageModel.Makes = _makesRepo.GetMakes(8);
                IEnumerable<PageEntity> pagesList = _pageMetasRepo.GetPagesList();

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                objPageModel.DesktopPages = serializer.Serialize(pagesList.Where(x => x.PlatformId == 1));
                objPageModel.MobilePages = serializer.Serialize(pagesList.Where(x => x.PlatformId == 2));





            }
            catch
            {

            }
            return objPageModel;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Aug-2017
        /// Description : Method to get active or inactive or both page metas list.
        /// </summary>
        /// <param name="pageMetaStatus"></param>
        /// <returns></returns>
        public ConfigurePageMetaSearchVM GetPageMetasData(ushort pageMetaStatus = 1)
        {
            ConfigurePageMetaSearchVM objPageMetaVM = new ConfigurePageMetaSearchVM();
            try
            {
                objPageMetaVM.PageMetaList = _pageMetasRepo.GetPageMetas(pageMetaStatus);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasConfiguration.GetPageMetas_pageMetaStatus : {0}", pageMetaStatus));
            }
            return objPageMetaVM;
        }
    }
}