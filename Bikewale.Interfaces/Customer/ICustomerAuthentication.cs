namespace Bikewale.Interfaces.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 25 Apr 2014
    /// Summary : Interface have all functions of the customers related to authentication and other operations.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    public interface ICustomerAuthentication<T,U>
    {
        bool IsRegisteredUser(string email);
        bool IsRegisteredUser(string email,string mobile);
        T AuthenticateUser(string email, string password, bool? createAuthTicket = null);
        T AuthenticateUser(string email);
        
        void UpdateCustomerMobileNumber(string mobile, string email, string name = null);
        void UpdatePasswordSaltHash(U customerId, string passwordSalt, string passwordHash);

        void SavePasswordRecoveryToken(U customerId, string token);
        bool IsValidPasswordRecoveryToken(U customerId, string token);
        void DeactivatePasswordRecoveryToken(U customerId);

        string GenerateAuthenticationToken(string custId, string custName, string custEmail);
    }
}
