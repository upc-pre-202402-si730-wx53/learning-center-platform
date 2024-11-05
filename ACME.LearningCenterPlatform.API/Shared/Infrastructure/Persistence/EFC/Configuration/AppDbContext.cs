using ACME.LearningCenterPlatform.API.Profiles.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Model.Entities;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //Enable Created and Updated Date Interceptor
            optionsBuilder.AddCreatedUpdatedInterceptor();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(30);


            modelBuilder.Entity<Tutorial>().HasKey(t => t.Id);
            modelBuilder.Entity<Tutorial>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Tutorial>().Property(t => t.Title).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Tutorial>().Property(t => t.Summary).IsRequired().HasMaxLength(240);

            modelBuilder.Entity<Asset>().HasDiscriminator(a => a.AssetType);

            modelBuilder.Entity<Asset>().HasKey(a => a.Id);

            modelBuilder.Entity<Asset>().HasDiscriminator<string>("type")
                .HasValue<Asset>("asset_base")
                .HasValue<ImageAsset>("asset_image")
                .HasValue<VideoAsset>("asset_video")
                .HasValue<ReadableContentAsset>("asset_readable_content");


            modelBuilder.Entity<Asset>().OwnsOne(i => i.AssetIdentifier, ai =>
            {
                ai.WithOwner().HasForeignKey("Id");
                ai.Property(P => P.Identifier).HasColumnName("AssetIdentifier");
            });

            modelBuilder.Entity<ImageAsset>().Property(p => p.ImageUri).IsRequired();
            modelBuilder.Entity<VideoAsset>().Property(p => p.VideoUri).IsRequired();
            modelBuilder.Entity<Tutorial>().HasMany(t => t.Assets);

            // Category Relationships
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Tutorials)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .HasPrincipalKey(c => c.Id);

            // Profiles Context
            modelBuilder.Entity<Profile>().HasKey(p => p.Id);
            modelBuilder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Profile>().OwnsOne(p => p.Name,
                n =>
                {
                    n.WithOwner().HasForeignKey("Id");
                    n.Property(p => p.FirstName).HasColumnName("FirstName");
                    n.Property(p => p.LastName).HasColumnName("LastName");
                });
            modelBuilder.Entity<Profile>().OwnsOne(p => p.Email,
                e =>
                {
                    e.WithOwner().HasForeignKey("Id");
                    e.Property(p => p.Address).HasColumnName("EmailAddress");
                });
            modelBuilder.Entity<Profile>().OwnsOne(p => p.Address,
                sa =>
                {
                    sa.WithOwner().HasForeignKey("Id");
                    sa.Property(p => p.Street).HasColumnName("AddressStreet");
                    sa.Property(p => p.Number).HasColumnName("AddressNumber");
                    sa.Property(p => p.City).HasColumnName("AddressCity");
                    sa.Property(p => p.PostalCode).HasColumnName("AddressPostalCode");
                    sa.Property(p => p.Country).HasColumnName("AddressCountry");
                });

            modelBuilder.UseSnakeCaseWithPluralizedNamingConvention();

        }
    }
}
