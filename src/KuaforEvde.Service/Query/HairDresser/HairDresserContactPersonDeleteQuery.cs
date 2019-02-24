using System.Threading;
using System.Threading.Tasks;
using KuaforEvde.Core.Enum;
using KuaforEvde.Core.Model;
using KuaforEvde.Data.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KuaforEvde.Service.Query.HairDresser
{
    public class HairDresserContactPersonDeleteQuery : IRequest<ResponseModel<bool>>
    {
        public int Id { get; set; }
        public int HairDresserId { get; set; }

    }

    public class HairDresserContactPersonDeleteQueryHandler : IRequestHandler<HairDresserContactPersonDeleteQuery, ResponseModel<bool>>
    {
        private readonly KuaforEvdeContext _db;

        public HairDresserContactPersonDeleteQueryHandler(KuaforEvdeContext db)
        {
            _db = db;
        }

        public async Task<ResponseModel<bool>> Handle(HairDresserContactPersonDeleteQuery request, CancellationToken cancellationToken)
        {
            var person = await _db.ContactPerson.FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);
            if (person != null && person.HairDresserId == request.HairDresserId)
            {
                person.Status = (byte)Status.Deleted;
                await _db.SaveChangesAsync(cancellationToken);
            }
            return new ResponseModel<bool>();
        }
    }
}
