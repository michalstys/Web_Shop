using System;
using System.Collections.Generic;

namespace WWSI_Shop.Persistence.MySQL.Model;

public partial class ProductReview
{
    public long IdProductReview { get; set; }

    public ulong IdProduct { get; set; }

    public string Description { get; set; } = null!;

    public short Rating { get; set; }

    public DateOnly ReviewDate { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;
}
