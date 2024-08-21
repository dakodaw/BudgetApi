using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class BudgetingGroupInBudgetEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "Budget",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budget_BudgetingGroupId",
                table: "Budget",
                column: "BudgetingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_BudgetingGroup_BudgetingGroupId",
                table: "Budget",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_BudgetingGroup_BudgetingGroupId",
                table: "Budget");

            migrationBuilder.DropIndex(
                name: "IX_Budget_BudgetingGroupId",
                table: "Budget");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "Budget");
        }
    }
}
