﻿using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindAlternativeBikesControl
    {
        public uint VersionId { get; set; }
        public ushort TopCount { get; set; }
        public int? Deviation { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int PQSourceId { get; set; }
        public uint cityId { get; set; }
        private const ushort TotalWidgetItems = 9;

        /// <summary>
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : Set default fetched record count to 9 and pass toprecord count data only
        /// </summary>
        /// <param name="rptAlternativeBikes"></param>
        public void BindAlternativeBikes(Repeater rptAlternativeBikes)
        {
            FetchedRecordsCount = 0;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, int>, BikeVersionsCacheRepository<BikeVersionEntity, int>>()
                        .RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                             ;
                    var objCache = container.Resolve<IBikeVersionCacheRepository<BikeVersionEntity, int>>();
                    IEnumerable<SimilarBikeEntity> objSimilarBikes = objCache.GetSimilarBikesList(Convert.ToInt32(VersionId), TotalWidgetItems, cityId);


                    if (objSimilarBikes != null && objSimilarBikes.Count() > 0)
                    {
                        objSimilarBikes = objSimilarBikes.Take(TopCount);
                        if (rptAlternativeBikes != null)
                        {
                            rptAlternativeBikes.DataSource = objSimilarBikes;
                            rptAlternativeBikes.DataBind();
                        }


                        FetchedRecordsCount = objSimilarBikes.Count();
                    }

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