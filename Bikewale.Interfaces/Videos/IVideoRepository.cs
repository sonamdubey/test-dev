using Bikewale.Entities.Videos;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Feb 2017
    /// Summary    : Interface for videos
    /// </summary>
    public interface IVideoRepository
    {
        ICollection<BikeVideoModelEntity> GetModelVideos(uint makeId);
    }
}
