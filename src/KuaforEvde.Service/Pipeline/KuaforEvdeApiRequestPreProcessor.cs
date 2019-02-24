using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Pipeline;

namespace KuaforEvde.Service.Pipeline
{
    public class KuaforEvdeRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IValidator<TRequest>[] _validators;

        public KuaforEvdeRequestPreProcessor(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return Task.CompletedTask;
            }

            var context = new ValidationContext(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
            return Task.CompletedTask;
        }
    }
}