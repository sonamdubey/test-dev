﻿using Bikewale.PWA.Entities.Images;
using System.Collections.Generic;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
    /// Modified by : Ashutosh Sharma on 14 Feb 2018
    /// Description : Added reducer for Widgets.
    /// </summary>
    public class PwaReduxStore
    {
        public PwaNewsReducer News { get; private set; }
        public PwaVideosReducer Videos { get; private set; }
        public PwaWidgetsReducer Widgets { get; private set; }

        public PwaReduxStore()
        {
            News = new PwaNewsReducer();
            Videos = new PwaVideosReducer();
            Widgets = new PwaWidgetsReducer();
        }

    }
}