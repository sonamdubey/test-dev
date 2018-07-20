

using System.Collections.Generic;

namespace BikewaleOpr.Interface.Amp
{
    /// <summary>
    /// Created by  : Pratibha Verma on 17 July 2018
    /// Description : Interface for AMPCache 
    /// </summary>
    public interface IAmpCache
    {
        void UpdateMakeAmpCache(uint makeId);
        void UpdateModelAmpCache(uint modelId);
        void UpdateModelAmpCache(IEnumerable<uint> modelIds);
    }
}
