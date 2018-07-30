using Bikewale.Interfaces.UsersTestimonial;
using System.Collections.Generic;

namespace Bikewale.DAL.UsersTestimonial
{
    /// <summary>
    /// Created By  :   Sumit Kate on 19 Jan 2016
    /// Description :   User's Testimonial Repository
    /// </summary>
    public class UsersTestimonialRepository : IUsersTestimonial
    {
        /// <summary>
        /// Fetches the Users Testimonial by calling GetTestimonial SP
        /// </summary>
        /// <param name="topCount">Top count default value 6</param>
        /// <returns></returns>
        public IEnumerable<Entities.UsersTestimonial.UsersTestimonial> FetchUsersTestimonial(uint topCount = 6)
        {
            IList<Entities.UsersTestimonial.UsersTestimonial> lstUsersTestimonials = null;
            //Database db = null;

            //try
            //{
            //    db = new Database();

            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandText = "GetTestimonial_16022016";
            //        cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = topCount;

            //        using (SqlDataReader dr = db.SelectQry(cmd))
            //        {
            //            if (dr != null)
            //            {
            //                lstUsersTestimonials = new List<Entities.UsersTestimonial.UsersTestimonial>();
            //                while (dr.Read())
            //                {
            //                    lstUsersTestimonials.Add(
            //                        new Entities.UsersTestimonial.UsersTestimonial()
            //                        {
            //                            Title = Convert.ToString(dr["Title"]),
            //                            Content = Convert.ToString(dr["Content"]),
            //                            HostUrl = Convert.ToString(dr["HostUrl"]),
            //                            UserImgUrl = Convert.ToString(dr["userImgUrl"]),
            //                            EntryDate = Convert.ToDateTime(dr["EntryDate"]),
            //                            UserName = Convert.ToString(dr["UserName"]),
            //                            City = Convert.ToString(dr["City"])
            //                        });
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, "UsersTestimonialRepository.FetchUsersTestimonial");
            //    
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
            return lstUsersTestimonials;
        }
    }
}
