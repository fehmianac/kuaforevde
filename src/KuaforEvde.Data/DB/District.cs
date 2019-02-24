using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class District
    {
        public District()
        {
            HairDresser = new HashSet<HairDresser>();
            HairDresserArea = new HashSet<HairDresserArea>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte Status { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<HairDresser> HairDresser { get; set; }
        public virtual ICollection<HairDresserArea> HairDresserArea { get; set; }
    }
}
