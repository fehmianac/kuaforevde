using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using KuaforEvde.Core.Config;
using KuaforEvde.Core.Model;

namespace KuaforEvde.Core.Cache
{
    public class RedisCacher : IRedisCacher
    {
        private static RedisConfig _configuration;
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private ConnectionMultiplexer Connection => _lazyConnection.Value;

        public RedisCacher(RedisConfig configuration)
        {
            _configuration = configuration;

            if (configuration.IsEnable)
            {
                _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(GetConfigurationOptions()));
            }
        }

        private static ConfigurationOptions GetConfigurationOptions()
        {
            var options = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                AllowAdmin = false,
                ConnectTimeout = 5000,
                KeepAlive = 30,
                SyncTimeout = 5000
            };

            if (!string.IsNullOrWhiteSpace(_configuration.Password))
            {
                options.Password = _configuration.Password;
            }

            foreach (var endpoint in _configuration.Endpoints)
            {
                options.EndPoints.Add(endpoint.Host, endpoint.Port);
            }

            return options;
        }

        public TimeSpan? GetKeyTimeToLive(string key)
        {
            try
            {
                if (!_configuration.IsEnable)
                    return null;
                if (Connection.IsConnected)
                {
                    var db = Connection.GetDatabase();
                    return db.KeyTimeToLive(key);
                }
                return null;
            }
            catch (System.Exception exception)
            {
                return null;
            }
        }

        public CacheResult<T> Get<T>(string key)
        {
            if (!_configuration.IsEnable)
                return new CacheResult<T>
                {
                    IsExistInCacheServer = false
                };

            if (Connection.IsConnected)
            {
                var db = Connection.GetDatabase();
                string value = db.StringGet(key);
                return new CacheResult<T>
                {
                    IsExistInCacheServer = db.KeyExists(key),
                    Result = string.IsNullOrEmpty(value) ? default(T) : JsonConvert.DeserializeObject<T>(value)
                };
            }
            return new CacheResult<T>
            {
                IsExistInCacheServer = false
            };
        }

        public void Set<T>(string key, T value, TimeSpan expiresIn)
        {
            if (!_configuration.IsEnable)
                return;

            if (Connection.IsConnected)
            {
                var db = Connection.GetDatabase();
                if (value != null)
                {
                    db.StringSet(key, JsonConvert.SerializeObject(value), expiresIn);
                }
            }
        }

        public async Task Remove(string key)
        {
            if (!_configuration.IsEnable)
                return;
            if (Connection.IsConnected)
            {
                var db = Connection.GetDatabase();
                await db.KeyDeleteAsync(key);
            }
        }

        public async Task Remove(List<string> keys)
        {
            if (keys == null || !keys.Any())
            {
                return;
            }

            var redisKeys = new RedisKey[keys.Count];
            for (int i = 0; i < keys.Count; i++)
            {
                redisKeys[i] = keys[i];
            }
            if (Connection.IsConnected)
            {
                var db = Connection.GetDatabase();
                await db.KeyDeleteAsync(redisKeys);
            }
        }

        public async Task ClearByKeys(List<string> keys)
        {
            await Remove(keys);
        }
    }

    public interface IRedisCacher
    {
        CacheResult<T> Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiresIn);
        Task Remove(string key);
        Task Remove(List<string> keys);
        Task ClearByKeys(List<string> keys);
    }
}
