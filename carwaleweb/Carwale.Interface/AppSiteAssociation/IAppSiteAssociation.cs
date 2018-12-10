using Carwale.Entity.IPToLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.AppSiteAssociation;
namespace Carwale.Interfaces.IAppSiteAssociation
{
    public interface IAppSiteAssociation
    {
        AppSiteAssociationEntity Get();
    }
}
