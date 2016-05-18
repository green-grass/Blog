using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using PhantomNet.Blog.JDev.Models;

namespace PhantomNet.Blog.JDev.Migrations.BlogDb
{
    [DbContext(typeof(BlogDbContext))]
    partial class BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("Relational:DefaultSchema", "Blog")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PhantomNet.Blog.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArticleId");

                    b.Property<string>("Author");

                    b.Property<int?>("BloggerId");

                    b.Property<int?>("CategoryId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Content");

                    b.Property<DateTime>("DataCreateDate");

                    b.Property<DateTime>("DataLastModifyDate");

                    b.Property<string>("DescriptionMeta");

                    b.Property<string>("Filters");

                    b.Property<bool>("IsActive");

                    b.Property<string>("KeywordsMeta");

                    b.Property<string>("Language");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Series");

                    b.Property<string>("ShortContent");

                    b.Property<string>("SourceUrl");

                    b.Property<string>("Tags");

                    b.Property<string>("Title");

                    b.Property<string>("UrlFriendlyTitle")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("ViewCount");

                    b.HasKey("Id");

                    b.HasIndex("UrlFriendlyTitle")
                        .IsUnique()
                        .HasAnnotation("Relational:Name", "UrlFriendlyTitleIndex");
                });

            modelBuilder.Entity("PhantomNet.Blog.Blogger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Biography");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Filters");

                    b.Property<string>("FullName");

                    b.Property<string>("Introduction");

                    b.Property<bool>("IsActive");

                    b.Property<string>("PenName");

                    b.Property<string>("UrlFriendlyPenName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("UrlFriendlyPenName")
                        .IsUnique()
                        .HasAnnotation("Relational:Name", "UrlFriendlyPenNameIndex");
                });

            modelBuilder.Entity("PhantomNet.Blog.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("DescriptionMeta");

                    b.Property<string>("Filters");

                    b.Property<bool>("IsActive");

                    b.Property<string>("KeywordsMeta");

                    b.Property<string>("Language");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.Property<int>("Position");

                    b.Property<string>("UrlFriendlyName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("UrlFriendlyName")
                        .IsUnique()
                        .HasAnnotation("Relational:Name", "UrlFriendlyNameIndex");
                });

            modelBuilder.Entity("PhantomNet.Blog.Article", b =>
                {
                    b.HasOne("PhantomNet.Blog.Article")
                        .WithMany()
                        .HasForeignKey("ArticleId");

                    b.HasOne("PhantomNet.Blog.Blogger")
                        .WithMany()
                        .HasForeignKey("BloggerId");

                    b.HasOne("PhantomNet.Blog.Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("PhantomNet.Blog.Category", b =>
                {
                    b.HasOne("PhantomNet.Blog.Category")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });
        }
    }
}
