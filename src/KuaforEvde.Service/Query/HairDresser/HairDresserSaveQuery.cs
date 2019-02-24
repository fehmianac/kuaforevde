using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KuaforEvde.Core.Enum;
using KuaforEvde.Core.Model;
using KuaforEvde.Data.DB;
using MediatR;

namespace KuaforEvde.Service.Query.HairDresser
{
    public class HairDresserSaveQuery : IRequest<ResponseModel<bool>>
    {
        public int AccountId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CityId { get; set; }
        public int DisrtictId { get; set; }
        public string AddressText { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public List<int> HairDresserArea { get; set; }

        public List<HairDresserPerson> HairDresserPersonList { get; set; }
        public List<HairDresserService> HairDresserServiceList { get; set; }
        public List<HairDresserImage> HairDresserImageList { get; set; }

        public class HairDresserPerson
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }

        public class HairDresserService
        {
            public int ServiceId { get; set; }
            public decimal Price { get; set; }
            public byte Gender { get; set; }
        }

        public class HairDresserImage
        {
            public string ImageUrl { get; set; }
            public int DisplayOrder { get; set; }
            public string AltText { get; set; }
            public byte IsMain { get; set; }
        }
    }



    public class HairDresserSaveQueryHandler : IRequestHandler<HairDresserSaveQuery, ResponseModel<bool>>
    {
        private readonly KuaforEvdeContext _db;
        public HairDresserSaveQueryHandler(KuaforEvdeContext db)
        {
            _db = db;
        }

        public async Task<ResponseModel<bool>> Handle(HairDresserSaveQuery request, CancellationToken cancellationToken)
        {
            var entity = new Data.DB.HairDresser
            {
                AccountId = request.AccountId,
                AddressText = request.AddressText,
                CityId = request.CityId,
                Description = request.Description,
                CreatedDate = DateTime.Now,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                LastModifiedDate = DateTime.Now,
                DisrtictId = request.DisrtictId,
                Status = (byte)Status.Active
            };
            foreach (var area in request.HairDresserArea)
            {
                entity.HairDresserArea.Add(new HairDresserArea
                {
                    DistrictId = area,
                    Status = (byte)Status.Active,
                    CreatedDate = DateTime.Now
                });
            }

            foreach (var hairDresserImage in request.HairDresserImageList)
            {
                entity.HairDresserImage.Add(new HairDresserImage
                {
                    Status = (byte)Status.Active,
                    CreatedDate = DateTime.Now,
                    AltText = hairDresserImage.AltText,
                    DisplayOrder = hairDresserImage.DisplayOrder,
                    ImageUrl = hairDresserImage.ImageUrl,
                    IsMain = hairDresserImage.IsMain
                });
            }

            foreach (var hairDresserService in request.HairDresserServiceList)
            {
                entity.HairDresserService.Add(new HairDresserService
                {
                    CreatedDate = DateTime.Now,
                    Status = (byte)Status.Active,
                    Gender = hairDresserService.Gender,
                    Price = hairDresserService.Price,
                    ServiceId = hairDresserService.ServiceId
                });
            }

            foreach (var hairDresserPerson in request.HairDresserPersonList)
            {
                entity.ContactPerson.Add(new ContactPerson
                {
                    Status = (byte)Status.Active,
                    CreatedDate = DateTime.Now,
                    Name = hairDresserPerson.Name,
                    Email = hairDresserPerson.Email,
                    LastName = hairDresserPerson.LastName,
                    Phone = hairDresserPerson.Phone
                });
            }

            await _db.HairDresser.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return new ResponseModel<bool>();
        }
    }
}
