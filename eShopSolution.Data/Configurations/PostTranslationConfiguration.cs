using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class PostTranslationConfiguration : IEntityTypeConfiguration<PostTranslation>
    {
        public void Configure(EntityTypeBuilder<PostTranslation> builder)
        {
            builder.ToTable("PostTranslations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.LanguageId).IsUnicode(false).IsRequired().HasMaxLength(5);

            builder.HasOne(x => x.Language).WithMany(x => x.PostTranslations).HasForeignKey(x => x.LanguageId);

            builder.HasOne(x => x.Post).WithMany(x => x.PostTranslations).HasForeignKey(x => x.PostId);

        }
    }
}
