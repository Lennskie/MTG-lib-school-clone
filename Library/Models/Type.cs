using System;
using System.Collections.Generic;

namespace mtg_lib.Library.Models
{
    public partial class Type
    {
        public Type()
        {
            CardTypes = new HashSet<CardType>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type1 { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<CardType> CardTypes { get; set; }
    }
}
