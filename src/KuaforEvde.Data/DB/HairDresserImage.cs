using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class HairDresserImage
    {
        public int Id { get; set; }
        public int HairDresserId { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AltText { get; set; }
        public byte IsMain { get; set; }
        public int Status { get; set; }

        public virtual HairDresser HairDresser { get; set; }
    }
}
