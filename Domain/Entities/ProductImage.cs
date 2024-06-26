using System;
using System.Collections.Generic;

namespace ProductCatalogAPI.Domain.Entities
{
    public partial class ProductImage
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string FileName { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
