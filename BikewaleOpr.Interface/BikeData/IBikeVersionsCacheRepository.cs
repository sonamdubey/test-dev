using BikewaleOpr.Entities.BikeData;

namespace BikewaleOpr.Interface.BikeData
{
	/// <summary>
	/// Created By : Ashish G. Kamble
	/// </summary>
	public interface IBikeVersionsCacheRepository
	{
		BikeVersionEntity GetVersionDetails(uint versionId);
	}
}
