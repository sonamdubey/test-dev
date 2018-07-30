
namespace Bikewale.Interfaces.Security
{
    /// <summary>
    /// Created by  :   Sumit Kate on 09 nov 2016
    /// Description :   This interface is to expose security related methods
    /// </summary>
    public interface ISecurity
    {
        Bikewale.Entities.AWS.Token GetToken();
        string GenerateHash(uint id);
        bool VerifyHash(string hash, uint id);
    }
}
