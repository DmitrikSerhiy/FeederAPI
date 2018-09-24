using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.CacheManagers
{
    public interface ICacheManager
    {
        int DurationInSeconds { get; set; }
    }
}
