using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KuaforEvde.Core.Cache
{
    public class MemoryCacher : IMemoryCacher
    {
        private readonly ConcurrentDictionary<string, CacheObject> _memory;

        public MemoryCacher()
        {
            _memory = new ConcurrentDictionary<string, CacheObject>();
        }

        public CacheObject Get(string key)
        {
            CacheObject cacheObject;
            if (_memory.TryGetValue(key, out cacheObject))
            {
                if (cacheObject.ExpiresAt < DateTime.UtcNow)
                {
                    _memory.TryRemove(key, out cacheObject);
                    return null;
                }
                return cacheObject;
            }
            return null;
        }

        public void Set<T>(string key, T value, TimeSpan expiresIn)
        {
            var expiresAt = DateTime.UtcNow.Add(expiresIn);

            CacheObject cacheObject;
            if (!_memory.TryGetValue(key, out cacheObject))
            {
                cacheObject = new CacheObject(value, expiresAt);
                _memory[key] = cacheObject;
                return;
            }
            cacheObject.Value = value;
            cacheObject.ExpiresAt = expiresAt;
        }

        public void Remove(string key)
        {
            CacheObject cacheObject;
            _memory.TryRemove(key, out cacheObject);
        }

        public void ClearByKeys(List<string> keys)
        {
            var cacheKeys = _memory.Keys;
            foreach (var key in cacheKeys)
            {
                foreach (var pattern in keys)
                {
                    var regex = new Regex(pattern);
                    if (regex.IsMatch(key))
                        Remove(key);
                }
            }
        }
    }
    public interface IMemoryCacher
    {
        CacheObject Get(string key);
        void Set<T>(string key, T value, TimeSpan expiresIn);
        void Remove(string key);
        void ClearByKeys(List<string> keys);
    }
}
