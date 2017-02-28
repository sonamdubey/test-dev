using Bikewale.Entities.Videos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
