using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KuaforEvde.Core.Enum;
using KuaforEvde.Core.Model;
using KuaforEvde.Data.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KuaforEvde.Service.Query.HairDresser
{
    public class HairDresserSaveContactPersonQuery : IRequest<ResponseModel<bool>>
    {
        public int? Id { get; set; }
        public int HairDresserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Status Status { get; set; }
    }

    public class HairDresserSaveContactPersonQueryHandler : IRequestHandler<HairDresserSaveContactPersonQuery, ResponseModel<bool>>
    {

        private readonly KuaforEvdeContext _db;
        public HairDresserSaveContactPersonQueryHandler(KuaforEvdeContext db)
        {
            _db = db;
        }

        public async Task<ResponseModel<bool>> Handle(HairDresserSaveContactPersonQuery request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {

                return await Update(request);
            }

            return await Insert(request);
        }

        private async Task<ResponseModel<bool>> Update(HairDresserSaveContactPersonQuery request)
        {
            var person = await _db.ContactPerson.FirstOrDefaultAsync(q => q.Id == request.Id);
            if (person != null)
            {
                person.Name = request.Name;
                person.LastName = request.LastName;
                person.Email = request.Email;
                person.Phone = request.Phone;
                person.Status = (byte)request.Status;
                await _db.SaveChangesAsync();
            }
            return new ResponseModel<bool>();
        }

        private async Task<ResponseModel<bool>> Insert(HairDresserSaveContactPersonQuery request)
        {
            await _db.ContactPerson.AddAsync(new ContactPerson
            {
                Status = (byte)Status.Active,
                CreatedDate = DateTime.Now,
                Name = request.Name,
                Email = request.Email,
                LastName = request.LastName,
                HairDresserId = request.HairDresserId,
                Phone = request.Phone
            });
            await _db.SaveChangesAsync();
            return new ResponseModel<bool>();

        }
    }
}

