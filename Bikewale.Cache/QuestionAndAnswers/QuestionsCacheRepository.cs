using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                qnaCount = _cache.GetFromCache<uint>(key, new TimeSpan(24, 0, 0), () => (uint) GetQuestionIdsByModelId(modelId).Count());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Cache.QuestionAndAnswers.GetQuestionCountByModelId, Model Id: {0}", modelId));
            }

            return qnaCount;
        }

    }
}
