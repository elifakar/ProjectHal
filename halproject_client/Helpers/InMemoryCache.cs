using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace halproject_client.Helpers
{
    public class InMemoryCache
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(30));
            }
            return item;
        }

        public void Remove(string cacheKey)
        {
            var item = MemoryCache.Default.Get(cacheKey);
            if (item != null)
                MemoryCache.Default.Remove(cacheKey);
        }
    }
}