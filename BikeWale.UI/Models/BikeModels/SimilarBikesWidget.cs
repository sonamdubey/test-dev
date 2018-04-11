
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
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
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
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
        /// Modified by :   Rajan Chauhan on 11 Apr 2018
        /// Description :   Replaced IBikeVersionsCacheRepository with IBikeVersions 
        /// </summary>
        /// <param name="objVersion"></param>
        /// <param name="versionId"></param>
        /// <param name="pqSource"></param>
        public SimilarBikesWidget(IBikeVersions<BikeVersionEntity, uint> objVersion
            , uint versionId, PQSourceEnum pqSource)
        {
            _versionId = versionId;
            _pqSource = pqSource;
            _objVersion = objVersion;
        }

        public SimilarBikesWidget(IBikeVersions<BikeVersionEntity, uint> objVersion
          , uint modelId, bool similarBikesByModel, PQSourceEnum pqSource)
        {
            _modelId = modelId;
            _similarBikesByModel = similarBikesByModel;
            _pqSource = pqSource;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Mar 2017
        /// Description :   Overload Constructor
        /// </summary>
        /// <param name="objVersion"></param>
        /// <param name="versionId"></param>
        /// <param name="pqSource"></param>
        /// <param name="showCheckOnRoadCTA"></param>
        public SimilarBikesWidget(IBikeVersions<BikeVersionEntity, uint> objVersion
            , uint versionId, PQSourceEnum pqSource, bool showCheckOnRoadCTA)
            : this(objVersion, versionId, pqSource)
        {

            _showCheckOnRoadCTA = showCheckOnRoadCTA;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Mar 2017
        /// Description :   Overload Constructor
        /// </summary>
        /// <param name="objVersion"></param>
        /// <param name="versionId"></param>
        /// <param name="pqSource"></param>
        /// <param name="showCheckOnRoadCTA"></param>
        /// <param name="showPriceInCityCTA"></param>
        public SimilarBikesWidget(IBikeVersions<BikeVersionEntity, uint> objVersion
            , uint versionId, PQSourceEnum pqSource, bool showCheckOnRoadCTA, bool showPriceInCityCTA)
            : this(objVersion, versionId, pqSource, showCheckOnRoadCTA)
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
                    objVM.Bikes = _objVersion.GetSimilarBikesList(_versionId, TopCount, CityId);
                }
                else
                {
                    objVM.Bikes = _objVersion.GetSimilarBikesByModel(_modelId, TopCount, CityId);
                }
                BindMinSpecs(objVM.Bikes);
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

        /// <summary>
        /// Created By : Pratibha Verma on 27 Mar 2018
        /// Summary : Bind MinSpecs to Generic Bike List
        /// </summary>
        private void BindMinSpecs(IEnumerable<SimilarBikeEntity> SimilarBikeList)
        {
            try
            {
                if (SimilarBikeList != null && SimilarBikeList.Any())
                {
                    IEnumerable<VersionMinSpecsEntity> versionMinSpecsEntityList = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(SimilarBikeList.Select(m => m.VersionBase.VersionId));
                    if (versionMinSpecsEntityList != null)
                    {
                        IEnumerator<VersionMinSpecsEntity> versionIterator = versionMinSpecsEntityList.GetEnumerator();
                        VersionMinSpecsEntity objVersionMinSpec;
                        foreach (var bike in SimilarBikeList)
                        {
                            if (versionIterator.MoveNext())
                            {
                                objVersionMinSpec = versionIterator.Current;
                                bike.MinSpecsList = objVersionMinSpec != null ? objVersionMinSpec.MinSpecsList : null;
                            }
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.SimilarBikesWidget.BindMinSpecs({0})",SimilarBikeList));
            }
        }
    }
}