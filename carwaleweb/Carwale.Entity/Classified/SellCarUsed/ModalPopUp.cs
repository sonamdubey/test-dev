using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class ModalPopUp
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public bool? IsYesButtonActive { get; set; }
        public string YesButtonText { get; set; }
        public string YesButtonLink { get; set; }
        public bool? IsNoButtonActive { get; set; }
        public string NoButtonText { get; set; }
        public string NoButtonLink { get; set; }
    }
}
