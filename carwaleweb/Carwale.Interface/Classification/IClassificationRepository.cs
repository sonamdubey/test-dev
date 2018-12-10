using Carwale.Entity.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classification
{
    public interface IClassificationRepository
    {
        List<BodyType> GetBodyType();
    }
}
