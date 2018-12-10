using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.AppSiteAssociation;

/// <summary>
/// Author: Ajay Singh(on 16 feb 2016)
/// Description : For IOS purpose
/// </summary>

namespace Carwale.Interfaces.IAppSiteAssociation
{
   public interface IAppSiteAssociationBL 
    {
       AppSiteAssociationEntity Get();

    }
}
