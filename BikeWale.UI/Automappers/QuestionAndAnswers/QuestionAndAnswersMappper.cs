
using AutoMapper;
using Bikewale.Entities.QuestionAndAnswers;
using QuestionsAnswers.Entities;
namespace Bikewale.Automappers.QuestionAndAnswers
{

    public class QuestionAndAnswersMappper
    {

        public static QuestionsAnswers.Entities.ClientInfo Convert(Entities.QuestionAndAnswers.BWClientInfo clientInfo)
        {
            Mapper.CreateMap<BWClientInfo, ClientInfo>();
            return Mapper.Map<BWClientInfo, ClientInfo>(clientInfo);
        }

        public static QuestionsAnswers.Entities.Answer Convert(Entities.QuestionAndAnswers.Answer answer)
        {
            Mapper.CreateMap<Entities.QuestionAndAnswers.Answer, QuestionsAnswers.Entities.Answer>();
            return Mapper.Map<Entities.QuestionAndAnswers.Answer, QuestionsAnswers.Entities.Answer>(answer);
        }
    }
}
