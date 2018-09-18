using Bikewale.Entities;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Models.QuestionAndAnswers;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Bikewale.Models.QuestionsAnswers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Sep 2018
    /// Description :   Model for Unanswered Questions
    /// </summary>
    public class UnansweredQuestionModel
    {
        private readonly IQuestions _questions;
        private const byte SINGLE_QUESTIONS = 1;
        private const byte TEN_QUESTIONS = 10;

        public string ReturnUrl { get; set; }
        public UnansweredQuestionModel(IQuestions questions)
        {
            _questions = questions;
        }
        public UnansweredQuestionVM GetData(uint modelId, string emailId)
        {
            UnansweredQuestionVM viewModel = null;
            if (modelId > 0)
            {
                var questions = _questions.GetRemainingUnansweredQuestions(modelId, SINGLE_QUESTIONS, emailId);
                if (questions != null && questions.Any())
                {
                    viewModel = new UnansweredQuestionVM();
                    viewModel.Question = questions.First();
                }
            }
            return viewModel;
        }

        public UnansweredQuestionsVM GetData(uint modelId, Entities.QuestionAndAnswers.Sources source, Platforms Platform, string userEmail, string userName)
        {
            UnansweredQuestionsVM viewModel = null;
            if (modelId > 0)
            {
                var questions = _questions.GetRemainingUnansweredQuestions(modelId, TEN_QUESTIONS, userEmail);
                if (questions != null && questions.Any())
                {
                    viewModel = new UnansweredQuestionsVM();

                    ICollection<QuestionUrl> questionUrls = new Collection<QuestionUrl>();

                    foreach (var question in questions)
                    {
                        string queryString = Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format(@"userEmail={0}&userName={1}&questionId={2}&modelId={3}", userEmail, userName, question.Id, modelId));
                        string decString = Bikewale.Utility.TripleDES.DecryptTripleDES(queryString);
                        questionUrls.Add(new QuestionUrl
                        {
                            QuestionData = question,
                            AnsweringUrl = String.Format("{0}/{1}questions-and-answers/answer/?q={2}&source={3}&returnUrl={4}", BWConfiguration.Instance.BwHostUrl, (Platform == Platforms.Mobile ? "m/" : ""), queryString, (ushort)source, ReturnUrl)
                        });
                    }
                    viewModel.Questions = questionUrls;
                }
            }
            return viewModel;
        }
    }
}