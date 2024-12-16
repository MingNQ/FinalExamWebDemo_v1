using System;
using System.Collections.Generic;

namespace FinalExamDemo.Models;

public partial class Provider
{
    public int Id { get; set; }

    public string ProviderName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
