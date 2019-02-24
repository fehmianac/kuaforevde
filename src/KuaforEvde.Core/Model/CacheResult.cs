using System;
using System.Collections.Generic;
using System.Text;

namespace KuaforEvde.Core.Model
{
    public class CacheResult<T>
    {
        public bool IsExistInCacheServer { get; set; }
        public T Result { get; set; }
    }
}
