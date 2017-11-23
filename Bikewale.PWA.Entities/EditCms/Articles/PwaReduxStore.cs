using System.Collections.Generic;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
    /// </summary>
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