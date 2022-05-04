using System;
using System.Collections.Generic;

namespace mtg_lib.Library.Models
{
    public partial class UserCoin
    {
        public string UserId { get; set; } = null!;
        public int Coins { get; set; }
        public bool CoinsClaimed { get; set; }
        public DateTime? ClaimedTimeStamp { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
    }
}
