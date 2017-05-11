
namespace Bikewale.Interfaces.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for customer DAL.
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface ICustomerRepository<T, U> : IRepository<T, U>
    {
        T GetByEmail(string emailId);

        void UpdateCustomerMobileNumber(string mobile, string email, string name = null);
        void UpdatePasswordSaltHash(U customerId, string passwordSalt, string passwordHash);

        void SavePasswordRecoveryToken(U customerId, string token);
        bool IsValidPasswordRecoveryToken(U customerId, string token);
        void DeactivatePasswordRecoveryToken(U customerId);
        bool IsFakeCustomer(ulong customerId);
    }
}