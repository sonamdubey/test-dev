using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuestionsAnswers.DAL;

namespace UnitTesting.BAL
{
    /// <summary>
    /// Created by
    /// </summary>
    [TestClass]
    public class QuestionsBAL
    {
        Mock<IQuestionsRepository> questionsRepo;

        [TestMethod]
        public void GetQuestions()
        {
            ////Arrange
            //questionsRepo = new Mock<IQuestionsRepository>();

            //QuestionsFilter filter = new QuestionsFilter()
            //{
            //    CustomerEmails = new List<string> { "sanskar.gupta@carwale.com", "sumit.kate@carwale.com" },
            //};


            ////Act
            //Questions questionsBAL = new Questions(questionsRepo.Object);
            //IEnumerable<Question> questions = questionsBAL.GetQuestions(filter, 1, 10, 2);


            ////Assert
            //Assert.IsNotNull(questions);
            //foreach (Question q in questions)
            //{
            //    if (q.AskedBy != null)
            //    {
            //        Assert.IsNotNull(q.AskedBy.Email);
            //    }
            //}
        }

        [TestMethod]
        public void GetQuestionsByEmailId()
        {
            ////Arrange
            //questionsRepo = new Mock<IQuestionsRepository>();
            //string email = "sanskar.gupta@carwale.com";

            //Question q1 = new Question();
            //List<Question> l = new List<Question>();
            //l.Add(q1);
            //IEnumerable<Question> expectedQuestions = l;
            //questionsRepo.SetupSequence(questionsDAL => questionsDAL.GetQuestionsByEmailId(It.IsAny<string>(), It.IsAny<byte>()))
            //    .Returns(null)
            //    .Returns(expectedQuestions)
            //    .Returns(null);

            ////Act
            //Questions questionsBAL = new Questions(questionsRepo.Object);
            //IEnumerable<Question> questions = questionsBAL.GetQuestionsByEmailId(email, 2);


            ////Assert
            //Assert.IsNotNull(questions);
            //foreach (Question q in questions)
            //{
            //    if (q.AskedBy != null)
            //    {
            //        Assert.AreEqual(email, q.AskedBy.Email);
            //    }
            //}
        }
    }
}
