using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entities;

namespace BikewaleOpr.Interface.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 26 May 2017
    /// Summary : interface have functions related to states, city, areas
    /// </summary>
    public interface ILocation
    {        
        IEnumerable<StateEntityBase> GetStates();
    }
}
