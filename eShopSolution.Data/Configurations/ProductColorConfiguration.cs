using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            builder.ToTable("ProductColors");
            builder.HasKey(x => x.Id);

         //   builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.Product).WithMany(x => x.ProductColor).HasForeignKey(x => x.ProductId);
        }
    }
}
