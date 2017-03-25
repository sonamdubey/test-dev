using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.BikeCare
{
    /// <summary>
    /// Created by : Aditi Srivastava on 24 Mar 2017
    /// Summary    : Model to get list of bike care articles for partial view
    /// </summary>
    public class RecentBikeCare
    {

         private readonly ICMSCacheContent _articles = null;

        #region Constructor
         public RecentBikeCare(ICMSCacheContent articles)
        {
            _articles = articles;
        }
        #endregion

        #region Functions to get data
         /// <summary>
         /// Created by : Aditi Srivastava on 24 Mar 2017
         /// Summary    : To get list of bike care articles
         /// </summary>
         public RecentBikeCareVM GetData(uint totalRecords, uint makeId, uint modelId)
        {
            RecentBikeCareVM recentBikeCare = new RecentBikeCareVM();
           
             try
            {
                recentBikeCare.ArticlesList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.TipsAndAdvices), totalRecords, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.BikeCare.RecentBikeCare.GetData: TotalRecords {0},MakeId {1}, ModelId {2}", totalRecords, makeId, modelId));
            }
             return recentBikeCare;
        }
        #endregion

    }
}