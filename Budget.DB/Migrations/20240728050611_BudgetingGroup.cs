using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class BudgetingGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupBudgetId",
                table: "User",
                newName: "BudgetingGroupId");

            migrationBuilder.CreateTable(
                name: "BudgetingGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetingGroup", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_BudgetingGroup_BudgetingGroupId",
                table: "User");

            migrationBuilder.DropTable(
                name: "BudgetingGroup");

            migrationBuilder.DropIndex(
                name: "IX_User_BudgetingGroupId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "BudgetingGroupId",
                table: "User",
                newName: "GroupBudgetId");
        }
    }
}
