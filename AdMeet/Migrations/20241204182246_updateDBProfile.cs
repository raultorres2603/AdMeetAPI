using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdMeet.Migrations
{
    /// <inheritdoc />
    public partial class updateDBProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profile_ProfileIdUser",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileIdUser",
                table: "Users",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Birthday",
                table: "Profile",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Profile",
                type: "varchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "Profile",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profile_ProfileIdUser",
                table: "Users",
                column: "ProfileIdUser",
                principalTable: "Profile",
                principalColumn: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profile_ProfileIdUser",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "Profile");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ProfileIdUser",
                keyValue: null,
                column: "ProfileIdUser",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileIdUser",
                table: "Users",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profile_ProfileIdUser",
                table: "Users",
                column: "ProfileIdUser",
                principalTable: "Profile",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
