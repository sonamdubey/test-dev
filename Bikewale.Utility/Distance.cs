using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class Distance
    {
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
    }
}
