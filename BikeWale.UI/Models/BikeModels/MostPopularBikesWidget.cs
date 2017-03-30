﻿
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
namespace Bikewale.Models
{
    /// <summary>
    /// <para />Created by  :   Sumit Kate on 24 Mar 2017
    /// <para />Description :   Most Popular Bikes Widget Model
    /// </summary>
    public class MostPopularBikesWidget
    {
        #region Private Readonly Member Variables and Objects
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly bool _showCheckOnRoadCTA;
        private readonly bool _showPriceInCityCTA;
        private readonly uint _makeId, _pageCatId;
        private readonly PQSourceEnum _pqSource;
        private readonly EnumBikeType _bikeType;
        #endregion

        #region Public Property
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        #endregion

        /// <summary>
        /// <para />Created by  :   Sumit Kate on 24 Mar 2017
        /// <para />Description :   Initialize the essential Model member variables
        /// <para />Example     :
        /// <para />MostPopularBikesWidget mostPopular = new MostPopularBikesWidget(_models, EnumBikeType.New, true);
        /// <para />mostPopular.TopCount = 9;
        /// <para />mostPopular.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
        /// <para />Bikewale.Models.ModelBase m = new Bikewale.Models.ModelBase();
        /// <para />MostPopularBikeWidgetVM vm = mostPopular.GetData();
        /// </summary>
        /// <param name="bikeModels"></param>
        /// <param name="bikeType"></param>
        /// <param name="showCheckOnRoadCTA"></param>
        public MostPopularBikesWidget(IBikeModels<BikeModelEntity, int> bikeModels, EnumBikeType bikeType, bool showCheckOnRoadCTA, bool showPriceInCityCTA)
        {
            _bikeModels = bikeModels;
            _showCheckOnRoadCTA = showCheckOnRoadCTA;
            _bikeType = bikeType;
            _showPriceInCityCTA = showPriceInCityCTA;
        }

        /// <summary>
        /// <para />Created by  :   Sumit Kate on 24 Mar 2017
        /// <para />Description :   Overloaded constructor. Call the base constructor and initialize the other properties
        /// <para />Example     :
        /// <para />MostPopularBikesWidget mostPopular = new MostPopularBikesWidget(_models, EnumBikeType.New, true, PQSourceEnum.Desktop_Scooters_Landing_Check_on_road_price, 1);
        /// <para />mostPopular.TopCount = 9;
        /// <para />mostPopular.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
        /// <para />Bikewale.Models.ModelBase m = new Bikewale.Models.ModelBase();
        /// <para />MostPopularBikeWidgetVM vm = mostPopular.GetData();
        /// </summary>
        /// <param name="bikeModels">Business Layer</param>
        /// <param name="bikeType">Bike type e.g. Bike/Scooters enum</param>
        /// <param name="pqSource">PQ Source to track the source of price quotes sources</param>
        /// <param name="pageCatId">page Cat id used to push GA events</param>
        /// <param name="showCheckOnRoadCTA">Hide or show Check on road button</param>
        public MostPopularBikesWidget(IBikeModels<BikeModelEntity, int> bikeModels, EnumBikeType bikeType, bool showCheckOnRoadCTA, bool showPriceInCityCTA, PQSourceEnum pqSource, uint pageCatId)
            : this(bikeModels, bikeType, showCheckOnRoadCTA, showPriceInCityCTA)
        {
            _pqSource = pqSource;
            _pageCatId = pageCatId;
        }

        /// <summary>
        /// <para />Created by  :   Sumit Kate on 24 Mar 2017
        /// <para />Description :   Overloaded constructor. Call the base constructor and initialize the other properties
        /// <para />Example     :
        /// <para />MostPopularBikesWidget mostPopular = new MostPopularBikesWidget(_models, EnumBikeType.New, true,PQSourceEnum.Desktop_Scooters_Landing_Check_on_road_price, 1,7);
        /// <para />mostPopular.TopCount = 9;
        /// <para />mostPopular.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
        /// <para />Bikewale.Models.ModelBase m = new Bikewale.Models.ModelBase();
        /// <para />MostPopularBikeWidgetVM vm = mostPopular.GetData();
        /// </summary>
        /// <param name="bikeModels">Business Layer</param>
        /// <param name="bikeType">Bike type e.g. Bike/Scooters enum</param>
        /// <param name="pqSource">PQ Source to track the source of price quotes sources</param>
        /// <param name="pageCatId">page Cat id used to push GA events</param>
        /// <param name="showCheckOnRoadCTA">Hide or show Check on road button</param>
        /// <param name="makeId"></param>
        public MostPopularBikesWidget(IBikeModels<BikeModelEntity, int> bikeModels, EnumBikeType bikeType, bool showCheckOnRoadCTA, bool showPriceInCityCTA, PQSourceEnum pqSource, uint pageCatId, uint makeId)
            : this(bikeModels, bikeType, showCheckOnRoadCTA, showPriceInCityCTA)
        {
            _makeId = makeId;
            _pqSource = pqSource;
            _pageCatId = pageCatId;
        }

        /// <summary>
        /// <para />Created by  :   Sumit Kate on 24 Mar 2017
        /// <para />Description :   Returns MostPopularBikeWidgetVM
        /// </summary>
        /// <returns></returns>
        public MostPopularBikeWidgetVM GetData()
        {
            MostPopularBikeWidgetVM objVM = null;
            try
            {
                objVM = new MostPopularBikeWidgetVM();
                objVM.PQSourceId = _pqSource;
                objVM.PageCatId = _pageCatId;
                objVM.ShowCheckOnRoadCTA = _showCheckOnRoadCTA;
                objVM.ShowPriceInCityCTA = _showPriceInCityCTA;
                objVM.Bikes = _bikeModels.GetMostPopularBikes(_bikeType, TopCount, _makeId, CityId);
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "Bikewale.Models.MostPopularBikesWidget.GetData");
            }
            return objVM;
        }
    }
}