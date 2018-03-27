
using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly uint _modelId;
        private readonly bool _showCheckOnRoadCTA = true;
        private readonly bool _showPriceInCityCTA;
        private readonly bool _similarBikesByModel = false;
        #endregion

        #region Public Property
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }
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

        public SimilarBikesWidget(IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache
          , uint modelId, bool similarBikesByModel, PQSourceEnum pqSource)
        {
            _versionCache = versionCache;
            _modelId = modelId;
            _similarBikesByModel = similarBikesByModel;
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
        /// Modified by : Pratibha Verma on 26 Mar 2018
        /// Description : grpc method call to fetch minSpecs data
        /// </summary>
        /// <returns></returns>
        public SimilarBikesWidgetVM GetData()
        {
            SimilarBikesWidgetVM objVM = null;
            try
            {
                objVM = new SimilarBikesWidgetVM();
                objVM.ShowCheckOnRoadCTA = _showCheckOnRoadCTA;
                objVM.ShowPriceInCityCTA = _showPriceInCityCTA;
                if (!_similarBikesByModel)
                {
                    objVM.Bikes = _versionCache.GetSimilarBikesList(_versionId, TopCount, CityId);
                    if (objVM.Bikes != null && objVM.Bikes.Any())
                    {
                        IEnumerable<VersionMinSpecsEntity> versionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(objVM.Bikes.Select(m => m.VersionBase.VersionId));
                        foreach (var bike in objVM.Bikes)
                        {
                            VersionMinSpecsEntity minSpecsEntity = versionMinSpecs.FirstOrDefault(x => x.VersionId.Equals(bike.VersionBase.VersionId));
                            bike.MinSpecsList = minSpecsEntity != null ? minSpecsEntity.MinSpecsList : null;
                        }
                    }
                }
                else
                {
                    objVM.Bikes = _versionCache.GetSimilarBikesByModel(_modelId, TopCount, CityId);
                    if (objVM.Bikes != null && objVM.Bikes.Any())
                    {
                        IEnumerable<VersionMinSpecsEntity> versionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(objVM.Bikes.Select(m => m.VersionBase.VersionId));
                        foreach (var bike in objVM.Bikes)
                        {
                            VersionMinSpecsEntity minSpecsEntity = versionMinSpecs.FirstOrDefault(x => x.VersionId.Equals(bike.VersionBase.VersionId));
                            bike.MinSpecsList = minSpecsEntity != null ? minSpecsEntity.MinSpecsList : null;
                        }
                    }
                }
                objVM.PQSourceId = _pqSource;
                objVM.IsNew = IsNew;
                objVM.IsUpcoming = IsUpcoming;
                objVM.IsDiscontinued = IsDiscontinued;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.SimilarBikesWidget.GetData");
            }
            return objVM;
        }
    }
}