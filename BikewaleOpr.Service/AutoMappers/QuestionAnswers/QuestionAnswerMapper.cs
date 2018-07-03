using AutoMapper;
using BikewaleOpr.DTO.Customer;
using BikewaleOpr.DTO.QuestionsAnswers;
using BikewaleOpr.Entity.QuestionsAndAnswers;
using QuestionsAnswers.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers
{
    /// <summary>
    /// Created by : SnehaL Dange on 11th June 2018
    /// Description : Automapper for QuestionAnswer service
    /// Modified by : Sanskar Gupta on 14 June 2018
    /// Description : Add an AutoMapper to map `DTO.QuestionsAnswers.UpdateQuestionTagsDTO` to `Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsEntity` in `BikewaleOpr.Service.AutoMappers.QuestionAnswerMapper`
    /// Modified by : Sanskar Gupta on 20 June 2018
    /// Description : Added an AutoMapper to map `AnswersEntity` to `AnswersDTO`
    /// </summary>
    public class QuestionAnswerMapper
    {

        public static QuestionsFilter Convert(Entity.QnA.QnAFilters qnAFilters)
        {
            Mapper.CreateMap<BikewaleOpr.Entity.QnA.QnAFilters, QuestionsFilter>();
            return Mapper.Map<BikewaleOpr.Entity.QnA.QnAFilters, QuestionsFilter>(qnAFilters);
        }

        public static IEnumerable<Entity.QnA.Tag> Convert(IEnumerable<QuestionsAnswers.Entities.Tag> tagList)
        {
            Mapper.CreateMap<QuestionsAnswers.Entities.Tag, Entity.QnA.Tag>();
            return Mapper.Map<IEnumerable<QuestionsAnswers.Entities.Tag>, IEnumerable<Entity.QnA.Tag>>(tagList);
        }

        internal static BikewaleOpr.Entity.QnA.Question.ModerateQuestionEntity Convert(BikewaleOpr.DTO.QuestionsAnswers.ModerateQuestionDTO moderateQuestion)
        {
            Mapper.CreateMap<BikewaleOpr.DTO.QuestionsAnswers.ModerateQuestionDTO, BikewaleOpr.Entity.QnA.Question.ModerateQuestionEntity>();
            return Mapper.Map<BikewaleOpr.DTO.QuestionsAnswers.ModerateQuestionDTO, BikewaleOpr.Entity.QnA.Question.ModerateQuestionEntity>(moderateQuestion);
        }

        public static Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsEntity Convert(DTO.QuestionsAnswers.UpdateQuestionTagsDTO updateQuestionTags)
        {
            Mapper.CreateMap<DTO.QuestionsAnswers.UpdateQuestionTagsDTO, Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsEntity>();
            return Mapper.Map<DTO.QuestionsAnswers.UpdateQuestionTagsDTO, Entity.QuestionsAndAnswers.Question.UpdateQuestionTagsEntity>(updateQuestionTags);
        }

        public static Answer Convert(DTO.QuestionsAnswers.AnswerDTO answerDTO)
        {
            Mapper.CreateMap<CustomerBaseDTO, Customer>();
            Mapper.CreateMap<AnswerBaseDTO, AnswerBase>();
            Mapper.CreateMap<AnswerDTO, Answer>();
            return Mapper.Map<AnswerDTO, Answer>(answerDTO);
        }

        public static AnswersDTO Convert(AnswersEntity answersEntity)
        {
            Mapper.CreateMap<Customer, CustomerBaseDTO>();
            Mapper.CreateMap<AnswerBase, AnswerBaseDTO>();
            Mapper.CreateMap<Answer, AnswerDTO>();
            Mapper.CreateMap<AnswersEntity, AnswersDTO>();
            return Mapper.Map<AnswersEntity, AnswersDTO>(answersEntity);
        }
    }
}