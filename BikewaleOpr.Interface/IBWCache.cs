
using BikewaleOpr.Entity;
using System.Collections.Generic;
namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Created by  :   Sumit Kate on 08 May 2018
    /// Description :   Interface for Bikewale Cache clear
    /// </summary>
    public interface IBWCache
    {
        void Clear(CacheContents cacheContent, IDictionary<string, string> keyValues);
    }
}
