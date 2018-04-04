
using BikewaleOpr.Entity.Users;
namespace BikewaleOpr.Interface.Users
{
    /// <summary>
    /// Author : Kartik rathod on 30 march 18
    /// Desc    : interface for users 
    /// </summary>
    public interface IUsers
    {
        /// <summary>
        /// Author  : Kartik rathod on 30 march 18
        /// Desc    : Google authentication with passed id_token provides authorised emailid
        /// </summary>
        /// <param name="id_token">provided token through google signin api</param>
        /// <returns>valid email id</returns>
        string GoogleApiAuthentication(string id_token);

        /// <summary>
        /// Author  : Kartik rathod on 30 march 18
        /// Desc    : get opr users details though api/userdetails/get api
        /// </summary>
        /// <param name="loginId">opr loginid</param>
        /// <returns>UserDetailsEntity</returns>
        UserDetailsEntity GetUserDetails(string loginId);
    }
}
