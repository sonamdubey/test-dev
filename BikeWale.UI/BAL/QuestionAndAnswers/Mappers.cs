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
     
        internal static TOut Convert<TIn, TOut>(TIn input)
        {            
            return Mapper.Map<TIn, TOut>(input);
        }
    }
}
