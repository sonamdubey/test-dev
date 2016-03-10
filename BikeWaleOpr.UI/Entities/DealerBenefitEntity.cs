using BikeWaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    public class DealerBenefitEntity
    {
        uint Id { get; set; }
        uint BenefitCatId { get; set; }
        string BenefitText { get; set; }
        bool IsActive { get; set; }
        CityEntityBase ObjCity { get; set; }
        MakeEntityBase ObjMake { get; set; }
        ModelEntityBase ObjModel { get; set; }
    }
}