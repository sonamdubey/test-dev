using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    public class CarMake
    {
        public int MakeId { get; set; }
        public int MakeName { get; set; }
        public bool IsDeleted { get; set; }
        public int LogoUrl { get; set; }
        public bool IsUsed { get; set; }
        public bool IsNew { get; set; }
        public bool IsIndian { get; set; }
        public bool IsImported { get; set; }
        public bool IsFuturistic { get; set; }
        public bool IsClassic { get; set; }
        public bool IsModified { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdateOn { get; set; }
        public int LastUpdateBy { get; set; }
        public string HostURL { get; set; }
        public int Popularity { get; set; }
        public string OriginalImgPath { get; set; }      
    }
}
