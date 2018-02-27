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

        /// <summary>
        /// Created By: Prasad Gawde
        /// </summary>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <returns>Returns the Rendered HTML for the News List Page</returns>
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
                string newStr = string.IsNullOrEmpty(componentName) ? "Null Component" : componentName;
                ThreadContext.Properties["ComponentName"] = newStr + " : " + containerId;
                ThreadContext.Properties["NewsURL"] = string.IsNullOrEmpty(url) ? "Null News URL" : url;
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

        /// <summary>
        /// Created by Prasad Gawde
        /// Modified By : Rajan Chauhan on 26 Feb 2018
        /// Description : Added pageName in args 
        ///               Changed NewsURL in ThreadContext properties to CMSURL
        /// </summary>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <param name="pageName"></param>
        /// <returns>Returns the Rendered HTML for the News Details Page for the input BasicId for which Store us constructed in Reducer</returns>
        public IHtmlString GetNewsDetails(PwaNewsDetailReducer reducer, string url, string containerId, string componentName, string pageName)
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
                string newStr = string.IsNullOrEmpty(componentName) ? "Null Component" : componentName;
                ThreadContext.Properties["ComponentName"] = newStr+" : "+containerId;
                ThreadContext.Properties["CMSURL"] = string.IsNullOrEmpty(url) ? "Null " + pageName + " URL" : url;
                _logger.Error(ex);
            }

            if (_logPWAStats)
            {
                sw.Stop();
                ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                ThreadContext.Properties["PageName"] = pageName + " Detail";
                _logger.Error(sw.ElapsedMilliseconds);
            }
            return renderedHtml;
        }

        /// <summary>
        /// Created by Prasad Gawde
        /// </summary>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <returns>Returns the Rendered HTML for the Video List Page</returns>
        public IHtmlString GetVideoListDetails(PwaAllVideos reducer, string url, string containerId, string componentName)
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
                    TopVideos = reducer.TopVideos,
                    OtherVideos = reducer.OtherVideos

                }, containerId: containerId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            if (_logPWAStats)
            {
                if (sw != null)
                {
                    sw.Stop();
                    ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                    ThreadContext.Properties["PageName"] = "Video List";
                    _logger.Error(sw.ElapsedMilliseconds);
                }
            }
            return renderedHtml;
        }

        /// <summary>
        /// Created By Prasad Gawde
        /// </summary>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <returns>Returns the Rendered HTML for the Video List when asked for a sub-category list videos i.e. Say Expert-reviews Videos</returns>
        public IHtmlString GetVideoBySubCategoryListDetails(PwaVideosBySubcategory reducer, string url, string containerId, string componentName)
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
                    VideosByCategory = reducer

                }, containerId: containerId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            if (_logPWAStats)
            {
                if (sw != null)
                {
                    sw.Stop();
                    ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                    ThreadContext.Properties["PageName"] = "Video Subcategory List";
                    _logger.Error(sw.ElapsedMilliseconds); 
                }
            }
            return renderedHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <returns>Returns the Rendered HTML for the Video Detail Page</returns>
        public IHtmlString GetVideoDetails(PwaVideoDetailReducer reducer, string url, string containerId, string componentName)
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
                    ModelInfo = reducer.ModelInfo,
                    RelatedInfo = reducer.RelatedInfo,
                    VideoInfo = reducer.VideoInfo

                }, containerId: containerId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            if (_logPWAStats)
            {
                if (sw != null)
                {
                    sw.Stop();
                    ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                    ThreadContext.Properties["PageName"] = "GetVideoDetails";
                    _logger.Error(sw.ElapsedMilliseconds); 
                }
            }
            return renderedHtml;
        }
    }
}
