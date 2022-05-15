using System;
using System.Collections.Generic;

namespace mtg_lib.Library.Models
{
    public partial class UserCard
    {
        public string UserId { get; set; } = null!;
        public long CardId { get; set; }
        public int? Cards { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
