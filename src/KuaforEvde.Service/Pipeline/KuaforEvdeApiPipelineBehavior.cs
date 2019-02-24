using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KuaforEvde.Core.Cache;
using KuaforEvde.Core.Model;
using MediatR;

namespace KuaforEvde.Service.Pipeline
{
    public class KuaforEvdeApiPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMultiCacher _multiCacher;

        public KuaforEvdeApiPipelineBehavior(IMultiCacher multiCacher)
        {
            _multiCacher = multiCacher;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                if (request is ICacheCleaner)
                {
                    var clearCacheRequest = request as ICacheCleaner;
                    var cacheKeys = clearCacheRequest.CacheKeys.Select(q => q).ToList();

                    _multiCacher.ClearByKeys(cacheKeys);
                }

                var cacheable = request as ICacheable;
                if (cacheable == null)
                    return await next();

                var cacheKey = string.Format(cacheable.CacheKey);
                if (request is BasePagingRequest)
                {
                    var paged = request as BasePagingRequest;

                    cacheKey += string.Format("-{0}-{1}", paged.StartPage, paged.Limit);
                }
                var cacheResult = _multiCacher.Get<TResponse>(cacheKey);
                if (cacheResult.Value != null)
                {
                    return cacheResult.GetValue<TResponse>();
                }

                var response = await next();
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _multiCacher.Set(cacheKey, response, cacheable.CacheDuration);
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
