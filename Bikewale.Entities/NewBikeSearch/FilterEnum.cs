using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.NewBikeSearch
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 31 Aug 2015
    /// </summary>
    public enum AntiBreakingSystem
    {
        ABSAvailable = 1,
        ABSNotAvailable = 2
    }

    public enum Brake
    {
        Drum = 1,
        Disc = 2
    }

    public enum StartType
    {
        Electric = 1,
        Manual = 2
    }

    public enum WheelType
    {
        Alloy = 1,
        Spoke = 2
    }
}
