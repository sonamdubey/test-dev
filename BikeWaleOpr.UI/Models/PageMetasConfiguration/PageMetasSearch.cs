using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using BikewaleOpr.BAL;
using BikewaleOpr.Entity.ConfigurePageMetas;
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

        public void SaveMetas(PageMetasEntity objMetas)
        {           
            try
            {
                bool isSuccess =  _pageMetasRepo.SavePageMetas(objMetas);

                if (isSuccess)
                {
                    string mailList = Bikewale.Utility.BWOprConfiguration.Instance.NotificationToMailIdForPageMetas;
                    string[] toMailList = mailList.Split(',');
                    ComposeEmailBase objEmail = new PageMetasChangeTemplate(objMetas);

                    foreach (var mail in toMailList)
                    {
                        objEmail.Send(mail, "Metas Changed", "");
                    }

                    if (objMetas.ModelId > 0)
                        MemCachedUtil.Remove("BW_ModelDetail_" + objMetas.ModelId);

                    MemCachedUtil.Remove("BW_MakeDetails_" + objMetas.MakeId);
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "PageMetasSearch.SaveMetas");
            }            
        }
    }
}