using System;
using System.Collections.Generic;

namespace KuaforEvde.Data.DB
{
    public partial class Token
    {
        public int Id { get; set; }
        public string TokenKey { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual Account Account { get; set; }
    }
}
