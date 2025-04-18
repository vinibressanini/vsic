using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infra.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_category",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_category", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_domain_event",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    @event = table.Column<string>(name: "event", type: "varchar(50)", nullable: false),
                    payload = table.Column<string>(type: "varchar", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_domain_event", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_post",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", nullable: false),
                    content = table.Column<string>(type: "varchar", nullable: false),
                    slug = table.Column<string>(type: "varchar", nullable: false),
                    status = table.Column<int>(type: "smallserial", nullable: false),
                    publish_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_post", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_user",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar", nullable: false),
                    email = table.Column<string>(type: "varchar", nullable: false),
                    password = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_post_category",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_post_category", x => new { x.CategoriesId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_tb_post_category_tb_category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "tb_category",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_post_category_tb_post_PostsId",
                        column: x => x.PostsId,
                        principalTable: "tb_post",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_comment",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "varchar", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_comment", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_comment_tb_post_PostId",
                        column: x => x.PostId,
                        principalTable: "tb_post",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_comment_tb_user_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "tb_user",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_favorites",
                columns: table => new
                {
                    FavoritedById = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoritesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_favorites", x => new { x.FavoritedById, x.FavoritesId });
                    table.ForeignKey(
                        name: "FK_tb_favorites_tb_post_FavoritesId",
                        column: x => x.FavoritesId,
                        principalTable: "tb_post",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_favorites_tb_user_FavoritedById",
                        column: x => x.FavoritedById,
                        principalTable: "tb_user",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_comment_AuthorId",
                table: "tb_comment",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_comment_PostId",
                table: "tb_comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_favorites_FavoritesId",
                table: "tb_favorites",
                column: "FavoritesId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_post_category_PostsId",
                table: "tb_post_category",
                column: "PostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_comment");

            migrationBuilder.DropTable(
                name: "tb_domain_event");

            migrationBuilder.DropTable(
                name: "tb_favorites");

            migrationBuilder.DropTable(
                name: "tb_post_category");

            migrationBuilder.DropTable(
                name: "tb_user");

            migrationBuilder.DropTable(
                name: "tb_category");

            migrationBuilder.DropTable(
                name: "tb_post");
        }
    }
}
