using AutoMapper;
using Bikewale.Entities.Customer;
using Bikewale.Entities.QuestionAndAnswers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.BAL.QuestionAndAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th August 2018
    /// Description: Mappers for BW and Qna Service Entites
    /// </summary>
    internal static class Mappers
    {   
        static Mappers()
        {
            Mapper.CreateMap<QuestionsAnswers.Entities.Customer, CustomerEntityBase>()
                  .ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.Id))
                  .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Name))
                  .ForMember(d => d.CustomerEmail, opt => opt.MapFrom(s => s.Email));
            Mapper.CreateMap<CustomerEntityBase, QuestionsAnswers.Entities.Customer>()
                  .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CustomerId))
                  .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CustomerName))
                  .ForMember(d => d.Email, opt => opt.MapFrom(s => s.CustomerEmail));


            Mapper.CreateMap<QuestionsAnswers.Entities.AnswerBase, Answer>();
            Mapper.CreateMap<Answer, QuestionsAnswers.Entities.AnswerBase>();
            
            Mapper.CreateMap<QuestionsAnswers.Entities.Tag, Bikewale.Entities.QuestionAndAnswers.Tag>();
            Mapper.CreateMap<Bikewale.Entities.QuestionAndAnswers.Tag, QuestionsAnswers.Entities.Tag>();
            
            Mapper.CreateMap<QuestionsAnswers.Entities.Question, Question>();
            Mapper.CreateMap<Question, QuestionsAnswers.Entities.Question>();
        }

        internal static TOut Convert<TIn, TOut>(TIn input)
        {            
            return Mapper.Map<TIn, TOut>(input);
        }
    }
}
