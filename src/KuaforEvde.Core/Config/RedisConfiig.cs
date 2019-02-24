using System;
using System.Collections.Generic;
using System.Text;

namespace KuaforEvde.Core.Config
{
    public class RedisConfig
    {
        public List<RedisEndpoint> Endpoints { get; set; }
        public string Password { get; set; }

        public bool IsEnable { get; set; }
    }

    public class RedisEndpoint
    {
        public string Host { get; set; }
        public int Port { get; set; }

    }
}
