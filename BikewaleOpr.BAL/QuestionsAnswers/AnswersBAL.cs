using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Notifications;
using QuestionsAnswers.BAL;
using BikewaleOpr.Interface.QuestionsAnswers;
using QuestionsAnswers.Entities;

namespace BikewaleOpr.BAL.QuestionsAnswers
{
    public class AnswersBAL : IAnswersBAL
    {
        private readonly IAnswers _answers = null;
        private readonly IQuestionsAndAnswersCache _questionsAnswersCacheRepository = null;
        public AnswersBAL(IAnswers answers, IQuestionsAndAnswersCache questionsAnswersCacheRepository)
        {
            _answers = answers;
            _questionsAnswersCacheRepository = questionsAnswersCacheRepository;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Function to get all the answers for a particular question.
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public IEnumerable<Answer> GetAnswers(string questionId)
        {
            IEnumerable<Answer> answers = null;
            try
            {
                answers = _answers.GetAnswers(questionId);
                HashSet<uint> internalUserIDs = _questionsAnswersCacheRepository.GetInternalUserIDs();

                if(answers == null || answers.Count()==0 || internalUserIDs == null)
                {
                    return answers;
                }

                Customer customer = null;
                foreach (Answer answer in answers)
                {
                    if (answer != null && (customer = answer.AnsweredBy) != null && internalUserIDs.Contains(customer.Id))
                    {
                        // The user who posted this answer is an Internal User
                        customer.IsInternalUser = true;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.QuestionsAnswers.AnswersBAL.GetAnswers(QuestionId:{0})", questionId));
            }

            return answers;
        }
    }
}
