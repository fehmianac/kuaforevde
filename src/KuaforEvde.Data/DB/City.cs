﻿using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class City
    {
        public City()
        {
            District = new HashSet<District>();
            HairDresser = new HashSet<HairDresser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte Status { get; set; }

        public virtual ICollection<District> District { get; set; }
        public virtual ICollection<HairDresser> HairDresser { get; set; }
    }
}
