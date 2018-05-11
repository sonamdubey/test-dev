using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.BikeData;
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
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;

        public BindAlternativeBikesControl(IBikeVersions<BikeVersionEntity, uint> objVersion)
        {
            _objVersion = objVersion;
        }

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
                IEnumerable<SimilarBikeEntity> objSimilarBikes = _objVersion.GetSimilarBikesList(VersionId, TotalWidgetItems, cityId, false);
                if (objSimilarBikes != null && objSimilarBikes.Any())
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
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

        }

    }
}