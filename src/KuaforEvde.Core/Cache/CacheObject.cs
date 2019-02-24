using System;

namespace KuaforEvde.Core.Cache
{
    public class CacheObject
    {
        public CacheObject(object value)
        {
            Value = value;
        }
        public CacheObject(object value, DateTime expiresAt)
        {
            Value = value;
            ExpiresAt = expiresAt;
        }
        public DateTime ExpiresAt { get; set; }
        public object Value { get; set; }

        public T GetValue<T>()
        {
            if (Value == null)
            {
                return default(T);
            }

            return (T)Value;
        }
    }
}
