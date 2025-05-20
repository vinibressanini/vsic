using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTablePostAddedNewColumnns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "tb_post",
                type: "varchar",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "views",
                table: "tb_post",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "tb_post");

            migrationBuilder.DropColumn(
                name: "views",
                table: "tb_post");
        }
    }
}
