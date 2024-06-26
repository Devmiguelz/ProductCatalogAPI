using System;
using System.Collections.Generic;

namespace ProductCatalogAPI.Domain.Entities
{
    public partial class Product
    {
        public Product()
        {
            ProductImage = new HashSet<ProductImage>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ProductImage> ProductImage { get; set; }
    }
}
