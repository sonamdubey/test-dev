using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Created By: Aditi Srivastava on 17 Oct 2016
    /// Description: list of all colors by version
    /// </summary>
    [Serializable, DataContract]
    public class BikeColorsbyVersionDTO
    {
        public IEnumerable<BikeColorsbyVersion> VersionColors { get; set; }

    }
}
