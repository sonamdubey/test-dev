using System;

namespace Carwale.Entity.Accessories.Tyres
{
    [Serializable]
    public class VersionTyresList : CarEntity
    {
        public TyreList TyresList {get;set;}

        public int MakeYear { get; set; }
    }
}
