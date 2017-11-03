using System.Collections.Generic;

namespace Bikewale.Entities.PWA.Articles
{
    public class PwaReduxStore
    {
        public PwaNewsReducer News { get; private set; }
        public PwaVideosReducer Videos { get; private set; }

        public PwaReduxStore()
        {
            News = new PwaNewsReducer();
            Videos = new PwaVideosReducer();
        }

    }
}