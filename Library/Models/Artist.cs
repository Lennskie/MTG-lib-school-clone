using System;
using System.Collections.Generic;

namespace mtg_lib.Library.Models
{
    public partial class Artist
    {
        public Artist()
        {
            Cards = new HashSet<Card>();
        }

        public long Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
