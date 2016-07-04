using Bikewale.BAL.EditCMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// Summary : Class have functions to bind the expert reviews.
    /// </summary>
    public class BindExpertReviewsControl
    {
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }


        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);

        /// <summary>
        /// Summary : Function to bind the expert reviews control. Function will cache the data from CW api on bikewale
        /// </summary>
        public void BindExpertReviews(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            try
            {
                IEnumerable<ArticleSummary> _objArticleList = null;


                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>();
                    IArticles _articles = container.Resolve<IArticles>();
                    _objArticleList = _articles.GetRecentExpertReviews(Convert.ToInt32(MakeId), Convert.ToInt32(ModelId), Convert.ToInt32(TotalRecords));
                }


                if (_objArticleList != null && _objArticleList.Count() > 0)
                {
                    FetchedRecordsCount = _objArticleList.Count();

                    rptr.DataSource = _objArticleList;
                    rptr.DataBind();
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