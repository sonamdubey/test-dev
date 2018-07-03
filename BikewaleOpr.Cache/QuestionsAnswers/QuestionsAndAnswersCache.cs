using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using BikewaleOpr.Entity.Users;
using BikewaleOpr.Interface.QuestionsAnswers;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Cache.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 19 June 2018
    /// </summary>
    public class QuestionsAndAnswersCache : IQuestionsAndAnswersCache
    {
        private readonly ICacheManager _cache;
        private readonly IUsersRepository _usersRepository;

        /// <summary>
        /// Constructor to initialize the dependencies
        /// </summary>
        public QuestionsAndAnswersCache(ICacheManager cache, IUsersRepository usersRepository)
        {
            _cache = cache;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Cache to get internal users.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetInternalUsers()
        {
            IEnumerable<User> internalUsers = null;

            try
            {
                internalUsers = _cache.GetFromCache<IEnumerable<User>>("BW_QnA_InternalUsers", new TimeSpan(1, 0, 0, 0, 0), () => _usersRepository.GetInternalUsers());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Cache.QuestionsAnswers.QuestionsAndAnswersCache.GetInternalUsers()");
            }

            return internalUsers;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Cache to get HashSet of IDs of internal users.
        /// </summary>
        /// <returns></returns>
        public HashSet<uint> GetInternalUserIDs()
        {
            HashSet<uint> internalUserIDs = null;
            try
            {
                internalUserIDs = _cache.GetFromCache<HashSet<uint>>("BW_QnA_InternalUserIDs", new TimeSpan(1, 0, 0, 0, 0), () => GetInternalUserIDsFromList());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Cache.QuestionsAnswers.QuestionsAndAnswersCache.GetInternalUserIDs()");
            }

            return internalUserIDs;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Function to get HashSet of Internal User IDs from the list of internal user objects.
        /// </summary>
        /// <returns></returns>
        public HashSet<uint> GetInternalUserIDsFromList()
        {
            HashSet<uint> internalUserIDs = new HashSet<uint>();

            try
            {
                IEnumerable<User> internalUsers = GetInternalUsers();


                if (internalUsers != null)
                {
                    foreach (User user in internalUsers)
                    {
                        internalUserIDs.Add(user.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Cache.QuestionsAnswers.QuestionsAndAnswersCache.GetInternalUserIDsFromList()");
            }

            return internalUserIDs;
        }
    }
}
