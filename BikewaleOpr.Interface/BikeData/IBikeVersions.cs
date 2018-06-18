using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;
namespace BikewaleOpr.Interface.BikeData
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th Oct 2016 
    /// Description : Interface for bike versions 
    /// </summary>
    public interface IBikeVersions
    {
        IEnumerable<BikeVersionEntityBase> GetVersions(uint modelId, string requestType);

		/// <summary>
		/// Written By : Ashish G. Kamble on 12 June 2018
		/// Summary : Function to get basic data for a given version id
		/// </summary>
		/// <param name="versionId"></param>
		/// <returns></returns>
		BikeVersionEntity GetVersionDetails(uint versionId);		
	}
}
