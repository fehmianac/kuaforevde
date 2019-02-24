using System;
using System.Threading;
using System.Threading.Tasks;
using KuaforEvde.Core.Model;
using KuaforEvde.Data.DB;
using MediatR;

namespace KuaforEvde.Service.Query.Service
{
    public class ServiceSaveQuery : IRequest<ResponseModel<bool>>
    {

    }

    public class ServiceSaveQueryHandler : IRequestHandler<ServiceSaveQuery, ResponseModel<bool>>
    {
        private readonly KuaforEvdeContext _db;
        public ServiceSaveQueryHandler(KuaforEvdeContext db)
        {
            _db = db;
        }

        public async Task<ResponseModel<bool>> Handle(ServiceSaveQuery request, CancellationToken cancellationToken)
        {
            await _db.Service.AddAsync(new Data.DB.Service
            {
                CreatedDate = DateTime.Now,
                ForFemale = true,
                ForMale = false,
                Name = "Manikür",
                Status = 1
            });
            await _db.SaveChangesAsync(cancellationToken);
            return new ResponseModel<bool>();
        }
    }
}
