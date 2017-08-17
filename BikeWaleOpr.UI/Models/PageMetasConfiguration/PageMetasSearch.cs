using Bikewale.Notifications;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ConfigurePageMetas;
using BikewaleOpr.Models.ConfigurePageMetas;
using System;

namespace BikewaleOpr.Models.PageMetasConfiguration
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 14-Aug-2017
    /// Description : Model for page meta search page. Contains method to get page metas list.
    /// </summary>
    public class PageMetasSearch
    {
        private readonly IPageMetasRepository _pageMetasRepo = null;

        public PageMetasSearch(IPageMetasRepository pageMetasRepo)
        {
            _pageMetasRepo = pageMetasRepo;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Aug-2017
        /// Description : Method to get active or inactive or both page metas list.
        /// </summary>
        /// <param name="pageMetaStatus"></param>
        /// <returns></returns>
        public ConfigurePageMetaSearchVM GetData(ushort pageMetaStatus = 1)
        {
            ConfigurePageMetaSearchVM objPageMetaVM = new ConfigurePageMetaSearchVM();
            try
            {
                objPageMetaVM.PageMetaList = _pageMetasRepo.GetPageMetas(pageMetaStatus);
                objPageMetaVM.PageMetaStatus = pageMetaStatus;

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PageMetasSearch.GetData_pageMetaStatus : {0}", pageMetaStatus));
            }
            return objPageMetaVM;
        }
    }
}