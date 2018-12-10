using System;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarMakesEntity : CarMakeEntityBase
    {        
        public bool Used { get; set; }
        public bool New { get; set; }
        public bool Futuristic { get; set; }
        public bool Indian { get; set; }
        public bool Imported { get; set; }
        public bool Classic { get; set; }
        public string LogoUrl { get; set; }        
    }
}
