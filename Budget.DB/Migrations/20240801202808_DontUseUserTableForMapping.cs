using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class DontUseUserTableForMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_BudgetingGroup_BudgetingGroupId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_BudgetingGroupId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsGroupAdmin",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupAdmin",
                table: "UsersBudgetingGroup",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroupAdmin",
                table: "UsersBudgetingGroup");

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupAdmin",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_User_BudgetingGroupId",
                table: "User",
                column: "BudgetingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_BudgetingGroup_BudgetingGroupId",
                table: "User",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
