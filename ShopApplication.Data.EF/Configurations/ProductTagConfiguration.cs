using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopApplication.Data.EF.Extensions;
using ShopApplication.Data.Entities;

namespace ShopApplication.Data.EF.Configurations
{
    public class ProductTagConfiguration : DbEntityConfiguration<ProductTag>
    {
        public override void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            entity.Property(c => c.TagId).HasMaxLength(50).IsRequired()
                .HasMaxLength(50).IsUnicode(false);
            // etc.
        }
    }
}
