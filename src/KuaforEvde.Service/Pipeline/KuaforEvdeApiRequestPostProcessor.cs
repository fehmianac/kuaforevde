using System.Threading.Tasks;
using MediatR.Pipeline;

namespace KuaforEvde.Service.Pipeline
{
    public class KuaforEvdeRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        public Task Process(TRequest request, TResponse response)
        {
            return Task.CompletedTask;
        }
    }
}

