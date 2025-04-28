using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_favorites_tb_post_FavoritesId",
                table: "tb_favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_favorites_tb_user_FavoritedById",
                table: "tb_favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_post_category_tb_category_CategoriesId",
                table: "tb_post_category");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_post_category_tb_post_PostsId",
                table: "tb_post_category");

            migrationBuilder.RenameColumn(
                name: "PostsId",
                table: "tb_post_category",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "tb_post_category",
                newName: "post_id");

            migrationBuilder.RenameIndex(
                name: "IX_tb_post_category_PostsId",
                table: "tb_post_category",
                newName: "IX_tb_post_category_category_id");

            migrationBuilder.RenameColumn(
                name: "FavoritesId",
                table: "tb_favorites",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "FavoritedById",
                table: "tb_favorites",
                newName: "post_id");

            migrationBuilder.RenameIndex(
                name: "IX_tb_favorites_FavoritesId",
                table: "tb_favorites",
                newName: "IX_tb_favorites_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_favorites_tb_post_post_id",
                table: "tb_favorites",
                column: "post_id",
                principalTable: "tb_post",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_favorites_tb_user_user_id",
                table: "tb_favorites",
                column: "user_id",
                principalTable: "tb_user",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_post_category_category_id",
                table: "tb_post_category",
                column: "category_id",
                principalTable: "tb_category",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_post_category_tb_post_post_id",
                table: "tb_post_category",
                column: "post_id",
                principalTable: "tb_post",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_favorites_tb_post_post_id",
                table: "tb_favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_favorites_tb_user_user_id",
                table: "tb_favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_post_category_category_id",
                table: "tb_post_category");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_post_category_tb_post_post_id",
                table: "tb_post_category");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "tb_post_category",
                newName: "PostsId");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "tb_post_category",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_post_category_category_id",
                table: "tb_post_category",
                newName: "IX_tb_post_category_PostsId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "tb_favorites",
                newName: "FavoritesId");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "tb_favorites",
                newName: "FavoritedById");

            migrationBuilder.RenameIndex(
                name: "IX_tb_favorites_user_id",
                table: "tb_favorites",
                newName: "IX_tb_favorites_FavoritesId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_favorites_tb_post_FavoritesId",
                table: "tb_favorites",
                column: "FavoritesId",
                principalTable: "tb_post",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_favorites_tb_user_FavoritedById",
                table: "tb_favorites",
                column: "FavoritedById",
                principalTable: "tb_user",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_post_category_tb_category_CategoriesId",
                table: "tb_post_category",
                column: "CategoriesId",
                principalTable: "tb_category",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_post_category_tb_post_PostsId",
                table: "tb_post_category",
                column: "PostsId",
                principalTable: "tb_post",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
