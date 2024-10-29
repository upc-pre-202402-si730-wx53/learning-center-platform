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
            modelBuilder.Entity<Asset>().HasDiscriminator<string>("asset_type")
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

            modelBuilder.UseSnakeCaseWithPluralizedNamingConvention();

        }
    }
}
