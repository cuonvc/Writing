using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Writing.Migrations
{
    /// <inheritdoc />
    public partial class hi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_tbl_Users_tbl_UserId",
                table: "Comments_tbl");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Salt",
                table: "Users_tbl",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_tbl_Users_tbl_UserId",
                table: "Comments_tbl",
                column: "UserId",
                principalTable: "Users_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_tbl_Users_tbl_UserId",
                table: "Comments_tbl");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Salt",
                table: "Users_tbl",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments_tbl",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_tbl_Users_tbl_UserId",
                table: "Comments_tbl",
                column: "UserId",
                principalTable: "Users_tbl",
                principalColumn: "Id");
        }
    }
}
