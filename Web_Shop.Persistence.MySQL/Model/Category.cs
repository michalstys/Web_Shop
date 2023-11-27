using System;
using System.Collections.Generic;

namespace WWSI_Shop.Persistence.MySQL.Model;

public partial class Category
{
    public uint IdCategory { get; set; }

    public uint? ParentIdCategory { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Category> InverseParentIdCategoryNavigation { get; set; } = new List<Category>();

    public virtual Category? ParentIdCategoryNavigation { get; set; }

    public virtual ICollection<Product> IdProducts { get; set; } = new List<Product>();
}
