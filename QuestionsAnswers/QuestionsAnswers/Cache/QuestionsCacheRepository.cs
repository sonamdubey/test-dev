using QuestionsAnswers.DAL;
using QuestionsAnswers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsAnswers.Cache
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
        /// Description : Function to cache questions on a per id basis and returns a list of question details.
        /// </summary>
        /// <param name="questionIds"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetQuestionDataByQuestionIds(IEnumerable<string> questionIds)
        {
            IEnumerable<Question> questions = null;
            Dictionary<string, string> dictIdKeys = null;
            
            try
            {
                dictIdKeys = new Dictionary<string, string>();
                foreach (string id in questionIds)
                {
                    dictIdKeys.Add(id, string.Format("QNA_Q_{0}", id));
                }
                Func<string, IEnumerable<Question>> fnCallback = _questionsRepository.GetQuestionDataByQuestionId;

                questions = _cache.GetListFromCache<Question>(dictIdKeys,
                    new TimeSpan(24, 0, 0),
                    fnCallback);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return questions;
        }
    }
}
