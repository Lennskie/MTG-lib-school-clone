using System;
using System.Collections.Generic;

namespace mtg_lib.Library.Models
{
    public partial class CardType
    {
        public long CardId { get; set; }
        public long TypeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual Type Type { get; set; } = null!;
    }
}
