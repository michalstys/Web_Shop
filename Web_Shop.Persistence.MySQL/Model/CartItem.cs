using System;
using System.Collections.Generic;

namespace WWSI_Shop.Persistence.MySQL.Model;

public partial class CartItem
{
    public ulong IdCartItem { get; set; }

    public ulong IdCart { get; set; }

    public ulong IdProduct { get; set; }

    public virtual Cart IdCartNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
