using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class UsersBudgetingGroupMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersBudgetingGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    BudgetingGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersBudgetingGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersBudgetingGroup_BudgetingGroup_BudgetingGroupId",
                        column: x => x.BudgetingGroupId,
                        principalTable: "BudgetingGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersBudgetingGroup_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersBudgetingGroup_BudgetingGroupId",
                table: "UsersBudgetingGroup",
                column: "BudgetingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBudgetingGroup_UserId",
                table: "UsersBudgetingGroup",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersBudgetingGroup");
        }
    }
}
