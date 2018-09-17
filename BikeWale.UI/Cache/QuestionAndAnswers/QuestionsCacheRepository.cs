using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Cache.QuestionAndAnswers
{
    public class QuestionsCacheRepository : IQuestionsCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IQuestionsRepository _questionsRepository;

        public QuestionsCacheRepository(ICacheManager cache, IQuestionsRepository questionsRepository)
        {
            _cache = cache;
            _questionsRepository = questionsRepository;
        }

        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description: Cache function to get all the question ids for a model id.
        /// Modified by : Snehal Dange on 7th August 2018
        /// Desc : Added call GetHashQuestionMapping which gets hash question mapping
        /// Modified By : Deepak Israni on 16 August 2018
        /// Description : Reverted back to GetQuestionIdsByModelId to preserve ordering.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetQuestionIdsByModelId(uint modelId)
        {
            IEnumerable<string> questions = null;

            string key = String.Format("BW_Questions_Model_{0}", modelId);

            try
            {
                questions = _cache.GetFromCache<IEnumerable<string>>(key, new TimeSpan(24, 0, 0), () => _questionsRepository.GetQuestionIdsByModelId(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Cache.QuestionAndAnswers.GetQuestionIdsByModelId, Model Id: {0}", modelId));
            }

            return questions;
        }


        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description : Function to get the count of Questions for a certain model id.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public uint GetQuestionCountByModelId(uint modelId)
        {
            uint qnaCount = 0;
            string key = String.Format("BW_Model_{0}_QuestionCount", modelId);

            try
            {
                qnaCount = _cache.GetFromCache<uint>(key, new TimeSpan(24, 0, 0), () => (uint)GetQuestionIdsByModelId(modelId).Count());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Cache.QuestionAndAnswers.GetQuestionCountByModelId, Model Id: {0}", modelId));
            }

            return qnaCount;
        }

        /// <summary>
        /// Created By : Deepak Israni on 13 July 2018
        /// Description : Cache function to get all the unanswered question ids for a certain model.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetUnansweredQuestionIdsByModelId(uint modelId)
        {
            IEnumerable<string> questions = null;

            string key = String.Format("BW_UnansweredQuestions_Model_{0}", modelId);

            try
            {
                questions = _cache.GetFromCache<IEnumerable<string>>(key, new TimeSpan(24, 0, 0), () => _questionsRepository.GetUnansweredQuestionIdsByModelId(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Cache.QuestionAndAnswers.GetUnansweredQuestionIdsByModelId, Model Id: {0}", modelId));
            }

            return questions;
        }


        /// <summary>
        /// Created by : Snehal Dange on 7th August 2018'
        /// Desc : Get the Hash-guid mapping for particular modelId .Version the cache key to store hash question for a model instead of only questions
        /// Modified by: Dhruv Joshi
        /// Dated: 10th August 2018
        /// Description: Cached wrapper class containing hashtables for both way (questionid-hash) mapping
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public HashQuestionIdMappingTables GetHashQuestionMapping(uint modelId)
        {

            HashQuestionIdMappingTables hashQuesMap = null;
            string key = String.Format("BW_Questions_M_{0}_V1", modelId);

            try
            {
                hashQuesMap = _cache.GetFromCache<HashQuestionIdMappingTables>(key, new TimeSpan(24, 0, 0), () => _questionsRepository.GetHashQuestionMappingByModelId(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Cache.QuestionAndAnswers.GetHashQuestionMapping, Model Id: {0}", modelId));
            }

            return hashQuesMap;

        }
    }
}
