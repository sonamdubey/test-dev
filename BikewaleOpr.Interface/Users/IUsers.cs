
using BikewaleOpr.Entity.Users;
namespace BikewaleOpr.Interface.Users
{
    /// <summary>
    /// Author : Kartik rathod on 30 march 18
    /// Desc    : interface for users 
    /// </summary>
    public interface IUsers
    {
        string GoogleApiAuthentication(string id_token);
        UserDetailsEntity GetUserDetails(string loginId);
    }
}
