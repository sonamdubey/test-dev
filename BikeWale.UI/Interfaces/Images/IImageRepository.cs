
using System;
namespace Bikewale.Interfaces.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   Image repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IImageRepository<T, U> : IRepository<T, U>
    {
        new UInt64 Add(T t);
    }
}
