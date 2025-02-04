using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elixir.Entities;

public class ProductCategory : BaseEntity<Guid>
{
    public Guid ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }



}
