using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class ContactPerson
    {
        public int Id { get; set; }
        public int HairDresserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual HairDresser HairDresser { get; set; }
    }
}
