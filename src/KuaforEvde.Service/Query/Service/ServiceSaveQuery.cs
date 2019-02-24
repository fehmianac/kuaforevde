using System;
using System.Threading;
using System.Threading.Tasks;
using KuaforEvde.Core.Enum;
using KuaforEvde.Core.Model;
using KuaforEvde.Data.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KuaforEvde.Service.Query.Service
{
    public class ServiceSaveQuery : IRequest<ResponseModel<bool>>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool ForMale { get; set; }
        public bool ForFemale { get; set; }
        public Status Status { get; set; }
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
            if (request.Id.HasValue)
            {
                return await Update(request);
            }
            return await Insert(request);
        }

        private async Task<ResponseModel<bool>> Update(ServiceSaveQuery request)
        {
            var service = await _db.Service.FirstOrDefaultAsync(q => q.Id == request.Id);
            if (service != null)
            {
                service.Name = request.Name;
                service.ForFemale = request.ForFemale;
                service.ForMale = request.ForMale;
                service.Status = (byte)request.Status;
                await _db.SaveChangesAsync();
            }

            return new ResponseModel<bool>();
        }

        private async Task<ResponseModel<bool>> Insert(ServiceSaveQuery request)
        {
            await _db.Service.AddAsync(new Data.DB.Service
            {
                CreatedDate = DateTime.Now,
                ForFemale = request.ForFemale,
                ForMale = request.ForMale,
                Name = request.Name,
                Status = (byte)Status.Active
            });
            await _db.SaveChangesAsync();
            return new ResponseModel<bool>();
        }
    }
}
