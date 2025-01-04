using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetGroupIdsToMoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "ReceiptRecordGroup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "IncomeSource",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "Income",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "GiftCard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "BudgetType",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "ReceiptRecordGroup");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "IncomeSource");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "Income");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "GiftCard");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "BudgetType");
        }
    }
}
