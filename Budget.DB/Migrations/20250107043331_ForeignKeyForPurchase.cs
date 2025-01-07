using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyForPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BudgetingGroupId",
                table: "Purchases",
                column: "BudgetingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_BudgetingGroup_BudgetingGroupId",
                table: "Purchases",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_BudgetingGroup_BudgetingGroupId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_BudgetingGroupId",
                table: "Purchases");
        }
    }
}
