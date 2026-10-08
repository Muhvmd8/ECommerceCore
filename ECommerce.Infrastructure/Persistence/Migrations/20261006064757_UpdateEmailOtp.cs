using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId",
                table: "EmailVerificationCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId1",
                table: "EmailVerificationCodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId1",
                table: "EmailVerificationCodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId",
                table: "EmailVerificationCodes",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId1",
                table: "EmailVerificationCodes",
                column: "ApplicationUserId1",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId",
                table: "EmailVerificationCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId1",
                table: "EmailVerificationCodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId1",
                table: "EmailVerificationCodes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId",
                table: "EmailVerificationCodes",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerificationCodes_ApplicationUser_ApplicationUserId1",
                table: "EmailVerificationCodes",
                column: "ApplicationUserId1",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
