using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.CMS
{
    public class CMSBikeTagsEntity
    {
        private BikeModelEntityBase _objModel = new BikeModelEntityBase();
        public BikeModelEntityBase ModelBase { get { return _objModel; } set { _objModel = value; } }

        private BikeMakeEntityBase _objMake = new BikeMakeEntityBase();
        public BikeMakeEntityBase MakeBase { get { return _objMake; } set { _objMake = value; } }

        private BikeVersionEntityBase _objVersion = new BikeVersionEntityBase();
        public BikeVersionEntityBase VersionBase { get { return _objVersion; } set { _objVersion = value; } }
    }
}
