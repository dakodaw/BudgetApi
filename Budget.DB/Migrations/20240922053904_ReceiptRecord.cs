using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptRecordGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptRecordGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptRecordGroup_BudgetType_BudgetTypeId",
                        column: x => x.BudgetTypeId,
                        principalTable: "BudgetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptRecordGroup_ReceiptRecord_ReceiptRecordId",
                        column: x => x.ReceiptRecordId,
                        principalTable: "ReceiptRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptRecordGroup_BudgetTypeId",
                table: "ReceiptRecordGroup",
                column: "BudgetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptRecordGroup_ReceiptRecordId",
                table: "ReceiptRecordGroup",
                column: "ReceiptRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptRecordGroup");

            migrationBuilder.DropTable(
                name: "ReceiptRecord");
        }
    }
}
