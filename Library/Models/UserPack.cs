using System;
using System.Collections.Generic;

namespace mtg_lib.Library.Models
{
    public partial class UserPack
    {
        public string UserId { get; set; } = null!;
        public int Packs { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
    }
}
