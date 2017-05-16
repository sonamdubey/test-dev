
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 08-May-2017
    /// Entity for Splash screen
    /// </summary>

    [Serializable, DataContract]
    public class SplashScreenEntity
    {
        public string SplashImgUrl { get; set; }
    }
}
