using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace PhantomNet.Blog.JDev.Migrations.BlogDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema("Blog");
            migrationBuilder.CreateTable(
                name: "Blogger",
                schema: "Blog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Biography = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Filters = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    PenName = table.Column<string>(nullable: true),
                    UrlFriendlyPenName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogger", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Blog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DescriptionMeta = table.Column<string>(nullable: true),
                    Filters = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    KeywordsMeta = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    UrlFriendlyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Blog",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Article",
                schema: "Blog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleId = table.Column<int>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    BloggerId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DataCreateDate = table.Column<DateTime>(nullable: false),
                    DataLastModifyDate = table.Column<DateTime>(nullable: false),
                    DescriptionMeta = table.Column<string>(nullable: true),
                    Filters = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    KeywordsMeta = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Series = table.Column<string>(nullable: true),
                    ShortContent = table.Column<string>(nullable: true),
                    SourceUrl = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UrlFriendlyTitle = table.Column<string>(nullable: true),
                    ViewCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "Blog",
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_Blogger_BloggerId",
                        column: x => x.BloggerId,
                        principalSchema: "Blog",
                        principalTable: "Blogger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Blog",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateIndex(
                name: "UrlFriendlyTitleIndex",
                schema: "Blog",
                table: "Article",
                column: "UrlFriendlyTitle",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "UrlFriendlyPenNameIndex",
                schema: "Blog",
                table: "Blogger",
                column: "UrlFriendlyPenName",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "UrlFriendlyNameIndex",
                schema: "Blog",
                table: "Category",
                column: "UrlFriendlyName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Article", schema: "Blog");
            migrationBuilder.DropTable(name: "Blogger", schema: "Blog");
            migrationBuilder.DropTable(name: "Category", schema: "Blog");
        }
    }
}
