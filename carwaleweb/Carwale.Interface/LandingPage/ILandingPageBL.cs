using Carwale.DTOs.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.LandingPage
{
    public interface ILandingPageBL
    {
        LandingPageDTO Get(int campaignId, int? modelId);
    }
}
