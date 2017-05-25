using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.DAL.CMS;
using Bikewale.Notifications;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Entities.PWA.Articles;
using System.Web;
using React;
using System.Diagnostics;
using Bikewale.Utility;
using log4net;

namespace Bikewale.BAL.PWA.CMS
{
    public class PWACMSRenderedData : IPWACMSContentRepository
    {
        static bool _logPWAStats = BWConfiguration.Instance.EnablePWALogging;
        static ILog _logger = LogManager.GetLogger("Pwa-Logger-Renderengine");

        private static IReactEnvironment Environment
        {
            // TODO: Figure out if this can be injected
            get { return AssemblyRegistration.Container.Resolve<IReactEnvironment>(); }
        }

        private static IHtmlString React<T>(
            string componentName,
            T props,
            string htmlTag = null,
            string containerId = null
        )
        {
            var reactComponent = Environment.CreateComponent(componentName, props, containerId);
            if (!string.IsNullOrEmpty(htmlTag))
            {
                reactComponent.ContainerTag = htmlTag;
            }
            var result = reactComponent.RenderHtml();
            return new HtmlString(result);
        }

        public IHtmlString GetNewsListDetails(PwaNewsArticleListReducer reducer, string url, string containerId, string componentName)
        {

            Stopwatch sw = null;
            if (_logPWAStats)
                sw = Stopwatch.StartNew();
            
            var renderedHtml = React(componentName, new
            {
                Url = url,
                ArticleListData = reducer.ArticleListData,
                NewBikesListData = reducer.NewBikesListData

            }, containerId:containerId);

            if (_logPWAStats)
            {
                sw.Stop();
                ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                ThreadContext.Properties["PageName"] = "News List";
                _logger.Error(sw.ElapsedMilliseconds);
            }
            return renderedHtml;
        }


        public IHtmlString GetNewsDetails(PwaNewsDetailReducer reducer, string url, string containerId, string componentName)
        {

            Stopwatch sw = null;
            if (_logPWAStats)
                sw = Stopwatch.StartNew();

            var renderedHtml = React(componentName, new
            {
                Url = url,
                ArticleDetailData = reducer.ArticleDetailData,
                RelatedModelObject = reducer.RelatedModelObject,
                NewBikesListData = reducer.NewBikesListData

            }, containerId:containerId);

            if (_logPWAStats)
            {
                sw.Stop();
                ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                ThreadContext.Properties["PageName"] = "News Detail";
                _logger.Error(sw.ElapsedMilliseconds);
            }
            return renderedHtml;
        }
    }
}
