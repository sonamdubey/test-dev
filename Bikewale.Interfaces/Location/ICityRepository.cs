using Bikewale.Entities.Location;
using System.Collections.ObjectModel;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Author  : Kartik Rathod on 8 jun 2018
    /// </summary>
    public interface ICityRepository
    {
        Collection<CityEntityBase> GetCitiesByStateName(string stateName);
    }
}
