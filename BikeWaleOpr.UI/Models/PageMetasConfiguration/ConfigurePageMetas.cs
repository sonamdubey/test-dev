using BikewaleOpr.Entity.ConfigurePageMetas;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ConfigurePageMetas;
using BikewaleOpr.Models.ConfigurePageMetas;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.Models.PageMetasConfiguration
{
    public class ConfigurePageMetas
    {
        private readonly IBikeMakesRepository _makesRepo = null;
        private readonly IPageMetasRepository _pageMetasRepo = null;

        private readonly uint? _pageMetaId;

        public ConfigurePageMetas(IBikeMakesRepository makesRepo, IPageMetasRepository pageMetasRepo, uint? pageMetaId)
        {           
            _makesRepo = makesRepo;
            _pageMetasRepo = pageMetasRepo;
            _pageMetaId = pageMetaId;
        }

        public ConfigurePageMetasVM GetData()
        {
            ConfigurePageMetasVM objPageModel = new ConfigurePageMetasVM();
            try
            {
                objPageModel.Makes = _makesRepo.GetMakes(7);
                IEnumerable<PageEntity> pagesList = _pageMetasRepo.GetPagesList();

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                objPageModel.DesktopPages = serializer.Serialize(pagesList.Where(x => x.PlatformId == 1));
                objPageModel.MobilePages = serializer.Serialize(pagesList.Where(x => x.PlatformId == 2));

                objPageModel.CurrentUserId = Convert.ToInt32(CurrentUser.Id);

                if(_pageMetaId.HasValue)
                {
                    objPageModel.PageMetaId = _pageMetaId.Value;
                    objPageModel.PageMetas = _pageMetasRepo.GetPageMetasById(_pageMetaId.Value);
                }

            }
            catch
            {

            }
            return objPageModel;
        }

        
    }
}