using System.Collections.Generic;

namespace Bikewale.Interfaces.UsersTestimonial
{
    /// <summary>
    /// Interface for User's Testimonial
    /// </summary>
    public interface IUsersTestimonial
    {
        IEnumerable<Entities.UsersTestimonial.UsersTestimonial> FetchUsersTestimonial(uint topCount = 6);
    }
}
