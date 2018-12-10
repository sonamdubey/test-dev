using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS.UserReviews;
using System.Collections.Generic;

namespace Carwale.Cache.CMS.UserReviews
{
    public class UserReviewsCache : IUserReviewsCache
    {

        private IUserReviewsRepository _userReviewRepo;
        private ICacheManager _memcache;
        
        public UserReviewsCache(ICacheManager memcache, IUserReviewsRepository userReviewRepo)
        {
            _userReviewRepo = userReviewRepo;
            _memcache = memcache;
        }

        public List<UserReviewEntity> GetUserReviewsList(int makeid ,int modelId, int versionId, int start, int end, int sortCriteria)
        {
            return _memcache.GetFromCache<List<UserReviewEntity>>(string.Format("UserReviewList-{0}-{1}-{2}-{3}-{4}-{5}",makeid, modelId, versionId, start, end, sortCriteria),
                        CacheRefreshTime.ThreeHoursExpire(), () => _userReviewRepo.GetUserReviewsList(makeid,modelId, versionId, start, end, sortCriteria));
        }

        public UserReviewDetail GetUserReviewDetailById(int reviewId, CMSAppId applicationId)
        {
            return _memcache.GetFromCache<UserReviewDetail>(string.Format("UserReviewDetail-{0}",reviewId), CacheRefreshTime.DefaultRefreshTime(),
                                                () => _userReviewRepo.GetUserReviewDetailById(reviewId, applicationId));
        }

        public string GetUserReviewedIdsByModel(int modelId)
        {
            return _memcache.GetFromCache<string>(string.Format("UserReviewedIdsByModel_{0}", modelId), CacheRefreshTime.DefaultRefreshTime(),
                () => _userReviewRepo.GetUserReviewedIdsByModel(modelId));
        }

        public List<CarReviewBaseEntity> GetMostReviewedCars()
        {
            return _memcache.GetFromCache<List<CarReviewBaseEntity>>("GetMostReviewedCars", CacheRefreshTime.DefaultRefreshTime(),
                () => _userReviewRepo.GetMostReviewedCars());
        }

        public List<UserReviewEntity> GetUserReviewsByType(UserReviewsSorting type)
        {
            return _memcache.GetFromCache<List<UserReviewEntity>>(string.Format("GetUserReviewsByType-{0}",type), CacheRefreshTime.DefaultRefreshTime(),
                () => _userReviewRepo.GetUserReviewsByType(type));
        }
		public int ProcessEmailVerfication(bool isEmailVerified, int userId)
		{
			return _memcache.GetFromCache<int>(string.Format("ProcessEmailVerfication-{0}", userId), CacheRefreshTime.DefaultRefreshTime(),
				() => _userReviewRepo.ProcessEmailVerfication(isEmailVerified, userId));
		}
	}
}
