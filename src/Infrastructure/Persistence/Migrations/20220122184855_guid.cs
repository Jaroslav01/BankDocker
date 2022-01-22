using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    public partial class guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransceiverAccountNumber",
                table: "Transactions",
                newName: "SenderAccountNumber");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderAccountNumber",
                table: "Transactions",
                newName: "TransceiverAccountNumber");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
