using System;

namespace KuaforEvde.Core.Cache
{
    public interface ICacheable
    {
        string CacheKey { get; }

        TimeSpan CacheDuration { get; }
    }
}
