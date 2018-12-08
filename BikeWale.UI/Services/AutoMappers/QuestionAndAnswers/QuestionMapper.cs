using AutoMapper;
using Bikewale.DTO.Customer;
using Bikewale.DTO.QuestionAndAnswers;
using Bikewale.Entities.Customer;
using Bikewale.Entities.QuestionAndAnswers;
using BikewaleElasticDTO = Bikewale.DTO.QuestionAndAnswers.ElasticSearch;
using BikeWaleElasticEntities = Bikewale.Entities.QuestionAndAnswers.ElasticSearch;


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
            return Mapper.Map<Questions, QuestionsDTO>(questions);
        }


        /// <summary>
        /// Created by  : Kumar Swapnil on 14 September 2018
        /// Description : Function to map `TIn` entity to `TOutDTO`.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TOut Convert<TIn, TOut>(TIn input)
        {
            return Mapper.Map<TIn, TOut>(input);
        }
    }
}