using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.Forums;
using Carwale.Entity;
using Carwale.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.Forums
{
    public class ForumsCache
    {
        protected readonly ICacheManager _cacheProvider;

        public ForumsCache()
        {
            _cacheProvider = new CacheManager();
        }

        public DataSet GetAllForums() {

            var forumDetails = new ForumsDAL();
            return _cacheProvider.GetFromCache<DataSet>("Forums",
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => forumDetails.GetAllForums());
        }

        public DataSet LoadCategoryViews() {

            var userDAL = new UserDAL();
            return _cacheProvider.GetFromCache<DataSet>("Forums-Category-Views",
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => userDAL.LoadCategoryViews());
        }

        public string GetUsersCount() {

            var userDAL = new UserDAL();
            return _cacheProvider.GetFromCache<string>("Forums-Users-Count",
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => userDAL.GetUsersCount());
        }


        public DataSet GetForumDetails(int forumId, int startIndex, int endIndex, int pageNo)
        {

            var forumDetails = new ForumsDAL();
            return _cacheProvider.GetFromCache<DataSet>(string.Format("Forums-Id-{0}-Page-{1}", forumId, pageNo),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => forumDetails.GetForumDetails(forumId,startIndex,endIndex));
        }

        public bool CheckUserHandle(string userId) {

            var userDAL = new UserDAL();
            return _cacheProvider.GetFromCache<bool>(string.Format("Forums-UserHandle-{0}", userId),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => userDAL.CheckUserHandle(userId));
        }

        public bool CheckCustomerIds(string ids) {

            var userDAL = new UserDAL();
            return _cacheProvider.GetFromCache<bool>(string.Format("Forums-CustomerIds-{0}", ids),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => userDAL.CheckCustomerIds(ids));
        }

        public ThreadBasicInfo GetAllForums(string threadId) {

            var threadDAL = new ThreadsDAL();
            return _cacheProvider.GetFromCache<ThreadBasicInfo>(string.Format("Forums-Thread-{0}", threadId),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => threadDAL.GetAllForums(threadId));

           
        }

        public int FindPost(int post, int threadId) {

            var postDetails = new PostDAL();
            return _cacheProvider.GetFromCache<int>(string.Format("Forums-Thread-{0}-Post-{1}", threadId, post),
              CacheRefreshTime.DefaultRefreshTime(),
                  () => postDetails.FindPost(post,threadId));
        }

        public bool IsUserBanned(string customerId) {

            var userDAL = new UserDAL();
            return _cacheProvider.GetFromCache<bool>(string.Format("Forums-Userbanned-{0}", customerId),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => userDAL.IsUserBanned(customerId));
        }

        public DataSet GetThreadDetails(int threadId, int startIndex, int endIndex,int pageNo) {

            var threadDAL = new ThreadsDAL();
            return _cacheProvider.GetFromCache<DataSet>(string.Format("Forums-Thread-{0}-Page-{1}", threadId,pageNo),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => threadDAL.GetThreadDetails(threadId,startIndex,endIndex));
        }

        public void InvalidatePaginationCache(string keytemplate,int noOfpage)
        {
            if (keytemplate != null)
            {
                for (int pagecount = 1; pagecount <= noOfpage; pagecount++)
                {
                    _cacheProvider.ExpireCache(string.Format("{0}-{1}",keytemplate,pagecount));
                }
            }
            return;
        }

        public bool IsModerator(string customerId)
        {
            var threadDAL = new ThreadsDAL();
            return _cacheProvider.GetFromCache<bool>(string.Format("IsModerator-{0}", customerId),
                          CacheRefreshTime.EODExpire(),
                              () => threadDAL.GetModeratorLoginStatus(customerId));
        }
    }
}
