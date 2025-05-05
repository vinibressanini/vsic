using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ChangedStatusFromIntToVarchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "tb_post",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "smallserial");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "tb_domain_event",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "tb_post",
                type: "smallserial",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "tb_domain_event",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
