using System;
using System.Collections.Generic;

namespace WWSI_Shop.Persistence.MySQL.Model;

public partial class Invoice
{
    public ulong IdInvoice { get; set; }

    public ulong IdCustomer { get; set; }

    public string Number { get; set; } = null!;

    public DateOnly IssueDate { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public virtual Customer IdCustomerNavigation { get; set; } = null!;

    public virtual ICollection<Order> IdOrders { get; set; } = new List<Order>();
}
