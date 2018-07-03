using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BikewaleOpr.Models.QuestionsAndAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: Base VM for Qna OPR
    /// </summary>


    public class QnABaseVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public QnAFilters Filters { get; set; }
        public EnumQnAPageType PageType { get; set; }
    }
}
