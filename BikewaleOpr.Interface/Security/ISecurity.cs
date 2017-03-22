
namespace BikewaleOpr.Interface.Security
{
    /// <summary>
    /// Created by  :   Sumit Kate on 09 nov 2016
    /// Description :   This interface is to expose security related methods
    /// </summary>
    public interface ISecurity
    {
        BikewaleOpr.Entities.AWS.Token GetToken();
        string GenerateHash(uint id);
        bool VerifyHash(string hash, uint id);
    }
}
