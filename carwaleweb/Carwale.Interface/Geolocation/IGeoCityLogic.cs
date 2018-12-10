using Carwale.DTOs;
using System.Collections.Generic;

namespace Carwale.Interfaces.Geolocation
{
    public interface IGeoCityLogic
    {
        Suggest GetCityFromLatLong(string latitude, string longitude);
    }
}
