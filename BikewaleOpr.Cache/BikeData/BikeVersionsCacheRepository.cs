using System;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikeData;

namespace BikewaleOpr.Cache.BikeData
{
	/// <summary>
	/// Created By : Ashish G. Kamble
	/// Summary : Class have functions related to bike versions cache
	/// </summary>
	public class BikeVersionsCacheRepository : IBikeVersionsCacheRepository
	{
		private readonly ICacheManager _cache = null;
		private readonly IBikeVersions _versionsRepo = null;

		/// <summary>
		/// Constructor to initialize the dependencies
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="versionsRepo"></param>
		public BikeVersionsCacheRepository(ICacheManager cache, IBikeVersions versionsRepo)
		{
			_cache = cache;
			_versionsRepo = versionsRepo;
		}

		/// <summary>
		/// Written By : Ashish G. Kamble on 12 June 2018
		/// Summary : Function to get basic data for a given version id. Data is cached for 10 min
		/// </summary>
		/// <param name="versionId">Bike version id</param>
		/// <returns>Returns BikeVersionEntity</returns>
		public BikeVersionEntity GetVersionDetails(uint versionId)
		{
			BikeVersionEntity objVersion = null;

			try
			{
				if (versionId > 0)
				{
					string key = String.Format("BW_VersionDetails_Id_{0}", versionId);

					objVersion = _cache.GetFromCache<BikeVersionEntity>(key, new TimeSpan(0, 0, 10, 0), () => _versionsRepo.GetVersionDetails(versionId));
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("BikewaleOpr.Cache.BikeData.BikeVersionsCacheRepository.GetVersionDetails_VersionId_{0} - Database Query", versionId));
			}

			return objVersion;
		}

	}	// class
}	// namespace
