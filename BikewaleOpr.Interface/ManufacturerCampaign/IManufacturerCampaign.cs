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
    /// <summary>
    /// Created by Subodh Jain 29 aug 2016
    /// Description : Interface for manufactureCampaign
    /// </summary>
    public interface IManufacturerCampaignRepository
    {
        IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerid);
        bool UpdateCampaignStatus(uint id, bool isactive);
        IEnumerable<ManufacturerEntity> GetManufacturersList();
    }
}
