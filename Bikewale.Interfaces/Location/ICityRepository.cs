using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Author  : Kartik Rathod on 8 jun 2018
    /// </summary>
    public interface ICityRepository
    {
        ICollection<CityEntityBase> GetCitiesByStateName(string stateName);
    }
}
