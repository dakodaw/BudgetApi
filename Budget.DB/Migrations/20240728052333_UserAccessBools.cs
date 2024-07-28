using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class UserAccessBools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUserAdmin",
                table: "User",
                newName: "IsSystemAdmin");

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupAdmin",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroupAdmin",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "IsSystemAdmin",
                table: "User",
                newName: "IsUserAdmin");
        }
    }
}
