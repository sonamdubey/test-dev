using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 03 Aug 2017
    /// Description :   Interface for BAL which performs operations on version availability.
    /// </summary>
    public interface IVersionAvailability
    {
        bool SaveVersionAvailability(uint dealerId, IEnumerable<uint> bikeVersionIds, IEnumerable<uint> numberOfDays);
        bool DeleteVersionAvailability(uint dealerId, IEnumerable<uint> bikeVersionIds);
    }
}
