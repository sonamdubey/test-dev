
using AutoMapper;
using Bikewale.Entities.QuestionAndAnswers;
using QuestionsAnswers.Entities;
namespace Bikewale.Automappers.QuestionAndAnswers
{

    public class QuestionAndAnswersMappper
    {

        public static QuestionsAnswers.Entities.ClientInfo Convert(Entities.QuestionAndAnswers.BWClientInfo clientInfo)
        {
          
            return Mapper.Map<BWClientInfo, ClientInfo>(clientInfo);
        }

        public static QuestionsAnswers.Entities.Answer Convert(Entities.QuestionAndAnswers.Answer answer)
        {
         
            return Mapper.Map<Entities.QuestionAndAnswers.Answer, QuestionsAnswers.Entities.Answer>(answer);
        }
    }
}
