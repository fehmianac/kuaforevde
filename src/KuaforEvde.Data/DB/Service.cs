using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class Service
    {
        public Service()
        {
            HairDresserService = new HashSet<HairDresserService>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool ForMale { get; set; }
        public bool ForFemale { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte Status { get; set; }

        public virtual ICollection<HairDresserService> HairDresserService { get; set; }
    }
}
