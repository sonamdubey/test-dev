using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Geolocation
{
    /// <summary>
    /// interface for city by latlong api
    /// written by Natesh kumar on 5/11/14
    /// </summary>
    public interface ILatLongCityRepository
    {
        City GetCityDetailsByLatLong(LatLongURI querystring);
    }
}
