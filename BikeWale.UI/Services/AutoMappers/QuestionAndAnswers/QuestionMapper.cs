using AutoMapper;
using Bikewale.DTO.Customer;
using Bikewale.DTO.QuestionAndAnswers;
using Bikewale.Entities.Customer;
using Bikewale.Entities.QuestionAndAnswers;

namespace Bikewale.Service.AutoMappers.QuestionAndAnswers
{
    public class QuestionMapper
    {
        /// <summary>
        /// Created By : Deepak Israni on 12 June 2018
        /// Description: Automapper to convert QuestionDTO to Question entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Question Convert(QuestionDTO entity)
        {
            Mapper.CreateMap<CustomerBase, CustomerEntityBase>();
            Mapper.CreateMap<AnswerDTO, Answer>();
            Mapper.CreateMap<Bikewale.DTO.QuestionAndAnswers.Tag, Bikewale.Entities.QuestionAndAnswers.Tag>();
            Mapper.CreateMap<QuestionDTO, Question>();

            return Mapper.Map<QuestionDTO, Question>(entity);
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 22 June 2018
        /// Description : Function to map `Questions` entity to `QuestionsDTO`.
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        public static QuestionsDTO Convert(Questions questions)
        {
            Mapper.CreateMap<CustomerEntityBase, CustomerBase>();
            Mapper.CreateMap<Answer, AnswerDTO>();
            Mapper.CreateMap<Entities.QuestionAndAnswers.Tag, Bikewale.DTO.QuestionAndAnswers.Tag>();
            Mapper.CreateMap<QuestionAnswer, QuestionAnswerDTO>();
            Mapper.CreateMap<QuestionBase, QuestionDTO>();
            Mapper.CreateMap<Questions, QuestionsDTO>()
                .ForMember(d => d.QuestionsAnswersList, opt => opt.MapFrom(s => s.QuestionList));
            return Mapper.Map<Questions, QuestionsDTO>(questions);
        }
    }
}