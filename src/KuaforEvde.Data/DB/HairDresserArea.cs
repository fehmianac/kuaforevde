using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class HairDresserArea
    {
        public int Id { get; set; }
        public int HairDresserId { get; set; }
        public int DistrictId { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte Status { get; set; }

        public virtual District District { get; set; }
        public virtual HairDresser HairDresser { get; set; }
    }
}
