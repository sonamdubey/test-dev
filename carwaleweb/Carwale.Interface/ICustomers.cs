using Carwale.Entity;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
    public interface ICustomerRepository<TEntity, TOut> //: IRepository<TEntity, TOut>
    {
        TOut Create(TEntity entity);
        bool Update(TEntity entity);
        TEntity GetCustomerFromEmail(string emailId,string oauth="");
        TEntity GetCustomerFromCustomerId(string customerId);
        bool ResetPassword(string CustomerId, string PasswordHashSalt, string newOauth);
        string SavePasswordChangeAT(string Email, string AccessToken);
        string GetCustomerIdByAccessToken(string AccessToken, out int MinutesDiff);
        bool CreateRememberMeSession(CustomerRememberMe custrm);
        string UseActiveRememberMeSession(CustomerRememberMe custrm, string NewAccessToken);
        bool EndRememberMeSession(string customerId, string identifier);
        bool UnsubscribeNewsletter(string email);
        void UpdateSourceId(EnumTableType source, string id);
        TEntity GetCustomerFromAuthKey(string oauth);
        TEntity GetCustomerFromSecurityKey(string key);
        bool UpdateEmailVerfication(bool isEmailVerified, int userId);
    }

    public interface ICustomerBL<TEntity, TOut>
    {
        TOut CreateCustomer(TEntity entity);
        TEntity GetCustomer(string email);
        TEntity GetCustomerById(string CustomerId);
        TEntity GetCustomer(string email, string password,string oauth="");
        bool ResetPassword(string customerId, string oldPassword, string newPassword);
        bool ResetPassword(string AccessToken, string newPassword);
        bool UpdateCustomerDetails(TEntity entity);
        bool GenPasswordChangeAT(string email);
        TEntity GetCustomerByAccessToken(string AccessToken);
        void CreateRememberMeSession(string customerId);
        bool UseActiveRememberMeSession();
        bool EndRememberMeSession(string customerId, string identifier);
        void UpdateSourceId(EnumTableType source, string id);
    }
}
