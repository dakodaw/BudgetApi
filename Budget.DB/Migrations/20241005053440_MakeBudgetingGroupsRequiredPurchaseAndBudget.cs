using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class MakeBudgetingGroupsRequiredPurchaseAndBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_BudgetingGroup_BudgetingGroupId",
                table: "Budget");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "ReceiptRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Budget",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_BudgetingGroup_BudgetingGroupId",
                table: "Budget",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_BudgetingGroup_BudgetingGroupId",
                table: "Budget");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "ReceiptRecord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Purchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Budget",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_BudgetingGroup_BudgetingGroupId",
                table: "Budget",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id");
        }
    }
}
