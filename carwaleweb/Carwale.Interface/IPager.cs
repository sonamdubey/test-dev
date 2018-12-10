using Carwale.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Carwale.Interfaces
{
    public interface IPager
    {
        T GetPager<T>(PagerEntity pagerDetails) where T : PagerOutputEntity , new();
    }
}
