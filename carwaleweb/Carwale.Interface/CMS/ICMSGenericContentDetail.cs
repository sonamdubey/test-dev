using Carwale.DTOs.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CMS
{
    public interface ICMSGenericContentDetail
    {
        public GenericContentDetailDTO GetContentDetail(string CategoryName, int id); 
    }
}
