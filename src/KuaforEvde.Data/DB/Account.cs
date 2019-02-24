using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class Account
    {
        public Account()
        {
            HairDresser = new HashSet<HairDresser>();
            Token = new HashSet<Token>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public byte Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime BirthDate { get; set; }
        public bool HasValidPhone { get; set; }
        public bool HasValidEmail { get; set; }
        public byte Status { get; set; }

        public virtual ICollection<HairDresser> HairDresser { get; set; }
        public virtual ICollection<Token> Token { get; set; }
    }
}
