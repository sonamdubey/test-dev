using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public enum PageSource
    {
        CarMake = 1,
        CarModel = 2,
        CarVersion = 3,
        CarModelPhotos = 4,
    }

    public enum PlatformSource
    {
        CarwaleDesktop = 1,
        CarwaleMobileWeb = 2,
        CarwaleMobileApp = 3,
    }

    public enum RedirectType
    {
        Internal = 1,
        External = 2,        
    }
}
