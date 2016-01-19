using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.UsersTestimonial;

namespace Bikewale.Interfaces.UsersTestimonial
{
    /// <summary>
    /// Interface for User's Testimonial
    /// </summary>
    public interface IUsersTestimonial
    {
        public IEnumerable<Entities.UsersTestimonial.UsersTestimonial> FetchUsersTestimonial(uint topCount = 6);
    }
}
