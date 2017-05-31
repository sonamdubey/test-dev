﻿using System;
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
using React.TinyIoC;
using React.Exceptions;


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
                    return AssemblyRegistration.Container.Resolve<IReactEnvironment>();
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
