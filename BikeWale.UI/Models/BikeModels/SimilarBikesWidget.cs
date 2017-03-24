
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Mar 2017
    /// Description :   SimilarBikesWidget Model
    /// </summary>
    public class SimilarBikesWidget
    {
        #region Private Readonly
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCache = null;
        private readonly PQSourceEnum _pqSource;
        private readonly uint _versionId;
        private readonly bool _showCheckOnRoadCTA = true;
        private readonly bool _showPriceInCityCTA;
        #endregion

        #region Public Property
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        #endregion

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Mar 2017
        /// Description :   Constructor to initialize member variables
        /// </summary>
        /// <param name="versionCache"></param>
        /// <param name="versionId"></param>
        /// <param name="pqSource"></param>
        public SimilarBikesWidget(IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache
            , uint versionId, PQSourceEnum pqSource)
        {
            _versionCache = versionCache;
            _versionId = versionId;
            _pqSource = pqSource;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Mar 2017
        /// Description :   Overload Constructor
        /// </summary>
        /// <param name="versionCache"></param>
        /// <param name="versionId"></param>
        /// <param name="pqSource"></param>
        /// <param name="showCheckOnRoadCTA"></param>
        public SimilarBikesWidget(IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache
            , uint versionId, PQSourceEnum pqSource, bool showCheckOnRoadCTA)
            : this(versionCache, versionId, pqSource)
        {

            _showCheckOnRoadCTA = showCheckOnRoadCTA;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Mar 2017
        /// Description :   Overload Constructor
        /// </summary>
        /// <param name="versionCache"></param>
        /// <param name="versionId"></param>
        /// <param name="pqSource"></param>
        /// <param name="showCheckOnRoadCTA"></param>
        /// <param name="showPriceInCityCTA"></param>
        public SimilarBikesWidget(IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache
            , uint versionId, PQSourceEnum pqSource, bool showCheckOnRoadCTA, bool showPriceInCityCTA)
            : this(versionCache, versionId, pqSource, showCheckOnRoadCTA)
        {
            _showPriceInCityCTA = showPriceInCityCTA;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Mar 2017
        /// Description :   GetData returns SimilarBikesWidget Viewmodel
        /// </summary>
        /// <returns></returns>
        public SimilarBikesWidgetVM GetData()
        {
            SimilarBikesWidgetVM objVM = null;
            try
            {
                objVM = new SimilarBikesWidgetVM();
                objVM.Bikes = _versionCache.GetSimilarBikesList(_versionId, TopCount, CityId);
                objVM.PQSourceId = _pqSource;
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "Bikewale.Models.SimilarBikesWidget.GetData");
            }
            return objVM;
        }
    }
}