using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdMeet.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profile_ProfileIdUser",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfileIdUser",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileIdUser",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profile_Id",
                table: "Users",
                column: "Id",
                principalTable: "Profile",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profile_Id",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ProfileIdUser",
                table: "Users",
                type: "varchar(100)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileIdUser",
                table: "Users",
                column: "ProfileIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profile_ProfileIdUser",
                table: "Users",
                column: "ProfileIdUser",
                principalTable: "Profile",
                principalColumn: "IdUser");
        }
    }
}
