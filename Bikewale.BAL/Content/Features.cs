using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Content;
using Bikewale.Notifications;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Bikewale.BAL.Content
{
    public class Features : IFeatures
    {
        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(Features));
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        string cacheKey = "BW_ViewF";
        private ArticlePageDetails objFeature = null;
        private IEnumerable<ModelImage> objImg = null;


        
    }
}
