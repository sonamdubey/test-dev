using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.Users;
using System.Collections.Generic;

namespace BikewaleOpr.Models.QuestionsAndAnswers
{
    public class QuestionDetailsBaseVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public IEnumerable<User> InternalUsers { get; set; }
    }
}
