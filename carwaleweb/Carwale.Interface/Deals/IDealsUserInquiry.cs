using Carwale.DTOs;
using Carwale.Entity;
using Carwale.Entity.Deals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Deals
{
    public interface IDealsUserInquiry<TEntity>
    {
        List<TEntity> GetDealsDroppedUsers();
    }
}
