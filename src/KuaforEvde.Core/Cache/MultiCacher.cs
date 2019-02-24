using System;
using System.Collections.Generic;
using KuaforEvde.Core.Config;

namespace KuaforEvde.Core.Cache
{
    public class MultiCacher : IMultiCacher
    {
        private readonly IMemoryCacher _memoryCacher;
        private readonly IRedisCacher _redisCacher;
        private readonly RedisConfig _redisConfig;

        public MultiCacher(IMemoryCacher memoryCacher, IRedisCacher redisCacher, RedisConfig redisConfig)
        {
            _memoryCacher = memoryCacher;
            _redisCacher = redisCacher;
            _redisConfig = redisConfig;
        }

        public CacheObject GetOrCreate<T>(string key, TimeSpan expiresIn, Func<T> createFn)
        {
            var cacheObject = Get<T>(key);
            if (cacheObject.Value == null)
            {
                var result = createFn();
                Set(key, result, expiresIn);
                return new CacheObject(result);
            }
            return cacheObject;
        }

        public CacheObject GetOrCreate<T>(string key, Func<T> createFn)
        {
            return GetOrCreate(key, TimeSpan.FromMinutes(10), createFn);
        }

        public CacheObject Get<T>(string key)
        {
            var cacheObject = _memoryCacher.Get(key);
            if (cacheObject != null)
            {
                return cacheObject;
            }

            if (_redisConfig.IsEnable)
            {
                var redisResult = _redisCacher.Get<T>(key);
                if (redisResult.IsExistInCacheServer)
                {
                    if (redisResult.Result == null)
                        return new CacheObject(redisResult.Result);

                    //Eğer redis'te varsa ama memory'de yoksa burada memory'e ekliyoruz.
                    var expiration = TimeSpan.FromMinutes(5);
                    cacheObject = new CacheObject(redisResult.Result, DateTime.UtcNow.AddMinutes(5));
                    _memoryCacher.Set(key, cacheObject.Value, expiration);
                    return cacheObject;
                }
            }
            return new CacheObject(null);
        }

        public void Set<T>(string key, T value, TimeSpan expiresIn)
        {
            _memoryCacher.Set(key, value, expiresIn);
            if (_redisConfig.IsEnable)
            {
                _redisCacher.Set(key, value, expiresIn);
            }
        }

        public void ClearByKeys(List<string> keys)
        {
            _memoryCacher.ClearByKeys(keys);
            if (_redisConfig.IsEnable)
            {
                _redisCacher.ClearByKeys(keys).GetAwaiter().GetResult();
            }
        }
    }
    public interface IMultiCacher
    {
        CacheObject GetOrCreate<T>(string key, TimeSpan expiresIn, Func<T> createFn);
        CacheObject GetOrCreate<T>(string key, Func<T> createFn);
        CacheObject Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiresIn);
        void ClearByKeys(List<string> keys);
    }
}
