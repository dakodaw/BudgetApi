using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiptRecordGroupIdToPurchases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "ReceiptRecord",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiptRecordGroupId",
                table: "Purchases",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "ReceiptRecord");

            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ReceiptRecordGroupId",
                table: "Purchases");
        }
    }
}
