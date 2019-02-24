using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class HairDresserService
    {
        public int Id { get; set; }
        public int HairDresserId { get; set; }
        public int ServiceId { get; set; }
        public decimal Price { get; set; }
        public byte Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte Status { get; set; }

        public virtual HairDresser HairDresser { get; set; }
        public virtual Service Service { get; set; }
    }
}
