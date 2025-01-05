using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetGroupIdsRequiredNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetingGroupId",
                table: "ReceiptRecordGroup");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "UsersBudgetingGroup",
                newName: "UserBudgetingGroupId");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "IncomeSource",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Income",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "GiftCard",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "BudgetType",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_BudgetingGroupId",
                table: "Settings",
                column: "BudgetingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptRecord_BudgetingGroupId",
                table: "ReceiptRecord",
                column: "BudgetingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSource_BudgetingGroupId",
                table: "IncomeSource",
                column: "BudgetingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_BudgetingGroupId",
                table: "Income",
                column: "BudgetingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCard_BudgetingGroupId",
                table: "GiftCard",
                column: "BudgetingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_BudgetingGroup_BudgetingGroupId",
                table: "GiftCard",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Income_BudgetingGroup_BudgetingGroupId",
                table: "Income",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeSource_BudgetingGroup_BudgetingGroupId",
                table: "IncomeSource",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptRecord_BudgetingGroup_BudgetingGroupId",
                table: "ReceiptRecord",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_BudgetingGroup_BudgetingGroupId",
                table: "Settings",
                column: "BudgetingGroupId",
                principalTable: "BudgetingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_BudgetingGroup_BudgetingGroupId",
                table: "GiftCard");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_BudgetingGroup_BudgetingGroupId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeSource_BudgetingGroup_BudgetingGroupId",
                table: "IncomeSource");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptRecord_BudgetingGroup_BudgetingGroupId",
                table: "ReceiptRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_BudgetingGroup_BudgetingGroupId",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_BudgetingGroupId",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptRecord_BudgetingGroupId",
                table: "ReceiptRecord");

            migrationBuilder.DropIndex(
                name: "IX_IncomeSource_BudgetingGroupId",
                table: "IncomeSource");

            migrationBuilder.DropIndex(
                name: "IX_Income_BudgetingGroupId",
                table: "Income");

            migrationBuilder.DropIndex(
                name: "IX_GiftCard_BudgetingGroupId",
                table: "GiftCard");

            migrationBuilder.RenameColumn(
                name: "UserBudgetingGroupId",
                table: "UsersBudgetingGroup",
                newName: "GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Settings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BudgetingGroupId",
                table: "ReceiptRecordGroup",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "IncomeSource",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "Income",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "GiftCard",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetingGroupId",
                table: "BudgetType",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
