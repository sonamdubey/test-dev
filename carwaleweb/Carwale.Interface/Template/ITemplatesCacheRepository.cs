using Carwale.Entity.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Template
{
    public interface ITemplatesCacheRepository
    {
        List<Templates> GetAll(short platformId, short typeId);
        Templates GetById(int templateId);
    }
}
