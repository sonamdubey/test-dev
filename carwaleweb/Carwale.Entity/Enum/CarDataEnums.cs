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

    public enum MobilePlatformScreenType
    {
        homepage = 1,
        newcarslist = 5,
        modeldetails = 6,
        versiondetails = 7,
        newslist = 8,
        comparedetails = 9,
        upcominglist = 10
    }

    public enum PriceBucket
    { 
        NoUserCity=0,
        HaveUserCity=1,
        PriceNotAvailable=2,
        CarNotSold=3
    }
    public enum ComponentType
    {
        OverView = 1,
        Specs = 2,
        Features = 3
    }

	public enum Status
	{
		New = 1,
		Discontinued = 2,
		Futuristic = 3,
		All = 4
	}
}
