using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class HairDresser
    {
        public HairDresser()
        {
            ContactPerson = new HashSet<ContactPerson>();
            HairDresserArea = new HashSet<HairDresserArea>();
            HairDresserImage = new HashSet<HairDresserImage>();
            HairDresserService = new HashSet<HairDresserService>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CityId { get; set; }
        public int DisrtictId { get; set; }
        public string AddressText { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public byte Status { get; set; }

        public virtual Account Account { get; set; }
        public virtual City City { get; set; }
        public virtual District Disrtict { get; set; }
        public virtual ICollection<ContactPerson> ContactPerson { get; set; }
        public virtual ICollection<HairDresserArea> HairDresserArea { get; set; }
        public virtual ICollection<HairDresserImage> HairDresserImage { get; set; }
        public virtual ICollection<HairDresserService> HairDresserService { get; set; }
    }
}
