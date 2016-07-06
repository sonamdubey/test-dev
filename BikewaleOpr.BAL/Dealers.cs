
using Bikewale.Notifications;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BikewaleOpr.BAL
{
    public class Dealers : IDealer
    {
        IDealerPriceQuote _dealerPQRepository = null;

        public Dealers(IDealerPriceQuote dealerPQRepository)
        {
            _dealerPQRepository = dealerPQRepository;
        }
#if unused
        public uint IsDealerExists(uint versionId, uint areaId)
        {
            uint dealerId = 0;

            double areaLattitude = 0, areaLongitude = 0;

            _dealerPQRepository.GetAreaLatLong(areaId, out areaLattitude, out areaLongitude);

            List<DealerLatLong> objDealersList = _dealerPQRepository.GetDealersLatLong(versionId, areaId);

            if (objDealersList != null && objDealersList.Count > 0)
            {
                var dealerDistList = new List<DealerCustDistanceMapping>();

                foreach (var dealer in objDealersList)
                {
                    dealerDistList.Add(new DealerCustDistanceMapping
                    {
                        dealer = dealer,
                        distance = GetDistanceBetweenTwoLocations(areaLattitude, areaLongitude, dealer.Lattitude, dealer.Longitude)
                    });
                }

                //list those dealers whose max serving distance = 0 (entire city) and user distance within dealers serving distance range.
                DealerCustDistanceMapping objNearestDealer = dealerDistList.FindAll(s => (s.dealer.ServingDistance == 0 || s.dealer.ServingDistance >= s.distance)).OrderBy(s => s.distance).FirstOrDefault();

                if (objNearestDealer != null)
                    dealerId = objNearestDealer.dealer.DealerId;
            }

            return dealerId;
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Dec 2014
        /// Summary : Round to the nearest 1/1000 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double Round(double value)
        {
            return Math.Round(value * 1000) / 1000;
        }
#endif
        #region GetAllAvailableDealer Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 23 Oct 2015
        /// Summary : To get All Dealer who can serve for perticulur area
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public IEnumerable<uint> GetAllAvailableDealer(uint versionId, uint areaId)
        {
            IEnumerable<uint> objDealerList = null;

            double areaLattitude = 0, areaLongitude = 0;

            try
            {
                _dealerPQRepository.GetAreaLatLong(areaId, out areaLattitude, out areaLongitude);

                List<DealerLatLong> objDealersList = _dealerPQRepository.GetDealersLatLong(versionId, areaId);

                if (objDealersList != null && objDealersList.Count > 0)
                {
                    var dealerDistList = new List<DealerCustDistanceMapping>();

                    foreach (var dealer in objDealersList)
                    {
                        dealerDistList.Add(new DealerCustDistanceMapping
                        {
                            dealer = dealer,
                            distance = GetDistanceBetweenTwoLocations(areaLattitude, areaLongitude, dealer.Lattitude, dealer.Longitude)
                        });
                    }

                    objDealerList = dealerDistList.FindAll(s => (s.dealer.ServingDistance == 0 || s.dealer.ServingDistance >= s.distance)).OrderBy(s => s.distance).Select(s => s.dealer.DealerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DealerPriceQuote.GetAllAvailableDealer");
                objErr.SendMail();
            }
            return objDealerList;
        }   //End of GetAllAvailableDealer
        #endregion

        /// <summary>
        /// Created By : Sadhana Upadhyay on 26 Oct 2015
        /// Summary : To dealer price quote details like offers, price, availability, booking amount etc.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <param name="dealerIds"></param>
        /// <returns></returns>
        public IEnumerable<DealerPriceQuoteDetailed> GetDealerPriceQuoteDetail(uint versionId, uint cityId, string dealerIds)
        {
            IList<DealerPriceQuoteDetailed> objDealer = null;
            IList<BikeAvailabilityByColor> objColorAvail = null;
            DealerPriceQuoteEntity objDealerEntity = null;

            string[] dealers = null;
            try
            {
                if (!String.IsNullOrEmpty(dealerIds))
                    dealers = dealerIds.Split(',');
                objDealerEntity = _dealerPQRepository.GetPriceQuoteForAllDealer(versionId, cityId, dealerIds);

                if (dealers != null && dealers.Length > 0 && objDealerEntity != null)
                {
                    objDealer = new List<DealerPriceQuoteDetailed>();
                    foreach (string dealer in dealers)
                    {
                        DealerPriceQuoteDetailed dealerPriceQuoteDetails = new DealerPriceQuoteDetailed();

                        dealerPriceQuoteDetails.OfferList = from offer in objDealerEntity.OfferList
                                                            where offer.DealerId == Convert.ToUInt32(dealer)
                                                            select offer;

                        dealerPriceQuoteDetails.PriceList = from price in objDealerEntity.PriceList
                                                            where price.DealerId == Convert.ToUInt32(dealer)
                                                            select price;

                        var ColorListForDealer = from color in objDealerEntity.BikeAvailabilityByColor
                                                 where color.DealerId == 0 || color.DealerId == Convert.ToUInt32(dealer)
                                                 group color by color.ColorId into newgroup
                                                 orderby newgroup.Key
                                                 select newgroup;


                        objColorAvail = new List<BikeAvailabilityByColor>();
                        foreach (var color in ColorListForDealer)
                        {
                            BikeAvailabilityByColor objAvail = new BikeAvailabilityByColor();

                            objAvail.ColorId = color.Key;
                            objAvail.DealerId = Convert.ToUInt32(dealer);

                            IList<string> HexCodeList = new List<string>();
                            foreach (var colorList in color)
                            {
                                objAvail.ColorName = colorList.ColorName;
                                objAvail.NoOfDays = colorList.NoOfDays;
                                objAvail.VersionId = colorList.VersionId;

                                HexCodeList.Add(colorList.HexCode);
                            }
                            objAvail.HexCode = HexCodeList;

                            objColorAvail.Add(objAvail);
                        }

                        dealerPriceQuoteDetails.AvailabilityByColor = objColorAvail;

                        dealerPriceQuoteDetails.DealerDetails = objDealerEntity.DealerDetails.Where(ss => ss.DealerId == Convert.ToUInt32(dealer)).Select(ss => ss.Dealer).FirstOrDefault();
                        dealerPriceQuoteDetails.Availability = objDealerEntity.DealerDetails.Where(ss => ss.DealerId == Convert.ToUInt32(dealer)).Select(ss => ss.Availability).FirstOrDefault();
                        dealerPriceQuoteDetails.BookingAmount = objDealerEntity.DealerDetails.Where(ss => ss.DealerId == Convert.ToUInt32(dealer)).Select(ss => ss.BookingAmount).FirstOrDefault();

                        objDealer.Add(dealerPriceQuoteDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DealerPriceQuote.GetDealerPriceQuoteDetail");
                objErr.SendMail();
            }
            return objDealer;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Dec 2014
        /// Summary : Function to get the distance between two co-ordinates.
        /// This uses the ‘haversine’ formula to calculate the great-circle distance between two points – that is, the shortest distance over the earth’s surface.
        /// Haversine formula:	a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2)
        ///                     c = 2 ⋅ atan2( Sqrt(a), Sqrt(1−a) )
        ///                     d = R ⋅ c
        ///                     where	φ is latitude, λ is longitude, R is earth’s radius (mean radius = 6,373km).
        /// note that angles need to be in radians to pass to trig functions!
        /// </summary>
        /// <param name="lattitude1">Value should be in degrees. E.g. lattitude1 = 38.898556</param>
        /// <param name="longitude1">Value should be in degrees. E.g. longitude1 = -77.037852</param>
        /// <param name="lattitude2">Value should be in degrees. E.g. lattitude2 = 38.897147</param>
        /// <param name="longitude2">Value should be in degrees. E.g. longitude2 = -77.043934</param>
        /// <returns>Distance between the points in kilometeres</returns>
        public double GetDistanceBetweenTwoLocations(double lattitude1, double longitude1, double lattitude2, double longitude2)
        {
            var radEarth = 6373; // mean radius of the earth (km) at 39 degrees from the equator
            double lat1, lon1, lat2, lon2, dlat, dlon, a, gtCirDistance, distance;

            // convert coordinates to radians
            lat1 = DegToRad(lattitude1);
            lon1 = DegToRad(longitude1);
            lat2 = DegToRad(lattitude2);
            lon2 = DegToRad(longitude2);

            // find the differences between the coordinates
            dlon = lon2 - lon1;
            dlat = lat2 - lat1;

            // Haversine formula
            a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);

            gtCirDistance = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));  // great circle distance in radians

            distance = radEarth * gtCirDistance; // great circle distance in km

            // Distance to be round off if necessory.
            // distance = Round(distance);

            return distance;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Dec 2014
        /// Summary : convert degrees to radians
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        private double DegToRad(double deg)
        {
            double rad = deg * Math.PI / 180; // radians = degrees * pi/180
            return rad;
        }
#if unused
        /// <summary>
        /// Created by  :   Sumit Kate on 21 Mar 2016
        /// Description :   Checks whether any Subscription Model Dealer is present.
        /// Modified by :   Sumit Kate on 22 Mar 2016
        /// Description :   Surrounded code with try..catch
        /// Modified By : Vivek Gupta on 12-04-2016, new sp BW_GetDealersLatLong_12042016
        /// Description : we are fetching nearest dealer from database itself . we have commute distance availbele in database.
        ///               stopeed using ariel distance calcualtion
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns>dealer id</returns>
        public uint IsSubscribedDealerExists(uint versionId, uint areaId)
        {
            uint dealerId = 0;

            if (versionId > 0 && areaId > 0)
            {
                try
                {
                    DealerLatLong objDealersList = _dealerPQRepository.GetCampaignDealersLatLong(versionId, areaId);

                    if (objDealersList != null)
                    {
                        dealerId = objDealersList.DealerId;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "AutoBiz.BAL.BW.BikeBooking.Dealers.IsSubscribedDealerExists");
                    objErr.SendMail();
                }
            }

            return dealerId;
        }
        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 28-04-2016
        /// Desc : to check dealer exists for areaId and version id and isSecondaryDealers availble
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public DealerInfo IsSubscribedDealerExistsV3(uint versionId, uint areaId)
        {
            DealerInfo objDealersList = null;

            if (versionId > 0 && areaId > 0)
            {
                try
                {
                    objDealersList = _dealerPQRepository.GetCampaignDealersLatLongV3(versionId, areaId);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "AutoBiz.BAL.BW.BikeBooking.Dealers.IsSubscribedDealerExists");
                    objErr.SendMail();
                }
            }

            return objDealersList;
        }
#endif
    } // class
}   // namespace


