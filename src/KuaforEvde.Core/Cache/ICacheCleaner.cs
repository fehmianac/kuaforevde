using System.Collections.Generic;

namespace KuaforEvde.Core.Cache
{
    public interface ICacheCleaner
    {
        List<string> CacheKeys { get; }
    }
}
