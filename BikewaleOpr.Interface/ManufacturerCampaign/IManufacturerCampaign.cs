using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ManufacturerCampaign
{
    public interface IManufacturerCampaign
    {
        IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerid);
        bool statuschangeCampaigns(uint id, uint isactive);

    }
}
