using System;
using System.Collections.Generic;

namespace WWSI_Shop.Persistence.MySQL.Model;

public partial class OrderItem
{
    public ulong IdOrderItem { get; set; }

    public ulong IdOrder { get; set; }

    public ulong IdProduct { get; set; }

    public string Sku { get; set; } = null!;

    public decimal Price { get; set; }

    public uint Quantity { get; set; }

    public decimal? Discount { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
