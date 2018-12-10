using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
    public interface ISecurityRepository<T>
    {
        //string[] GetRsaKeyPair(string publicKey);
        string GetPassword(string username);
        bool IsValidSource(string SourceId, string CWK);    //Added by Supriya K on 11/6/2014 to validate sourceid against CWK key
    }
}
