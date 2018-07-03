using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestionsAnswers.Entities;
using QuestionsAnswers.DAL;

namespace QuestionsAnswers.BAL
{
    public class Answers : IAnswers
    {
        private static IAnswersRepository _answersRepository = null;
        public Answers(IAnswersRepository answersRepository)
        {
            _answersRepository = answersRepository;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Function to get all the answers for a particular question
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public IEnumerable<Answer> GetAnswers(string questionId)
        {
            IEnumerable<Answer> answers = null;
            try
            {
                answers = _answersRepository.GetAnswers(questionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return answers;
        }
    }
}
