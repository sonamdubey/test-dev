using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 26 May 2017
    /// Summary : interface have functions related to states, city, areas
    /// </summary>
    public interface ILocation
    {        
        IEnumerable<StateEntityBase> GetStates();
        IEnumerable<CityNameEntity> GetDealerCities();
        IEnumerable<CityNameEntity> GetCitiesByState(uint stateId);
        IEnumerable<CityNameEntity> GetAllCities();
    }
}
