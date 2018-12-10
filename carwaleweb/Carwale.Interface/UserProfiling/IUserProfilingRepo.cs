using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.CarData;
using Carwale.Entity.UserProfiling;

namespace Carwale.Interfaces.UserProfiling
{
    public interface IUserProfilingRepo
    {
        bool UserProfilingStatus(int platformId);
    }
}
