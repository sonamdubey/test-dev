using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Utility;
using log4net;
using React;
using React.Exceptions;
using React.TinyIoC;
using System;
using System.Diagnostics;
using System.Web;


namespace Bikewale.BAL.PWA.CMS
{
    /// <summary>
    /// Created By : Prasad Gawde on 25th May 2017
    /// Description : The class is responsible for server side rendering of Newslist and News details
    /// </summary>
    public class PWACMSRenderedData : IPWACMSContentRepository
    {
        static bool _logPWAStats = BWConfiguration.Instance.EnablePWALogging;
        static ILog _logger = LogManager.GetLogger("Pwa-Logger-Renderengine");

        /// <summary>
        /// Gets the React environment
        /// </summary>
        private static IReactEnvironment Environment
        {
            get
            {
                try
                {
                    return ReactEnvironment.Current;
                }
                catch (TinyIoCResolutionException ex)
                {
                    throw new ReactNotInitialisedException(
#if LEGACYASPNET
						"ReactJS.NET has not been initialised correctly.",
#else
                        "ReactJS.NET has not been initialised correctly. Please ensure you have " +
                        "called services.AddReact() and app.UseReact() in your Startup.cs file.",
#endif
                        ex
                    );
                }
            }
        }


        static IHtmlString React<T>(
            string componentName,
            T props,
            string htmlTag = null,
            string containerId = null,
            bool clientOnly = false,
            bool serverOnly = false,
            string containerClass = null
        )
        {
            try
            {
                var reactComponent = Environment.CreateComponent(componentName, props, containerId, clientOnly);
                if (!string.IsNullOrEmpty(htmlTag))
                {
                    reactComponent.ContainerTag = htmlTag;
                }
                if (!string.IsNullOrEmpty(containerClass))
                {
                    reactComponent.ContainerClass = containerClass;
                }
                var result = reactComponent.RenderHtml(clientOnly, serverOnly);
                return new HtmlString(result);
            }
            finally
            {
                Environment.ReturnEngineToPool();
            }
        }


        public IHtmlString GetNewsListDetails(PwaNewsArticleListReducer reducer, string url, string containerId, string componentName)
        {

            Stopwatch sw = null;
            if (_logPWAStats)
                sw = Stopwatch.StartNew();

            IHtmlString renderedHtml = null;
            try
            {
                renderedHtml = React(componentName, new
                {
                    Url = url,
                    ArticleListData = reducer.ArticleListData,
                    NewBikesListData = reducer.NewBikesListData

                }, containerId: containerId);
            }
            catch (Exception ex)
            {
                ThreadContext.Properties["ArticleUrl"] = url;
                ThreadContext.Properties["Component"] = componentName + ":" + containerId;
                _logger.Error(ex);
            }

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

            IHtmlString renderedHtml = null;
            try
            {
                renderedHtml = React(componentName, new
                {
                    Url = url,
                    ArticleDetailData = reducer.ArticleDetailData,
                    RelatedModelObject = reducer.RelatedModelObject,
                    NewBikesListData = reducer.NewBikesListData

                }, containerId: containerId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

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
