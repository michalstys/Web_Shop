using System;
using System.Collections.Generic;

namespace WWSI_Shop.Persistence.MySQL.Model;

public partial class Order
{
    public ulong IdOrder { get; set; }

    public ulong IdCustomer { get; set; }

    public ulong IdShippingAddress { get; set; }

    public string OrderNumber { get; set; } = null!;

    public virtual Customer IdCustomerNavigation { get; set; } = null!;

    public virtual Address IdShippingAddressNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Invoice> IdInvoices { get; set; } = new List<Invoice>();
}
